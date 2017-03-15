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
            List<Individual> population = generateInitialPopulation(graph, 10);
            // initial population

            Algorithm alg = new Algorithm(graph);

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
