using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment2
{
    class Program
    {
        static void Main()
        {
            Graph graph = new Graph();
            
            //for(int i = 5; i < 21; i += 5)
                Execute(graph, 1, true);

            Console.Write("Done!");
            Console.ReadLine();
            /*Solution sol;

            // run 25 times
            for (int c = 0; c < 2; c++)
            {
                for (int ls = 0; ls < 3; ls++)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        Execute(graph, 20, c == 0);
                    }
                }
            }*/
        }

        static Tuple<int, int, double> Execute(Graph graph, int populationSize, Boolean criteria)
        {
            Algorithm alg = new Algorithm(graph);

            /*for (int t = 0; t < 10; t++)
            {
                Thread thread = new Thread(() => alg.GLS(t, populationSize));
                thread.Start();

                Thread.Sleep(100);
            }*/
            //alg.MLS(0);
            alg.GLS(0, 10);

            //Console.ReadLine();
            return new Tuple<int, int, double>(0, 0, 0.0);
        }

        
    }
}
