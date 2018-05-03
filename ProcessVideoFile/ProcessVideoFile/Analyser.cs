using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Affdex;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace ProcessVideoFile
{
    class Analyser
    {

        //in this class I'm going to make some methods which would give information
        //input info - timestamp
        //output yes\no, confidence

        //I should also make a face model which will understand eyes position

        Dictionary<string, double> bounds;
        Dictionary<string, bool> isOn;
        Dictionary<string, int> counter;

        private FaceData faceStates; //data field

        //TO DO: make it work with files



        public Analyser(FaceData _faceStates, string[] boundsConfig)
        {
            bounds = new Dictionary<string, double>();
            isOn = new Dictionary<string, bool>();
            foreach (string bound in boundsConfig)
            {
                bounds.Add(bound.Split(':')[0], Convert.ToDouble(bound.Split(':')[1]));
                isOn.Add(bound.Split(':')[0], true);
            }

            faceStates = _faceStates;
        }


        public Dictionary<string, int> CountEverything()
        {
            counter = new Dictionary<string, int>();
            for (int i = 0; i < faceStates.Count; i++)
            {
                Face face = faceStates.GetFace(i);

                foreach (PropertyInfo prop in typeof(Affdex.Expressions).GetProperties())
                {
                    if (!counter.Keys.Contains(prop.Name))
                        counter.Add(prop.Name, 0);

                    double valueFrame = Convert.ToDouble( (float)prop.GetValue(face.Expressions, null));
                    double valueBound = bounds[prop.Name];
                    bool ison = isOn[prop.Name];


                    if ((valueFrame > valueBound) && ison )
                    {
                        counter[prop.Name]++;
                        isOn[prop.Name] = false;
                    }
                    else if ((valueFrame < valueBound) && !ison)
                    {
                        isOn[prop.Name] = true;
                    }

                }
            }
            return counter;
        }


        public string IdToString()
        {
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < faceStates.Count; i++)
            {
                foreach (FeaturePoint fp in faceStates.GetFace(i).FeaturePoints)
                    output.Append(fp.Id + " ");
                output.Append(Environment.NewLine);
            }
            return output.ToString();
        }

        //should be rewritten
        public Bitmap GetFaceMap(int frameNum)
        {
            const int mapscale = 2; //develop this
            FeaturePoint[] fps = faceStates.GetFace(frameNum).FeaturePoints;

            Bitmap map = new Bitmap(faceStates.FrameWidth * mapscale, faceStates.FrameHeight * mapscale);

            Graphics g = Graphics.FromImage(map);

            Pen pen = new Pen(Brushes.Red, 2);

            Rectangle rectf;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, map.Width, map.Height));
            foreach (var point in fps)
            {
                Point p = new Point(mapscale * Convert.ToInt32(point.X), mapscale * Convert.ToInt32(point.Y));
                g.DrawEllipse(pen, new Rectangle(p, new Size(4, 4)));
                rectf = new Rectangle(p, new Size(20, 20));
                g.DrawString(point.Id.ToString(), new Font("Tahoma", 8), Brushes.Black, rectf);

            }           

            g.Flush();

            return map;
        }

        public string[] BayesNaiveInfo1()
        {
            Dictionary<string, int> dict = CountEverything();

            List<string> info = new List<string>();

            foreach (string key in dict.Keys)
                if (dict[key] > 0) info.Add(key);

            return info.ToArray();
        }

    }
}
