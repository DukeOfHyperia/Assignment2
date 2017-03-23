using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Solution
    {
        List<int> solutions, lclOptima;
        List<double> time;
        int popSize;
        String lsType; // 0: ILS, 1: MLS, 2: GLS

        public Solution(int Type, int PopSize)
        {
            if (Type == 0)
                lsType = "ILS";
            else if (Type == 1)
                lsType = "MLS";
            else
                lsType = "GLS";
            popSize = PopSize;

            solutions = new List<int>();
            lclOptima = new List<int>();
            time = new List<double>();
        }

        public void addRun(Tuple<int, int, double> data)
        {
            solutions.Add(data.Item1);
            lclOptima.Add(data.Item2);
            time.Add(data.Item3);
        }

        private String calculateStatistics(List<double> data)
        {
            if (data.Count != 0)
            {
                double avg = data.Average();
                double std;
                if (data.Count > 1)
                    std = Math.Sqrt(data.Select(x => (x - avg) * (x - avg)).Sum() / (data.Count - 1));
                else
                    std = 0;
                return (Math.Round(avg, 2) + " (" + Math.Round(std, 2) + ")").Replace(',', '.');
            }
            return "NA (NA)";
        }
        private String calculateStatistics(List<int> data)
        {
            List<double> newData = new List<double>();
            foreach (int i in data)
                newData.Add(Convert.ToDouble(i));
            return calculateStatistics(newData);
        }
    }

    /*public void outputResults()
    {
        Console.WriteLine(crossovertype + ": " + fitnessFunction);
        Console.WriteLine("Population size: " + populationSize);
        Console.WriteLine("Succes   : " + succes + "/25");
        Console.WriteLine("First hit: " + calculateStatistics(firstHit));
        Console.WriteLine("Converge : " + calculateStatistics(converge));
        Console.WriteLine("Fct Evals: " + calculateStatistics(fctEvals));
        Console.WriteLine("CPU time : " + calculateStatistics(cpuTime) + "ms");
        Console.WriteLine("");

        // Output LaTeX-code for tables
        /*Console.WriteLine(" & " + String.Join(" & ", populationSize,
                                                        succes + "/25",
                                                        calculateStatistics(firstHit),
                                                        calculateStatistics(converge),
                                                        calculateStatistics(fctEvals),
                                                        calculateStatistics(cpuTime)) + " \\\\");
        Console.WriteLine("");*/
}