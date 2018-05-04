using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Affdex;
using System.Drawing;
using System.Threading;
using System.Reflection;
using NDesk.Options;

namespace ProcessVideoFile
{
    class Program
    {
        const int defaultFrameRate = 30;
        const uint defaultMaxNumFaces = 1;
        const FaceDetectorMode defaultDetectorMode = FaceDetectorMode.LARGE_FACES;

        const string defaultClassifierPath = @"C:\Program Files\Affectiva\AffdexSDK\data";
        const string defaultExpressionsConfigfile = "expressions.txt";
        const string logFileName = "Log.txt";

        static string infoFileName1;
        static string infoFileName2;
        static string infoFileName3;

        const string suffixExpressions = "_expressions.txt";
        const string suffixEmotions = "_emotions.txt";
        const string suffixFeaturePoints = "_featurepoints.txt";

        static VideoDetector detector;
        static ProcessVideoFeed pvd;
        static Status status;
        static FaceTracker tracker;

        static bool writeExpressions = true;
        static bool writeEmotions = false;
        static bool writeFeaturePoints = false;

        static void Main(string[] args)
        {
            try
            {
                bool show_help = false;
                string videoToPredict = null;
                string folderSamplesPath = null;
                string outputBayesInfoFile = null;
                string expressionsConfigFname = null;
                string inputBayesInfoFile = null;

                bool countExpressions = false;
                bool naiveBayesFullInfo = false;

                var p = new OptionSet()

                {
                    { "p|PredictVideo=", "the {NAME} of video to predict", v => videoToPredict = v },
                    { "s|SamplesVideoPath=", "the {NAME} of sample videos to train classifier", v => folderSamplesPath = v },
                    { "o|OutputBayesInfoFile=", "{NAME} file to write all output classifier data", v => outputBayesInfoFile = v },
                    { "i|InputBayesInfoFile=", "{NAME} file to write all input classifier data", v => inputBayesInfoFile = v },
                    { "c|CountExpressions=",  "writes counted expressions", v => countExpressions = v != null },
                    { "f|NaiveBayesFullInfo=", "shows full train input and output for classifier", v => naiveBayesFullInfo = v != null },
                    { "e|ExpressionsConfig=", string.Format( "path of file expressions config file. Default is .\\{0}", defaultExpressionsConfigfile),
                        v => expressionsConfigFname = v},
                    { "h|help",  "show this message and exit", v => show_help = v != null }
                };

                p.Parse(args);

                if (show_help)
                {
                    ShowHelp(p);
                    return;
                }

                List<string[]> samples = null;
                List<bool> samplesBool = null;
                string[] boundsConfig;
                if (expressionsConfigFname != null)
                    boundsConfig = File.ReadAllLines(expressionsConfigFname);
                else
                    boundsConfig = File.ReadAllLines(defaultExpressionsConfigfile);

                if (folderSamplesPath != null)
                {
                    
                    string[] filenames = null;
                    if (Directory.Exists(folderSamplesPath)) filenames = Directory.GetFiles(folderSamplesPath);
                    else throw new Exception("Directory not found");

                    samples = new List<string[]>();
                    samplesBool = new List<bool>();

                    foreach (string name in filenames)
                    {
                        if (name.Contains(".mp4"))
                        {
                            Log(string.Format("Start processing video {0}", name));

                            ProcessVideo(name);

                            Analyser analysis = new Analyser(pvd.GetFaceData(), boundsConfig);
                            if (countExpressions) WriteInfo1(analysis.CountedExpressions());

                            string[] arr = analysis.BayesNaiveInfo1();

                            samples.Add(arr);
                            if (name.Contains("true")) samplesBool.Add(true);
                            else samplesBool.Add(false);

                            Log(string.Format("Processing {0} done!", name));
                        }
                    }
                    if (inputBayesInfoFile != null)
                        for (int i = 0; i < samples.Count; i++)
                            File.AppendAllText(inputBayesInfoFile, string.Format("{0}{1}{2}", samplesBool[i], ToTextLine(samples[i]), Environment.NewLine));

                    Log(string.Format("All samples done!"));
                    Console.ReadKey();
                }



                if (videoToPredict != null)
                {
                    if ((samples == null) && (samplesBool == null))
                        if (inputBayesInfoFile != null)
                        {
                            string[] lines = File.ReadAllLines(inputBayesInfoFile);
                            samples = new List<string[]>();
                            samplesBool = new List<bool>();

                            foreach (string line in lines)
                            {
                                string[] keywords = line.Split(' ');
                                samplesBool.Add(bool.Parse(keywords[0]));
                                Array.Copy(keywords, 1, keywords, 0, keywords.Length - 1);
                                samples.Add(keywords);
                            }
                                
                        }

                    NaiveBayesClassifier classifier = new NaiveBayesClassifier(samples, samplesBool);

                     

                    ProcessVideo(videoToPredict);
                    Analyser a = new Analyser(pvd.GetFaceData(), boundsConfig);

                    double prob = classifier.Predict(a.BayesNaiveInfo1(), out bool isrock);
                    Console.WriteLine("{0} {1}", isrock, prob);
                }
                Console.Read();
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try '--help' for more information.");
                return;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                Console.ReadKey();
            }

        }



        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: greet [OPTIONS]+ message");
            Console.WriteLine("Greet a list of individuals with an optional message.");
            Console.WriteLine("If no message is specified, a generic greeting is used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        private static void ProcessVideo(string fileName)
        {
            string videoname = fileName.Split('.')[0];

            infoFileName1 = videoname + suffixExpressions;
            infoFileName2 = videoname + suffixEmotions;
            infoFileName3 = videoname + suffixFeaturePoints;

            File.Delete(infoFileName1);
            File.Delete(infoFileName2);
            File.Delete(infoFileName3);

            //initialize detector
            detector = new VideoDetector(defaultFrameRate, defaultMaxNumFaces, defaultDetectorMode);
            detector.setClassifierPath(defaultClassifierPath);

            //initialize callback functions
            pvd = new ProcessVideoFeed(detector, ShowMessage, WriteInfo1, WriteInfo2, WriteInfo3);
            status = new Status(detector, ShowMessage, WriteInfo1);
            //tracker = new FaceTracker(detector, ShowMessage, WriteInfo1);

            //set detector's options
            detector.setDetectAllExpressions(true);
            detector.setDetectAllEmotions(true);

            //TO DO: make for appearances
            //detector.setDetectAllAppearances(true);

            //start and process detector
            detector.start();
            detector.process(fileName);

            //wait until it all done
            while (!status.IsReady)
                Thread.Sleep(100);


            //stop detector
            detector.stop();

            Log(String.Format("Video processing done! {0} frames were captured, {1} frames was processed", pvd.GetFrameCapturedCount(), pvd.GetFrameProcessedCount()));

        }


        private static void Log(string message)
        {
            message = string.Format("{0} {1}{2}", DateTime.Now, message, Environment.NewLine);
            Console.Write(message);
            File.AppendAllText(logFileName, message);
        }

        private static void ShowMessage(string message)
        {
            Log(message);
        }

        private static void WriteInfo1(string info)
        {
            if (writeExpressions) File.AppendAllText(infoFileName1, info);
        }

        private static void WriteInfo2(string info)
        {
            if (writeEmotions) File.AppendAllText(infoFileName2, info);
        }

        private static void WriteInfo3(string info)
        {
            if (writeFeaturePoints) File.AppendAllText(infoFileName3, info);
        }

        private static string ToTextLine(string[] arr)
        {
            StringBuilder str = new StringBuilder();
            foreach (var s in arr)
                str.Append(" " + s);
            return str.ToString();
        }

    }
}
