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
                    graph.Add(Convert.ToInt32(data[1]) - 1, new Vertex(data));
                }
            }
        }

        public int calculateFitness(String input)
        {
            int connections = 0;

            for(int i = 0; i < input.Length; i++)
            {
                Char partition = input[i];

                foreach(int n in graph[i].neighbours)
                    if (input[n] != partition)
                        connections++;
            }
            return connections / 2;
        }

        public int calculateFitness(String input, int baseConnections, int first, int second)
        {
            List<int> changes = new List<int>() { first, second };

            foreach (int i in changes)
            {
                int connections = 0;
                Char partition = input[i];

                foreach (int n in graph[i].neighbours)
                    if (input[n] != partition)
                        connections++;

                baseConnections +=  - (graph[i].neighbours.Count - connections) + connections;
            }
            if (graph[first].neighbours.Contains(second))
                baseConnections--;

            return baseConnections;
        }
    }
}
