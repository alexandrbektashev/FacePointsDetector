using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Affdex;

namespace ProcessVideoFile
{
    class Status : ProcessStatusListener
    {
        public delegate void GetInfo(string message);

        private GetInfo ShowMessage;
        private GetInfo WriteInfo;

        public Status(VideoDetector detector, GetInfo showMessage, GetInfo writeInfo)
        {

            ShowMessage = showMessage;
            WriteInfo = writeInfo;
            detector.setProcessStatusListener(this);
        }

        void ProcessStatusListener.onProcessingException(AffdexException ex)
        {
            ShowMessage(ex.Message);
            WriteInfo(ex.Message);

        }

        void ProcessStatusListener.onProcessingFinished()
        {
            ShowMessage("Processing finished!");
            WriteInfo("Processing finished!");

        }
    }
}
