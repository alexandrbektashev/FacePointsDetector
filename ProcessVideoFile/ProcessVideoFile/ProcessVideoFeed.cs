using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Affdex;


namespace ProcessVideoFile
{
    class ProcessVideoFeed : ImageListener
    {
        public delegate void GetInfo(string message);

        private GetInfo ShowMessage;
        private GetInfo WriteInfoExpressions;
        private GetInfo WriteInfoEmotions;
        private GetInfo WriteInfoFeaturePoints;

        private int frameCapturedCounter = 0;
        private int frameProcessedCounter = 0;

        private bool isFirstFrame = true;
        const int longestWordLenght = 19; //this is based on expressions properties

        private FaceData faceStates;

        public ProcessVideoFeed(VideoDetector detector,
            GetInfo showMessage, GetInfo writeInfoExpressions,
            GetInfo writeInfoEmotions, GetInfo writeInfoFeaturePoints)
        {
            ShowMessage = showMessage;
            WriteInfoExpressions = writeInfoExpressions;
            WriteInfoEmotions = writeInfoEmotions;
            WriteInfoFeaturePoints = writeInfoFeaturePoints;

            faceStates = new FaceData();

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

                    faceStates.AddFace(face, frame.getTimestamp());
                    

                    if (isFirstFrame)
                    {
                        faceStates.SetFrameData(frame.getHeight(), frame.getWidth());
                        output.Append(string.Format("{0,5}{1,9}", "Frame", "TimeScale"));
                        foreach (PropertyInfo prop in typeof(Affdex.Expressions).GetProperties())
                        {
                            output.Append(string.Format("#{0,19}", prop.Name));
                        }
                        output.Append(Environment.NewLine);
                        WriteInfoExpressions(output.ToString());
                        output.Clear();

                        output.Append(string.Format("{0,5}{1,9}", "Frame", "TimeScale"));
                        foreach (PropertyInfo prop in typeof(Affdex.Emotions).GetProperties())
                        {
                            output.Append(string.Format("#{0,19}", prop.Name));
                        }
                        output.Append(Environment.NewLine);
                        WriteInfoEmotions(output.ToString());
                        output.Clear();

                        output.Append("Face feature points" + Environment.NewLine);
                        WriteInfoFeaturePoints(output.ToString());
                        output.Clear();
                    }
                    isFirstFrame = false;

                    //timescale and frame count output
                    output.Append(string.Format("{0,4}#{1,9:0.0000}", frameProcessedCounter, frame.getTimestamp()));

                    //expressions output
                    foreach (PropertyInfo prop in typeof(Affdex.Expressions).GetProperties())
                    {
                        float value = (float)prop.GetValue(face.Expressions, null);
                        output.Append(string.Format("#{0,19:0.0000000}", value));
                    }
                    output.Append(Environment.NewLine);
                    WriteInfoExpressions(output.ToString());

                    //output of feature points
                    output.Clear();
                    var featurePoints = face.FeaturePoints;
                    foreach (var p in featurePoints)
                    {
                        output.Append(string.Format("{0:0}:{1:0};", p.X, p.Y));
                    }
                    output.Append(Environment.NewLine);
                    WriteInfoFeaturePoints(output.ToString());
                    output.Clear();

                    //timescale and frame count output
                    output.Append(string.Format("{0,4}#{1,9:0.0000}", frameProcessedCounter, frame.getTimestamp()));

                    //expressions output
                    foreach (PropertyInfo prop in typeof(Affdex.Emotions).GetProperties())
                    {
                        float value = (float)prop.GetValue(face.Emotions, null);
                        output.Append(string.Format("#{0,19:0.0000000}", value));
                    }
                    output.Append(Environment.NewLine);
                    WriteInfoEmotions(output.ToString());


                    //TO DO: make handler for appearances




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

        public FaceData GetFaceData()
        {
            return faceStates;
        }
    }
}
