using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Solution
    {
        int popSize;
        String lsType; // 0: ILS, 1: MLS, 2: GLS
        String neighbourSearch; // true: VSN, false: FM
        List<Tuple<int, int, long>> timeOptima;

        public Solution(String Type, String Search, int PopSize = 0)
        {
            lsType = Type;
            popSize = PopSize;

            neighbourSearch = Search;

            timeOptima = new List<Tuple<int, int, long>>();
        }

        public void addRun(List<Tuple<int, int, long>> sol)
        {
            timeOptima.AddRange(sol);
        }

        private String calculateStatistics(List<int> data)
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

        public void outputResults()
        {
            StreamWriter sw = new StreamWriter(lsType + "[" + neighbourSearch + "](" + popSize + ").txt");

            List<int> data = new List<int>() { 1500, 1200 };
            for (int i = 0; i < data.Count; i++)
            {
                List<int> results = new List<int>();
                List<int> times = selectResultsOnOptima(data[i], i == 0, out results);

                int min = results.Min();
                int max = results.Max();
                double avg = results.Average();

                Console.WriteLine(lsType + " (" + neighbourSearch + ") : " + popSize + "               (" + (i+1) + "/2)");
                Console.WriteLine("*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*");
                String line = "& " + String.Join(" & ", min, calculateStatistics(results), max, calculateStatistics(times)) + " \\\\";
                Console.WriteLine(line);
                sw.WriteLine(line);
                Console.WriteLine("");
                sw.WriteLine("");
            }
            Console.WriteLine("");
            sw.Close();

        }

        private List<int> selectResultsOnOptima(int bound, Boolean equal, out List<int> best)
        {
            List<int> times = new List<int>();
            best = new List<int>();

            if (equal)
            {
                foreach (Tuple<int, int, long> tup in timeOptima)
                {
                    if (tup.Item1 == bound)
                    {
                        times.Add(Convert.ToInt32(tup.Item3));
                        best.Add(tup.Item2);
                    }
                }
            }
            else
            {
                foreach (Tuple<int, int, long> tup in timeOptima)
                {
                    if (tup.Item3 >= bound)
                    {
                        times.Add(tup.Item1);
                        best.Add(tup.Item2);
                    }
                }
            }
            return times;
        }
    }
}