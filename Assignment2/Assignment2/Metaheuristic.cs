using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Metaheuristic
    {
        Graph graph;

        public Metaheuristic(Graph g)
        {
            graph = g;
        }

        public List<Individual> generateInitialPopulation(int n)
        {
            List<Individual> population = new List<Individual>();

            int l = graph.graph.Count;
            Char[] baseIndividual = new Char[l];
            for (int i = 0; i < l / 2; i++)
            {
                baseIndividual[i] = '1';
                baseIndividual[i + l / 2] = '0';
            }

            for (int i = 0; i < n; i++)
            {
                String individual = scrambleToRandomIndividual(baseIndividual);
                population.Add(new Individual(individual, graph.calculateFitness(individual)));
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

        
        public Individual VSN(Individual input)
        { 
            Random rand = new Random();
            int random1 = rand.Next(0, input.solution.Length-1);
            int random2 = random1;
            while (random2 == random1 || input.solution[random1] == input.solution[random2])
                random2 = rand.Next(0, input.solution.Length-1);

            char t1 = input.solution[random1];
            char t2 = input.solution[random2];
            char[] binary = input.solution.ToCharArray();
            char temp = binary[random1];
            binary[random1] = binary[random2];
            binary[random2] = temp;

            String newsolution = new String(binary);

            return new Individual(newsolution, graph.calculateFitness(newsolution));
        }

        public Individual PerturbateSolution(Individual input, int size)
        {
            Random rand = new Random();
            int random1 = 0;
            int random2 = 0;
            char[] binary = input.solution.ToCharArray();

            for (int i = 0; i < size; i++)
            {
                random1 = rand.Next(0, input.solution.Length - 1);
                random2 = random1;
                while (random2 == random1 || input.solution[random1] == input.solution[random2])
                    random2 = rand.Next(0, input.solution.Length - 1);

                char t1 = input.solution[random1];
                char t2 = input.solution[random2];

                char temp = binary[random1];
                binary[random1] = binary[random2];
                binary[random2] = temp;
            }

            String mutatedsolution = new String(binary);
            return new Individual(mutatedsolution, graph.calculateFitness(mutatedsolution));
        }

        public Individual Recombination(Individual parent1, Individual parent2)
        {
            //To be added

            return parent1;
        
        }


    }
}
