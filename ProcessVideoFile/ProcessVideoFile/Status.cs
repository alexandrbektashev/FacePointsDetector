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
        public Status(VideoDetector detector)
        {
            detector.setProcessStatusListener(this);
        }

        void ProcessStatusListener.onProcessingException(AffdexException ex)
        {
            Console.WriteLine(  ex.Message);
        }

        void ProcessStatusListener.onProcessingFinished()
        {
            Console.WriteLine("Processing finished!");
        }
    }
}
