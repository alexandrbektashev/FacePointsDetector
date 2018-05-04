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


        static void Main(string[] args)
        {
            try
            {
                bool show_help = false;
                string videoToPredict = null;
                string folderSamplesPath = null;
                string outputBayesInfoFile = null;
                string expressionsConfigFname = null;


                bool countExpressions = false;
                bool naiveBayesInput = false;
                bool naiveBayesFullInfo = false;

                var p = new OptionSet()

                {
                    { "p|PredictVideo=", "the {NAME} of video to predict", v => videoToPredict = v },
                    { "s|SamplesVideoPath=", "the {NAME} of sample videos to train classifier", v => videoToPredict = v },
                    { "o|OutputBayesInfoFile=", "output file to write all classifier data", v => outputBayesInfoFile = v },
                    { "c|CountExpressions=",  "writes counted expressions", v => countExpressions = v != null },
                    { "n|NaiveBayesInput=",  "writes what was received by naive Bayes classifier", v => naiveBayesInput = v != null },
                    { "f|NaiveBayesFullInfo=", "shows full train input and output for classifier", v => naiveBayesFullInfo = v != null },
                    { "e|ExpressionsConfig=", string.Format( "the path to expressions config file. Default is .\\{0}", defaultExpressionsConfigfile),
                        v => expressionsConfigFname = v},
                //{ "r|repeat=", "this must be an integer.", (int v) => repeat = v },
                //{ "t|type=", "trial type: date or times", v => {
                //if (v == "date") DateOrTimes = true;
                //else if (v == "times") DateOrTimes = false;
                //else throw new OptionException("Wrong parameter value", "t|type="); } },
                    { "h|help",  "show this message and exit", v => show_help = v != null }
                };

                p.Parse(args);
         
                if (show_help)
                {
                    ShowHelp(p);
                    return;
                }

                Dictionary<string[], bool> samples = new Dictionary<string[], bool>();
                string[] boundsConfig;
                if (expressionsConfigFname != null)
                    boundsConfig = File.ReadAllLines(expressionsConfigFname);
                else
                    boundsConfig = File.ReadAllLines(defaultExpressionsConfigfile);


                if (folderSamplesPath != null)
                {

                    string[] filenames = null;
                    if (!Directory.Exists(folderSamplesPath)) filenames = Directory.GetFiles(folderSamplesPath);
                    if (filenames == null) throw new Exception("No samples found");

                    foreach (string name in filenames)
                    {
                        if (name.Contains(".mp4"))
                        {
                            Log(string.Format("Start processing video {0}", name));

                            ProcessVideo(name);

                            Analyser analysis = new Analyser(pvd.GetFaceData(), boundsConfig);
                            if (countExpressions) counter = analysis.CountEverything();

                            string[] arr = analysis.BayesNaiveInfo1();

                            if (name.Contains("true")) samples.Add(arr, true);
                            else samples.Add(arr, false);

                            Log(string.Format("Processing {0} done!", name));
                            foreach (string str in arr)
                                WriteInfo1(str + " ");
                        }
                    }

                    Log(string.Format("All done!"));
                }
                Console.ReadKey();

                if (videoToPredict != null)
                {
                    Console.WriteLine("Going to predict");

                    NaiveBayesClassifier classifier = new NaiveBayesClassifier(samples);

                    string fname = @"C:\Users\aesth\Pictures\Camera Roll\test.mp4";
                    ProcessVideo(fname);
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
            tracker = new FaceTracker(detector, ShowMessage, WriteInfo1);

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
            File.AppendAllText(infoFileName1, info);
        }

        private static void WriteInfo2(string info)
        {
            File.AppendAllText(infoFileName2, info);
        }

        private static void WriteInfo3(string info)
        {
            File.AppendAllText(infoFileName3, info);
        }

    }
}
