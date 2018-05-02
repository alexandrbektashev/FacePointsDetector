using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessVideoFile
{
    class NaiveBayesClassifier
    {
        // TO DO: make it less crappy
        private Dictionary<string, int> freqTruthWic;
        private Dictionary<string, int> freqLieWic;
        private List<string> actions;
        private double DcTruth;
        private double DcLie;
        private double LcTruth;
        private double LcLie;
        private double V;
        private double D;

        public NaiveBayesClassifier(Dictionary<string[], bool> trainSamples)
        {

            Train(trainSamples);

        }

        private void Train(Dictionary<string[], bool> trainSamples)
        {
            //espessially this method

            freqLieWic = new Dictionary<string, int>();
            freqTruthWic = new Dictionary<string, int>();
            actions = new List<string>();
            DcLie = DcTruth = V = LcLie = LcTruth = D = 0; //за это меня следует убить
            D = trainSamples.Count;

            foreach (string[] attributes in trainSamples.Keys)
                if (trainSamples[attributes])
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
            V = actions.Count;
            LcLie = freqLieWic.Sum(x => x.Value);
            LcTruth = freqTruthWic.Sum(x => x.Value);

            //Console.WriteLine("Таблица действий");
            //foreach (string atr in actions)
            //    Console.WriteLine("{0,10}\t{1,2}\t{2,2}", atr,
            //        freqLieWic.Keys.Contains(atr) ? freqLieWic[atr] : 0,
            //        freqTruthWic.Keys.Contains(atr) ? freqTruthWic[atr] : 0);
            //Console.WriteLine("Статистика");
            //Console.WriteLine("D = {0}, DcTruth = {1}, DcLie = {2},{3}" +
            //    "V = {4}, LcTruth = {5}, LcLie = {6}", D, DcTruth, DcLie, Environment.NewLine,
            //    V, LcTruth, LcLie);

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

    }

}
