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

namespace ProcessVideoFile
{
    class Program
    {
        const int defaultFrameRate = 30;
        const uint defaultMaxNumFaces = 1;
        const FaceDetectorMode defaultDetectorMode = FaceDetectorMode.LARGE_FACES;

        const string defaultClassifierPath = @"C:\Program Files\Affectiva\AffdexSDK\data";        
        const string logFileName = "Log.txt";

//        static string defaultVideFileName = "video.mp4";
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
                //the upper level

                string[] boundsConfig = File.ReadAllLines("expressions.txt");



                //Dictionary<string[], bool> samples = new Dictionary<string[], bool>();

                //NaiveBayesClassifier classifier = new NaiveBayesClassifier(samples);

                //ShowMessage(classifier.Predict(new string[] { "nose", "smile" }, out bool istrue).ToString());
                //ShowMessage(istrue.ToString());

                string[] filenames = Directory.GetFiles(@"C:\Users\aesth\Pictures\Camera Roll\test");
                Dictionary<string, int> counter = new Dictionary<string, int>();

                foreach (string name in filenames)
                {
                    if (name.Contains(".mp4"))
                    {
                        Log(string.Format("Start processing video {0}", name));

                        ProcessVideo(name);

                        Analyser analysis = new Analyser(pvd.GetFaceData(), boundsConfig);
                        counter = analysis.CountEverything();
                        Log(string.Format("Processing {0} done!", name));
                    }
                }


                foreach(string str in counter.Keys)
                    Console.WriteLine("{0, 20} - {1,3}", str, counter[str]);

                Log(string.Format("All done!"));

                Console.ReadKey();
                

            }

            catch (Exception ex)
            {
                Log(ex.Message);
                Console.ReadKey();
            }

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
            Console.WriteLine(message);
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
