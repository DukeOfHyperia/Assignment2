using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Graph
    {
        public Dictionary<int, Vertex> graph;

        public Graph()
        {
            graph = new Dictionary<int, Vertex>();

            using (StreamReader sr = new StreamReader("graph.txt"))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    int length = Int32.MaxValue;
                    while (line.Length < length)
                    {
                        length = line.Length;
                        line = line.Replace("  ", " ");
                    }

                    String[] data = line.Split(' ');
                    graph.Add(Convert.ToInt32(data[1]), new Vertex(data));
                }
            }
        }

        public int calculateFitness(String input)
        {
            int connections = 0;

            for(int i = 0; i < input.Length; i++)
            {
                Char partition = input[i];

                foreach(int n in graph[i + 1].neighbours)
                    if (input[n - 1] != partition)
                        connections++;
            }

            return connections / 2;
        }
    }
}
