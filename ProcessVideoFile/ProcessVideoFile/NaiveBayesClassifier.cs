using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessVideoFile
{
    class NaiveBayesClassifier
    {
        private Dictionary<string, int> freqTruthWic;
        private Dictionary<string, int> freqLieWic;
        private List<string> actions;
        private double DcTruth;
        private double DcLie;
        private double LcTruth;
        private double LcLie;
        private double V;
        private double D;


        public NaiveBayesClassifier(List<string[]> trainSamples, List<bool> trainSamplesBool)
        {

            Train(trainSamples, trainSamplesBool);

        }

        private void Train(List<string[]> trainSamples, List<bool> trainSamplesBool)
        {
            //espessially this method

            freqLieWic = new Dictionary<string, int>();
            freqTruthWic = new Dictionary<string, int>();

            actions = new List<string>();
            DcLie = DcTruth = V = LcLie = LcTruth = D = 0; //за это меня следует убить
            D = trainSamples.Count;

            for (int i = 0; i < trainSamples.Count; i++)
            {
                string[] attributes = trainSamples[i];
                if (trainSamplesBool[i])
                {
                    DcTruth++;
                    foreach (string atr in attributes)
                    {
                        if (freqTruthWic.Keys.Contains(atr))
                            freqTruthWic[atr]++;
                        else freqTruthWic.Add(atr, 1);
                        if (!actions.Contains(atr)) actions.Add(atr);
                    }
                }
                else
                {
                    DcLie++;
                    foreach (string atr in attributes)
                    {
                        if (freqLieWic.Keys.Contains(atr))
                            freqLieWic[atr]++;
                        else freqLieWic.Add(atr, 1);
                        if (!actions.Contains(atr)) actions.Add(atr);
                    }
                }
            }
            V = actions.Count;
            LcLie = freqLieWic.Sum(x => x.Value);
            LcTruth = freqTruthWic.Sum(x => x.Value);



        }

        public double Predict(string[] model, out bool istrue)
        {
            double truth = Math.Log(DcTruth / D);
            double lie = Math.Log(DcLie / D);
            foreach (string atr in model)
            {
                if (freqTruthWic.Keys.Contains(atr))
                {
                    double wic = freqTruthWic[atr];
                    double value = Math.Log((wic + 1) / (V + LcTruth));
                    truth += value;
                }
                if (freqLieWic.Keys.Contains(atr))
                {
                    double wic = freqLieWic[atr];
                    double value = Math.Log((wic + 1) / (V + LcLie));
                    lie += value;
                }

            }
            if (truth > lie) istrue = true;
            else istrue = false;

            double probability = Math.Exp(lie) / (Math.Exp(lie) + Math.Exp(truth));

            return probability;

        }

        public string Predict(string[] model, out bool istrue, out double probability)
        {
            StringBuilder data = new StringBuilder();
            data.Append("Calculations:" + Environment.NewLine);
            
            StringBuilder truthProb = new StringBuilder();
            StringBuilder lieProb = new StringBuilder();
            
            double truth = Math.Log(DcTruth / D);
            truthProb.Append("For truth:" + Environment.NewLine);
            truthProb.Append("Dt = log(DcTruth/D) + Sum(wic + 1)/(V + LcTruth))" + Environment.NewLine);
            truthProb.Append(string.Format("Dt = {0:0.000} ", truth));

            double lie = Math.Log(DcLie / D);
            lieProb.Append("For lie:" + Environment.NewLine);
            lieProb.Append("Dl = log(DcLie/D) + Sum(wic + 1)/(V + LcLie))" + Environment.NewLine);
            lieProb.Append(string.Format("Dl = {0:0.000} ", lie));

            foreach (string atr in model)
            {
                if (freqTruthWic.Keys.Contains(atr))
                {
                    double wic = freqTruthWic[atr];
                    double value = Math.Log((wic + 1) / (V + LcTruth));
                    truthProb.Append(string.Format(" {0:0.000}", value));
                    truth += value;
                }
                if (freqLieWic.Keys.Contains(atr))
                {
                    double wic = freqLieWic[atr];
                    double value = Math.Log((wic + 1) / (V + LcLie));
                    lieProb.Append(string.Format(" {0:0.000}", value));
                    lie += value;
                }

            }
            truthProb.Append(string.Format("={0:0.00000}{1}", truth, Environment.NewLine));
            lieProb.Append(string.Format("={0:0.00000}{1}", lie, Environment.NewLine));
            
            data.Append(truthProb + Environment.NewLine);
            data.Append(lieProb + Environment.NewLine);

            istrue = false;
            if (truth > lie)
            {
                istrue = true;
                data.Append(string.Format("{0:0.00000} > {1:0.00000}{2}Truth is more possible{3}", truth, lie, Environment.NewLine, Environment.NewLine));

            }
            else
                data.Append(string.Format("{0:0.00000} < {1:0.00000}{2}Lie is more possible{3}", truth, lie, Environment.NewLine, Environment.NewLine));

            double probabilityLie = Math.Exp(lie) / (Math.Exp(lie) + Math.Exp(truth));
            double probabilityTruth = Math.Exp(truth) / (Math.Exp(lie) + Math.Exp(truth));

            data.Append(string.Format("Probability for truth - {0:0.00}{1}", probabilityTruth, Environment.NewLine));
            data.Append(string.Format("Probability for lie - {0:0.00}{1}", probabilityLie, Environment.NewLine));

            probability = probabilityLie;
            
            return data.ToString();
        }

        public string GetInfo()
        {
            StringBuilder table = new StringBuilder();

            table.Append("Classifier settings:" + Environment.NewLine);
            table.Append(string.Format("Dc lie = {0,5}; Dc truth = {1, 5}{2}", DcLie, DcTruth, Environment.NewLine));
            table.Append(string.Format("Lc lie = {0,5}; Lc truth = {1, 5}{2}", LcLie, LcTruth, Environment.NewLine));
            table.Append(string.Format("V      = {0,5}; D        = {1, 5}{2}", V, D, Environment.NewLine));
            table.Append(string.Format("{0,19}\t{1,5}\t{2,5}{3}", "Type", "Lie", "Truth", Environment.NewLine));
            foreach (string atr in actions)
                table.Append(string.Format("{0,19}\t{1,5}\t{2,5}{3}", atr,
                    freqLieWic.Keys.Contains(atr) ? freqLieWic[atr] : 0,
                    freqTruthWic.Keys.Contains(atr) ? freqTruthWic[atr] : 0, Environment.NewLine));

            return table.ToString();
        }
    }

}
