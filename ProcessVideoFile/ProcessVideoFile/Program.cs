﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Affdex;


namespace ProcessVideoFile
{
    class Program
    {
        const int defaultFrameRate = 30;
        const uint defaultMaxNumFaces = 1;
        const FaceDetectorMode defaultDetectorMode = FaceDetectorMode.LARGE_FACES;

        const string defaultClassifierPath = @"C:\Program Files\Affectiva\AffdexSDK\data";
        const string defaultVideFileName = "video.mp4";

        const string logFileName = "Log.txt";
        const string logFileName1 = "LogProcessedFileExpressions.txt";
        const string logFileName2 = "LogProcessedFileEmotions.txt";
        const string logFileName3 = "LogProcessedFileFeaturePoints.txt";

        static VideoDetector detector;
        static ProcessVideoFeed pvd;
        static Status status;
        static FaceTracker tracker;


        static void Main(string[] args)
        {
            try
            {
                File.Delete(logFileName1);
                File.Delete(logFileName2);
                File.Delete(logFileName3);

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
                
                //start and process detector
                detector.start();
                detector.process(defaultVideFileName);

                //show some info
                Console.Read();


                //stop detector
                detector.stop();

                ShowMessage(String.Format("Video processing done! {0} frames were captured, {1} frames was processed", pvd.GetFrameCapturedCount(), pvd.GetFrameProcessedCount()));
                Console.ReadKey();

            }

            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                WriteInfo1(ex.Message);
            }

        }

        private static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        private static void WriteInfo1(string info)
        {
            File.AppendAllText(logFileName1, info);
        }

        private static void WriteInfo2(string info)
        {
            File.AppendAllText(logFileName2, info);
        }

        private static void WriteInfo3(string info)
        {
            File.AppendAllText(logFileName3, info);
        }

    }
}
