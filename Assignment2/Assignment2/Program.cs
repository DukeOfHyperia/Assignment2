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
            Solution sol;

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
            }
        }

        static Tuple<int, int, double> Execute(Graph graph, int populationSize, Boolean criteria)
        {
                List<Individual> population = generateInitialPopulation(graph, populationSize);
                // initial population

                Algorithm alg = new Algorithm(graph, population);

            return new Tuple<int, int, double>(0, 0, 0.0);
        }

        static List<Individual> generateInitialPopulation(Graph g, int n)
        {
            List<Individual> population = new List<Individual>();

            int l = g.graph.Count;
            Char[] baseIndividual = new Char[l];
            for (int i = 0; i < l / 2; i++)
            {
                baseIndividual[i] = '1';
                baseIndividual[i + l / 2] = '0';
            }

            for (int i = 0; i < n; i++)
            {
                String individual = scrambleToRandomIndividual(baseIndividual);
                population.Add(new Individual(individual, g.calculateFitness(individual)));
            }

            return population;
        }

        static String scrambleToRandomIndividual(Char[] individual)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            int n = individual.Length;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Char value = individual[k];
                individual[k] = individual[n];
                individual[n] = value;
            }

            return new String(individual);
        }
    }
}
