using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Algorithm
    {
        Metaheuristic h;
        Graph g;

        public Algorithm(Graph graph, List<Individual> population = null)
        {
            h = new Metaheuristic(graph);
            g = graph;
        }

        // MLS - ILS - GLS
        public List<Tuple<int, int, long>> MLS(int id, Boolean searchType, out Individual best)
        {
            List<Tuple<int, int, long>> sol = new List<Tuple<int, int, long>>();
            int checks = 0;
            Stopwatch clock = new Stopwatch();
            clock.Start();

            Individual startSolution = h.generateInitialPopulation(1).Dequeue();
            best = new Individual("", int.MaxValue);

            int lclO = 0;
            while (lclO <= 2000 || checks <= 10) // 1200 seconds
            {
                Individual newSolution;
                if (searchType)
                    newSolution = h.VSN(startSolution);
                else
                    newSolution = h.FM(startSolution);

                if (newSolution.connections < best.connections)
                    best = newSolution;

                startSolution = h.generateInitialPopulation(1).Dequeue();

                // OUTPUT
                if (lclO % 500 == 0 && lclO <= 2000)
                    sol.Add(new Tuple<int, int, long>(lclO, best.connections, clock.ElapsedMilliseconds / 1000));

                if (clock.ElapsedMilliseconds > 120000 * checks && checks <= 10)
                {
                    sol.Add(new Tuple<int, int, long>(lclO, best.connections, clock.ElapsedMilliseconds / 1000));
                    checks++;
                }
                lclO++;
            }
            clock.Stop();

            return sol;
        }
        
        public List<Tuple<int, int, long>> ILS(int id, int perturbationSize, Boolean searchType, out Individual best)
        {
            List<Tuple<int, int, long>> sol = new List<Tuple<int, int, long>>();
            int checks = 1;
            Stopwatch clock = new Stopwatch();
            clock.Start();

            Individual startSolution = h.generateInitialPopulation(1).Dequeue();
            best = new Individual("", int.MaxValue);

            Individual newSolution;

            int lclO = 0;
            while (lclO <= 2000 || checks <= 10) // 1200 seconds
            {
                
                if (searchType)
                    newSolution = h.VSN(startSolution);
                else
                    newSolution = h.FM(startSolution);

                if (newSolution.connections < best.connections)
                    best = newSolution;

                startSolution = h.pertubation(newSolution, perturbationSize);

                // OUTPUT
                if (lclO % 500 == 0 && lclO <= 2000)
                    sol.Add(new Tuple<int, int, long>(lclO, best.connections, clock.ElapsedMilliseconds / 1000));

                if (clock.ElapsedMilliseconds > 120000 * checks && checks <= 10)
                {
                    sol.Add(new Tuple<int, int, long>(lclO, best.connections, clock.ElapsedMilliseconds / 1000));
                    checks++;
                }
                lclO++;
            }
            clock.Stop();

            return sol;
        }
        
        public List<Tuple<int, int, long>> GLS(int id, int popSize, Boolean searchType, out Individual best)
        {
            List<Tuple<int, int, long>> sol = new List<Tuple<int, int, long>>();
            int checks = 1;
            Stopwatch clock = new Stopwatch();
            clock.Start();

            int max = Int32.MaxValue;
            best = new Individual("", max);
            

            List<Individual> initialpopulation = h.generateInitialPopulation(popSize).ToList();
            //incremental: only one population, everytime an offspring created, it competes with worst solution in population
            //generational: new population (of offspring) replaces the old population

            //keep local optima found = local search
            List<Individual> newPopulation = new List<Individual>();
            Individual localOptima;
            for(int i = 0; i < popSize; i++)
            {
                if (searchType)
                    localOptima = h.VSN(initialpopulation[i]);
                else
                    localOptima = h.FM(initialpopulation[i]);

                //Console.WriteLine("Start with:  " + localOptima.connections);
                newPopulation.Add(localOptima);
            }

            //incremental
            Random rand = new Random();
            int lclO = popSize;
            while (lclO <= 2000 || checks <= 10) // 1200 seconds
            {
                //parent pair selected at random
                int first = rand.Next(0, popSize);
                int second = first;
                while (second == first)
                    second = rand.Next(0, popSize);

                //recombinaton
                Individual offspring = h.recombination(newPopulation[first].solution, newPopulation[second].solution);
                if (searchType)
                    offspring = h.VSN(offspring);
                else
                    offspring = h.FM(offspring);

                //selection (incremental) = competes with worst solution in population
                if (offspring.connections < max && !newPopulation.Any(x => x.solution == offspring.solution))
                {
                    newPopulation.Add(offspring);
                    newPopulation = newPopulation.OrderBy(idv => idv.connections).Take(popSize).ToList();
                    max = newPopulation[popSize - 1].connections;
                }

                if (newPopulation[0].connections < best.connections)
                    best = newPopulation[0];

                // OUTPUT
                if (lclO % 500 == 0 && lclO <= 2000)
                    sol.Add(new Tuple<int, int, long>(lclO, best.connections, clock.ElapsedMilliseconds / 1000));

                if (clock.ElapsedMilliseconds > 120000 * checks && checks <= 10)
                {
                    sol.Add(new Tuple<int, int, long>(lclO, best.connections, clock.ElapsedMilliseconds / 1000));
                    checks++;
                }
                lclO++;

                //Console.WriteLine(lclO + " <" + Math.Round((double)clock.ElapsedMilliseconds / 1000, 2) + "> " + best.connections);
            }
            clock.Stop();

            return sol;
        }
    }
}
