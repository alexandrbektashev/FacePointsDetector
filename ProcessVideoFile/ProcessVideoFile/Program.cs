using System;
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
        const string defaultLogFileName = "LogProcessedFile.txt";

        static void Main(string[] args)
        {
            try
            {
                //initialize detector
                VideoDetector detector = new VideoDetector(defaultFrameRate, defaultMaxNumFaces, defaultDetectorMode);
                detector.setClassifierPath(defaultClassifierPath);

                //initialize callback functions
                ProcessVideoFeed pvd = new ProcessVideoFeed(detector, ShowMessage, WriteInfo);
                Status status = new Status(detector, ShowMessage, WriteInfo);

                //set detector's options
                detector.setDetectAllEmotions(true);
                detector.setDetectSmirk(true);
                detector.setDetectGlasses(true);

                //start and process detector
                detector.start();
                detector.process(defaultVideFileName);

                //show some info
                Console.Read();


                //stop detector
                detector.stop();
                
                ShowMessage(String.Format("Video processing done! {0} frames were captured, {1} frames was processed", pvd.GetFrameCapturedCount(), pvd.GetFrameProcessedCount() ));
                Console.ReadKey();

            }

            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                WriteInfo(ex.Message);
            }

        }

        private static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        private static void WriteInfo(string info)
        {
            File.AppendAllText(defaultLogFileName, info);
        }
    }
}
