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

        public Algorithm(Graph graph, List<Individual> population = null)
        {
            h = new Metaheuristic(graph);
        }

        // MLS - ILS - GLS
        public void MLS(int id)
        {
            Queue<Individual> localOptima = h.generateInitialPopulation(1);
            int best = int.MaxValue;

            for (int lclO = 0; lclO < 2000; lclO++)
            {
                Individual solution = localOptima.Dequeue();
                Individual newSolution = solution;

                while (newSolution != null && newSolution.connections >= solution.connections)
                    newSolution = h.VSN_search(solution);

                if (newSolution == null)
                {
                    newSolution = h.generateInitialPopulation(1).Dequeue();
                    Console.WriteLine("Old best: " + best);
                    Console.WriteLine("New startpoint");
                }
                
                localOptima.Enqueue(newSolution);

                if (newSolution.connections < best)
                    best = newSolution.connections;

                if(lclO % 10 == 0)
                    Console.WriteLine("<" + id + "> [" + lclO + "]" + newSolution.connections);
            }

            Console.WriteLine("<" + id + ">              Final:" + best);
        }
        
        public void ILS(int id, int perturbationSize, int timeLimit = 0)
        {
            Individual startSolution = h.generateInitialPopulation(1).Dequeue();
            Individual newSolution = startSolution;
            Individual best = new Individual("", int.MaxValue);
            int nrOfReturns = 0;

            for(int lclO = 0; lclO < 2000; lclO++)
            {
                while (newSolution.connections >= startSolution.connections)
                {
                    newSolution = h.VSN_search(startSolution);

                    if (newSolution == null)
                    {
                        newSolution = h.pertubation(startSolution, perturbationSize);
                        Console.WriteLine("Jumped to " + newSolution.connections);
                        break;
                    }
                }

                if (newSolution.connections < best.connections)
                    best = newSolution;
                else if (newSolution.connections == best.connections && newSolution.solution == best.solution)
                    nrOfReturns++;

                Console.WriteLine("<" + id + " : " + perturbationSize + "> [" + lclO + "] " + best.connections + " !" + nrOfReturns + "! " + newSolution.connections);

                startSolution = newSolution;
            }

            Console.WriteLine("<" + id + ">              Final:" + best.connections);
        }
        
        
        public void GLS(int id, int popSize, int timeLimit = 0)
        {
            List<Individual> initialpopulation = h.generateInitialPopulation(popSize).ToList();
            //incremental: only one population, everytime an offspring created, it competes with worst solution in population
            //generational: new population (of offspring) replaces the old population

            //keep local optima found = local search
            List<Individual> newPopulation = new List<Individual>();
            for(int i = 0; i < popSize; i++)
            {
                Console.WriteLine("LS on:       " + initialpopulation[i].connections);
                Individual localoptima = h.VSN(initialpopulation[i], 1500);
                Console.WriteLine("Results in:  " + localoptima.connections);
                newPopulation.Add(localoptima);
            }

            //incremental
            Random rand = new Random();
            for(int i = 0; i < 2000; i++)
            {
                //parent pair selected at random
                int random1 = rand.Next(0, popSize);
                int random2 = random1;
                while (random2 == random1)
                    random2 = rand.Next(0, popSize);

                //recombinaton
                Individual offspring = h.VSN(h.recombination(newPopulation[random1].solution, newPopulation[random2].solution), 2500);

                if (newPopulation.Where(x => x.solution == offspring.solution).FirstOrDefault() == default(Individual))
                    newPopulation.Add(offspring);

                //selection (incremental) = competes with worst solution in population
                newPopulation = newPopulation.OrderBy(idv => idv.connections).Take(popSize).ToList();

                Console.WriteLine("<" + id + "> [" + i + "] " + newPopulation[0].connections + " ! " + offspring.connections);
            }

            Console.WriteLine("<" + id + " : " + popSize + "> " + newPopulation[0].connections);
            Console.ReadLine();

        }
    }
}
