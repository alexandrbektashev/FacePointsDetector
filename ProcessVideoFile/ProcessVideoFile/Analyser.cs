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

        const float boundEyeClosure = 1.0F;
        const float boundJawDrop = 0;
        const float boundLipStretch = 0;
        const float boundDimpler = 0;
        const float boundLidTighten = 0;
        const float boundCheekRaise = 0;
        const float boundEyeWiden = 0;
        const float boundAttention = 96;
        const float boundSmirk = 0;
        const float boundMouthOpen = 0;
        const float boundLipSuck = 0;
        const float boundLipPress = 0;
        const float boundLipPucker = 0;
        const float boundChinRaise = 0;
        const float boundLipCornerDepressor = 0;
        const float boundUpperLipRaise = 0;
        const float boundNoseWrinkle = 0;
        const float boundBrowFurrow = 10;
        const float boundBrowRaise = 0;
        const float boundInnerBrowRaise = 0;
        const float boundSmile = 0;

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

        public int AttentionDetectCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.Attention > boundAttention))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.Attention < boundAttention)) isOpen = true;

            }

            return count;
        }

        public int BrowFurrowCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.BrowFurrow > boundBrowFurrow))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.BrowFurrow < boundBrowFurrow)) isOpen = true;

            }

            return count;
        }

        public int BrowRaiseCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.BrowRaise > boundBrowRaise))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.BrowRaise < boundBrowRaise)) isOpen = true;

            }

            return count;
        }

        public int CheekRaiseCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.CheekRaise > boundCheekRaise))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.CheekRaise < boundCheekRaise)) isOpen = true;

            }

            return count;
        }

        public int ChinRaiseCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.ChinRaise > boundChinRaise))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.ChinRaise < boundChinRaise)) isOpen = true;

            }

            return count;
        }

        public int DimplerCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.Dimpler > boundDimpler))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.Dimpler < boundDimpler)) isOpen = true;

            }

            return count;
        }

        public int EyeWidenCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.EyeWiden > boundEyeWiden))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.EyeWiden < boundEyeWiden)) isOpen = true;

            }

            return count;
        }

        public int InnerBrowRaiseCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.InnerBrowRaise > boundInnerBrowRaise))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.InnerBrowRaise < boundInnerBrowRaise)) isOpen = true;

            }

            return count;
        }

        public int JawDropCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.JawDrop > boundJawDrop))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.JawDrop < boundJawDrop)) isOpen = true;

            }

            return count;
        }

        public int LidTightenCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.LidTighten > boundLidTighten))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.LidTighten < boundLidTighten)) isOpen = true;

            }

            return count;
        }

        public int LipCornerDepressorCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.LipCornerDepressor > boundLipCornerDepressor))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.LipCornerDepressor < boundLipCornerDepressor)) isOpen = true;

            }

            return count;
        }

        public int LipPressCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.LipPress > boundLipPress))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.LipPress < boundLipPress)) isOpen = true;

            }

            return count;
        }

        public int LipPuckerCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.LipPucker > boundLipPucker))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.LipPucker < boundLipPucker)) isOpen = true;

            }

            return count;
        }

        public int LipStretchCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.LipStretch > boundLipStretch))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.LipStretch < boundLipStretch)) isOpen = true;

            }

            return count;
        }

        public int LipSuckCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.LipSuck > boundLipSuck))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.LipSuck < boundLipSuck)) isOpen = true;

            }

            return count;
        }

        public int MouthOpenCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.MouthOpen > boundMouthOpen))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.MouthOpen < boundMouthOpen)) isOpen = true;

            }

            return count;
        }

        public int NoseWrinkleCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.NoseWrinkle > boundNoseWrinkle))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.NoseWrinkle < boundNoseWrinkle)) isOpen = true;

            }

            return count;
        }

        public int SmileCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.Smile > boundSmile))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.Smile < boundSmile)) isOpen = true;

            }

            return count;
        }

        public int SmirkCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.Smirk > boundSmirk))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.Smirk < boundSmirk)) isOpen = true;

            }

            return count;
        }

        public int UpperLipRaiseCount(int frameStart, int frameStop)
        {
            int count = 0;

            bool isOpen = true;

            for (int i = frameStart; i <= frameStop; i++)
            {
                Face face = faceStates.GetFace(i);

                if (isOpen && (face.Expressions.UpperLipRaise > boundUpperLipRaise))
                {
                    count++;
                    isOpen = false;
                }
                else if (!isOpen && (face.Expressions.UpperLipRaise < boundUpperLipRaise)) isOpen = true;

            }

            return count;
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

        public string GetCountAll()
        {
            int s = 0;
            int f = faceStates.Count - 1;
            return GetCountAll(s, f);
        }

        public string GetCountAll(int framestart, int framestop)
        {
            StringBuilder output = new StringBuilder();
            output.Append(string.Format("Attentions detected {0}{1}", AttentionDetectCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("BrowFurrow detected {0}{1}", BrowFurrowCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("BrowRaiseCount detected {0}{1}", BrowRaiseCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("CheekRaise detected {0}{1}", CheekRaiseCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("ChinRaise detected {0}{1}", ChinRaiseCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("Dimpler detected {0}{1}", DimplerCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("EyeClosure detected {0}{1}", EyeClosureCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("EyeWiden detected {0}{1}", EyeWidenCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("InnerBrowRaise detected {0}{1}", InnerBrowRaiseCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("JawDrop detected {0}{1}", JawDropCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("LidTighten detected {0}{1}", LidTightenCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("LipCornerDepressor detected {0}{1}", LipCornerDepressorCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("LipPress detected {0}{1}", LipPressCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("LipPucker detected {0}{1}", LipPuckerCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("LipStretch detected {0}{1}", LipStretchCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("LipSuck detected {0}{1}", LipSuckCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("MouthOpen detected {0}{1}", MouthOpenCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("NoseWrinkle detected {0}{1}", NoseWrinkleCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("Smile detected {0}{1}", SmileCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("Smirk detected {0}{1}", SmirkCount(framestart, framestop), Environment.NewLine));
            output.Append(string.Format("UpperLipRaise detected {0}{1}", UpperLipRaiseCount(framestart, framestop), Environment.NewLine));

            return output.ToString();
        }




        
    }
}
