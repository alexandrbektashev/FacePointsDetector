using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Affdex;


namespace ProcessVideoFile
{
    class ProcessVideoFeed : ImageListener
    {
        public delegate void GetInfo(string message);

        private GetInfo ShowMessage;
        private GetInfo WriteInfo;

        private int frameCapturedCounter = 0;
        private int frameProcessedCounter = 0;

        public ProcessVideoFeed(VideoDetector detector,
            GetInfo showMessage, GetInfo writeInfo)
        {
            ShowMessage = showMessage;
            WriteInfo = writeInfo;

            detector.setImageListener(this);

        }

        void ImageListener.onImageCapture(Frame frame)
        {
            frameCapturedCounter++;
            frame.Dispose();
        }

        void ImageListener.onImageResults(Dictionary<int, Face> faces, Frame frame)
        {
            if (faces != null)
            {
                foreach (KeyValuePair<int, Affdex.Face> pair in faces)
                {
                    Affdex.Face face = pair.Value;
                    StringBuilder output = new StringBuilder();
                    //foreach (PropertyInfo prop in typeof(Affdex.Emotions).GetProperties())
                    //{
                    //    float value = (float)prop.GetValue(face.Emotions, null);
                    //    string output = string.Format("{0}: {1:0.00}", prop.Name, value);
                    //}

                    foreach (PropertyInfo prop in typeof(Affdex.Expressions).GetProperties())
                    {
                        if (prop.Name == "EyeClosure")
                        {
                            float value = (float)prop.GetValue(face.Expressions, null);
                            ShowMessage(string.Format("{0} - {1:0.00}", prop.Name, value));
                        }
                    }
                        

                    var featurePoints = face.FeaturePoints;
                    

                    foreach (var p in featurePoints)
                    {
                        output.Append(string.Format("{0:0}:{1:0};", p.X, p.Y));
                    }

                    //WriteInfo(output.ToString());
                    frameProcessedCounter++;
                }
            }

        }

        public int GetFrameCapturedCount()
        {
            return frameCapturedCounter;
        }

        public int GetFrameProcessedCount()
        {
            return frameProcessedCounter;
        }
    }
}
