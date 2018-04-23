using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        static void Main(string[] args)
        {

            VideoDetector detector = new VideoDetector(defaultFrameRate, defaultMaxNumFaces, defaultDetectorMode);
            detector.setClassifierPath(defaultClassifierPath);

            ProcessVideoFeed pvd = new ProcessVideoFeed(detector);
            Status status = new Status(detector);

            detector.setDetectAllEmotions(true);
            detector.setDetectSmirk(true);
            detector.setDetectGlasses(true);



            detector.start();
            detector.process(defaultVideFileName);

            Console.Read();
            detector.stop();

        }
    }
}
