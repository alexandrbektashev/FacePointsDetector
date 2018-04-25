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
        void FaceListener.onFaceFound(float timestamp, int faceId)
        {
            throw new NotImplementedException();
        }

        void FaceListener.onFaceLost(float timestamp, int faceId)
        {
            throw new NotImplementedException();
        }
    }
}
