using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Algorithm
    {
        Metaheuristic h;

        public Algorithm(Graph graph)
        {
            h = new Metaheuristic(graph);
        }

        // MLS - ILS - GLS
        public void MLS()
        {
            int iteration = 0;
            List<Individual> localoptima = new List<Individual>();

            List<Individual> initialsolutions = h.generateInitialPopulation(50);

            for (int p = 0; p < 2500 / initialsolutions.Count; p++)
            {
                foreach(Individual i in initialsolutions)
                {
                    Individual solution = i;
                    Individual newsolution = h.VSN(solution);
                    while (newsolution.connections >= solution.connections)
                    {
                        solution = newsolution;
                        newsolution = h.VSN(solution);
                    }
                    localoptima.Add(newsolution);
                    iteration++;
                    Console.WriteLine("[" + iteration + "]" + newsolution.connections);
                }
            }
        }

        public void ILS(int perturbationSize, int timeLimit = 0)
        {
            int iteration = 1;
            int notleft = 0;
            List<Individual> localoptima = new List<Individual>();
            Individual solution;

            Individual initialsolution = h.generateInitialPopulation(1)[0];
            solution = h.VSN(initialsolution);
            localoptima.Add(solution);
            Console.WriteLine("[" + iteration + "]" + solution.connections);

            while(localoptima.Count <= 2500)
            {
                Individual mutatedsolution = h.PerturbateSolution(solution, perturbationSize);
                Individual newsolution = h.VSN(mutatedsolution);

                if (newsolution.connections < localoptima[localoptima.Count - 1].connections) //When the new local optimum is better than the current one, ILS continues its search from this new local optimum
                {
                    solution = newsolution;
                    localoptima.Add(solution);

                    iteration++;
                    Console.WriteLine("[" + iteration + "]" + newsolution.connections);
                }
                else //else it simply returns to the previous local optimum and tries again with another mutation
                {
                    if (newsolution.connections == localoptima[localoptima.Count - 1].connections) //the search has not left the region of attraction of the perturbed local optimum
                    {
                        notleft++;
                    }
                    solution = localoptima[localoptima.Count-1];
                }
            }
        }


        public void GLS(int popSize, int timeLimit = 0)
        {
            List<Individual> initialpopulation = h.generateInitialPopulation(popSize);
            //incremental: only one population, everytime an offspring created, it competes with worst solution in population
            //generational: new population (of offspring) replaces the old population

            //keep local optima found = local search
            List<Individual> newpopulation = new List<Individual>();
            for(int i = 0; i < popSize; i++)
            {
                Individual localoptima = h.VSN(initialpopulation[i]);
                newpopulation.Add(localoptima);
            }

            //incremental
            Random rand = new Random();
            for(int i = 0; i < 2500; i++)
            {
                //parent pair selected at random
                int random1 = rand.Next(0, popSize);
                int random2 = random1;
                while (random2 == random1)
                    random2 = rand.Next(0, popSize);

                //recombinaton
                Individual offspring = h.Recombination(newpopulation[random1], newpopulation[random2]);

                //local search
                Individual temp = h.VSN(offspring);

                //No duplicate solutions allowed in the population??
                bool has = newpopulation.Any(idv => idv.solution == temp.solution);
                while(has)
                    temp = h.VSN(offspring);

                newpopulation.Add(temp);

                //selection (incremental) = competes with worst solution in population
                newpopulation = newpopulation.OrderBy(idv => idv.connections).Take(popSize).ToList();

                //randomize the population
                int n = newpopulation.Count;
                while (n > 1)
                {
                    n--;
                    int k = rand.Next(n + 1);
                    Individual value = newpopulation[k];
                    newpopulation[k] = newpopulation[n];
                    newpopulation[n] = value;
                }
            }

        }
        
    }
}
