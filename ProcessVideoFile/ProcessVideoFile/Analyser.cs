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

namespace ProcessVideoFile
{
    class Analyser
    {

        //in this class I'm going to make some methods which would give information
        //input info - timestamp
        //output yes\no, confidence

        //I should also make a face model which will understand eyes position

        const float boundEyeClosure = 1.0F;
        
        private FaceData faceStates; //data field

        //TO DO: make it work with files

        public Analyser(FaceData _faceStates)
        {
            faceStates = _faceStates;
        }

        public int EyeClosureCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;
            
            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);
              
                if (isOpen && (face.Expressions.EyeClosure > boundEyeClosure))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.EyeClosure < boundEyeClosure)) isOpen = true;

            }

            return count;
        }
        public int EyeClosureCount()
        {
            int s = 0;
            int f = faceStates.Count - 1;
            return EyeClosureCount(s, f);
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

        
    }
}
