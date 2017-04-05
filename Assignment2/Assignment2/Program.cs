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
            Algorithm alg = new Algorithm(graph);

            for (int type = 0; type < 3; type++)
            {
                if (type == 0) // MLS
                {
                    Thread tMLS = new Thread(() => MLS(alg));
                    tMLS.Name = "MLS ";
                    tMLS.Start();
                }
                else if (type == 1) // ILS
                {
                    Thread tILS = new Thread(() => ILS(alg));
                    tILS.Name = "ILS";
                    tILS.Start(); 
                }
                else if (type == 2) // GLS
                {
                    Thread tGLS = new Thread(() => GLS(alg));
                    tGLS.Name = "GLS";
                    tGLS.Start(); 
                }

                Thread.Sleep(1000);
            }
            Console.ReadLine();
        }

        static void MLS(Algorithm alg)
        {
            Console.WriteLine("Start: MLS [VSN] : " + DateTime.Now.ToShortTimeString());
            Individual best;
            List<Tuple<int, int, long>> data;
            Solution solMLS = new Solution("MLS", "VSN");
            for (int i = 0; i < 10; i++)
            {
                data = alg.MLS(0, true, out best);
                solMLS.addRun(data);
                Console.WriteLine("MLS [VSN](" + i + "/10) : " + DateTime.Now.ToShortTimeString());
            }

            solMLS.outputResults();

            Console.WriteLine("Start: MLS [FM] : " + DateTime.Now.ToShortTimeString());
            solMLS = new Solution("MLS", "FM");
            for (int i = 0; i < 10; i++)
            {
                data = alg.MLS(0, false, out best);
                solMLS.addRun(data);
                Console.WriteLine("MLS [FM](" + i + "/10) : " + DateTime.Now.ToShortTimeString());
            }

            solMLS.outputResults();
        }
        static void ILS(Algorithm alg)
        {
            Individual best;
            List<Tuple<int, int, long>> data;
            Solution solILS;
            List<int> pertSizes = new List<int>() { 2, 5, 10 };
            foreach (int size in pertSizes)
            {
                Console.WriteLine("Start: ILS<" + size + "> : " + DateTime.Now.ToShortTimeString());
                solILS = new Solution("ILS", "VSN", size);
                for (int i = 0; i < 10; i++)
                {
                    data = alg.ILS(i, size, true, out best);
                    solILS.addRun(data);
                    Console.WriteLine("ILS<" + size + "> (" + i + "/10) : " + DateTime.Now.ToShortTimeString());
                }

                solILS.outputResults();
            }

            Console.WriteLine("Start: ILS<5> : " + DateTime.Now.ToShortTimeString());
            solILS = new Solution("ILS", "FM", 5);
            for (int i = 0; i < 10; i++)
            {
                data = alg.ILS(i, 5, false, out best);
                solILS.addRun(data);
                Console.WriteLine("ILS<5> [FM](" + i + "/10) : " + DateTime.Now.ToShortTimeString());
            }

            solILS.outputResults();
        }
        static void GLS(Algorithm alg)
        {
            Individual best;
            List<Tuple<int, int, long>> data;
            Solution solGLS;
            List<int> popSizes = new List<int>() { 5, 10, 25 };
            foreach (int size in popSizes)
            {
                Console.WriteLine("Start: GLS<" + size + "> : " + DateTime.Now.ToShortTimeString());
                solGLS = new Solution("GLS", "VSN", size);
                for (int i = 0; i < 10; i++)
                {
                    data = alg.GLS(i, size, true, out best);
                    solGLS.addRun(data);
                    Console.WriteLine("GLS<" + size + "> (" + i + "/10) : " + DateTime.Now.ToShortTimeString());
                }

                solGLS.outputResults();
            }

            Console.WriteLine("Start: GLS<25> : " + DateTime.Now.ToShortTimeString());
            solGLS = new Solution("GLS", "FM", 25);
            for (int i = 0; i < 10; i++)
            {
                data = alg.GLS(i, 25, false, out best);
                solGLS.addRun(data);
                Console.WriteLine("GLS<25> [FM](" + i + "/10) : " + DateTime.Now.ToShortTimeString());
            }

            solGLS.outputResults();
        }

    }
}
