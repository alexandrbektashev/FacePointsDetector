using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AffdexSampleApp
{
    public partial class ProcessCameraFeed : Form, Affdex.ImageListener
    {
        public ProcessCameraFeed(Affdex.Detector detector)
        {
            detector.setImageListener(this);
            InitializeComponent();
        }
        
        public void onImageCapture(Affdex.Frame frame)
        {
            frame.Dispose();
        }

        public void onImageResults(Dictionary<int, Affdex.Face> dict, Affdex.Frame frame)
        {
            if (dict != null)
            {
                foreach (KeyValuePair<int, Affdex.Face> pair in dict)
                {
                    Affdex.Face face = pair.Value;
                    
                    foreach (PropertyInfo prop in typeof(Affdex.Emotions).GetProperties())
                    {
                        float value = (float)prop.GetValue(face.Emotions, null);
                        string output = string.Format("{0}: {1:0.00}", prop.Name, value);
                        System.Console.WriteLine(output);
                    }

                }
            }
        }
    }

}
