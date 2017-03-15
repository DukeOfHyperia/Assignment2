using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Program
    {
        static void Main()
        {
            Graph graph = new Graph();

            // run 25 times
            Execute(graph);
        }

        static void Execute(Graph graph)
        {
            // initial population

            Algorithm alg = new Algorithm(graph);

        }
    }
}
