using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Affdex;

namespace ProcessVideoFile
{
    class FaceTracker : FaceListener
    {

        public delegate void GetInfo(string message);

        private GetInfo ShowMessage;
        private GetInfo WriteInfo;

        public FaceTracker(VideoDetector detector, GetInfo showMessage, GetInfo writeInfo)
        {
            ShowMessage = showMessage;
            WriteInfo = writeInfo;
            detector.setFaceListener(this);
        }

        void FaceListener.onFaceFound(float timestamp, int faceId)
        {
            ShowMessage(String.Format("{0} face detected on {1}", faceId, timestamp));
        }

        void FaceListener.onFaceLost(float timestamp, int faceId)
        {
            ShowMessage("Face lost");
        }
    }
}
