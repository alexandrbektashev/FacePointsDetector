using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Affdex;


namespace ProcessVideoFile
{
    class ProcessVideoFeed : ImageListener
    {
        

        public ProcessVideoFeed(VideoDetector detector)
        {
            
            detector.setImageListener(this);
        }

        void ImageListener.onImageCapture(Frame frame)
        {
            
            frame.Dispose();
        }

        void ImageListener.onImageResults(Dictionary<int, Face> faces, Frame frame)
        {
            if (faces != null)
            {
                foreach (KeyValuePair<int, Affdex.Face> pair in faces)
                {
                    Affdex.Face face = pair.Value;

                    //foreach (PropertyInfo prop in typeof(Affdex.Emotions).GetProperties())
                    //{
                    //    float value = (float)prop.GetValue(face.Emotions, null);
                    //    string output = string.Format("{0}: {1:0.00}", prop.Name, value);
                    //    System.Console.WriteLine(output);
                    //}

                    var featurePoints = face.FeaturePoints;
                    //StringBuilder output = new StringBuilder();

                    //foreach (var p in featurePoints)
                    //{
                    //    output.Append(string.Format("{0:0}:{1:0};", p.X, p.Y));
                    //}
                    Console.WriteLine("Success! {0} points were written.", featurePoints.Length);

                }
            }

        }
    }
}
