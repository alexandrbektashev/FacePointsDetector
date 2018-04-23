using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Affdex;

namespace AffdexSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {


            Affdex.Detector myDetector = new Affdex.CameraDetector(0, 30, 10, 1, Affdex.FaceDetectorMode.LARGE_FACES);


            myDetector.setClassifierPath("C:\\Program Files\\Affectiva\\AffdexSDK\\data");

            ProcessCameraFeed feedform = new ProcessCameraFeed(myDetector);
            myDetector.setDetectAllEmotions(true);
            myDetector.setDetectSmirk(true);
            myDetector.setDetectGlasses(true);

            

            myDetector.start();
            feedform.ShowDialog();
            myDetector.stop();
            

        }

    }
}
