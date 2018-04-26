using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Affdex;

namespace ProcessVideoFile
{
    class FaceData
    {
        private List<Face> faceStates;
        private List<float> timeStamps;

        private int frameWidth;
        private int frameHeight;


        public FaceData()
        {
            faceStates = new List<Face>();
            timeStamps = new List<float>();
        }

        public void SetFrameData(int h, int w)
        {
            frameWidth = w;
            frameHeight = h;

        }

        public void AddFace(Face face, float time)
        {
            faceStates.Add(face);
            timeStamps.Add(time);
        }

        public Face GetFace(int frameNum)
        {
            return faceStates[frameNum];
        }

        public float GetTimestamp(int frameNum)
        {
            return timeStamps[frameNum];
        }

        public int Count { get { return faceStates.Count; } }
        public int FrameHeight { get { return frameHeight; } }
        public int FrameWidth { get { return frameWidth; } }



    }

}
