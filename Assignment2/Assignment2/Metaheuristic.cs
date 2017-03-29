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
        List<Tuple<int, int>> swaps;
        public Metaheuristic(Graph g)
        {
            graph = g;
            swaps = createSwaps(g.graph.Count);
        }

        public Individual pertubation(Individual input, int size = 1)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            int first, second;
            for (int i = 0; i < size; i++)
            {
                first = rand.Next(0, input.solution.Length);
                second = first;
                while (first == second || input.solution[first] == input.solution[second])
                    second = rand.Next(0, input.solution.Length);

                input = VSN_swap(input, first, second);
            }
            return input;
        }

        public Individual VSN(Individual input, int bound = 7500)
        {
            Individual current = input;
            while (true)
            {
                input = VSN_search(input, bound);
                if (input == null)
                    return current;
                current = input;
            }
        }

        public Individual VSN_search(Individual input, int bound = 7500)
        {
            List<Tuple<int, int>> lclSwaps = new List<Tuple<int, int>>(swaps);

            Random rand = new Random(DateTime.Now.Millisecond);
            Tuple<int, int> swap;
            Individual newSolution;

            while (swaps.Count - lclSwaps.Count < bound)
            {
                swap = lclSwaps[rand.Next(0, lclSwaps.Count)];
                newSolution = VSN_swap(input, swap.Item1, swap.Item2);
                if (newSolution.connections < input.connections)
                    return newSolution;

                lclSwaps.Remove(swap);
            }
            return null;
        }

        public Individual VSN_swap(Individual input, int first, int second)
        {
            if (input.solution[first] == input.solution[second])
                return new Individual("", int.MaxValue);

            String s1 = input.solution[first].ToString();
            String s2 = input.solution[second].ToString();

            String solution = input.solution.Remove(first, 1).Insert(first, s2).Remove(second, 1).Insert(second, s1);

            return new Individual(solution, graph.calculateFitness(solution, input.connections, first, second));
        }
        

        public Individual recombination(String parent1, String parent2)
        {
            int hmDistance = this.calculateHamming(parent1, parent2);

            if (hmDistance > parent1.Length / 2)
                parent1 = this.invert(parent1);

            String child = "";
            List<int> cr = new List<int>(); 
            for (int i = 0; i < parent1.Length; i++)
            {
                if (parent1[i] == parent2[i])
                    child += parent1[i].ToString();
                else
                    cr.Add(i);
            }

            int c1 = (parent1.Length - cr.Count) / 2, c0 = c1;
            Random rand = new Random();
            foreach(int i in cr)
            {
                if (c1 == parent1.Length / 2)
                    child = child.Insert(i, "0");
                else if (c0 == parent1.Length / 2)
                    child = child.Insert(i, "1");
                else
                {
                    int value = rand.Next(0, 200) % 2;
                    child = child.Insert(i, value.ToString());
                    if (value == 1)
                        c1++;
                    else
                        c0++;
                }
            }
            return new Individual(child, graph.calculateFitness(child));
        }

        public Queue<Individual> generateInitialPopulation(int n)
        {
            Queue<Individual> population = new Queue<Individual>();

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
                population.Enqueue(new Individual(individual, graph.calculateFitness(individual)));
            }

            return population;
        }

        private int calculateHamming(String input1, String input2)
        {
            int distance = 0;

            for (int i = 0; i < input1.Length; i++)
                if (input1[i] != input2[i])
                    distance++;

            return distance;
        }
        private String invert(String input)
        {
            String inverted = "";
            for (int i = 0; i < input.Length; i++)
                if (input[i] == '1')
                    inverted += "0";
                else // if (input[i] == '0')
                    inverted += "1";
            return inverted;
        }

        private String scrambleToRandomIndividual(Char[] individual)
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

        private List<Tuple<int, int>> createSwaps(int size)
        {
            List<Tuple<int, int>> swaps = new List<Tuple<int, int>>();

            for (int i = 0; i < size; i++)
                for (int j = i + 1; j < size; j++)
                    swaps.Add(new Tuple<int, int>(i, j));

            return swaps;
        }

        public Individual FM(Individual solution)
        {
            Individual newsolution = new Individual("", 0);
            while (newsolution.connections < solution.connections) //repeat until no improvement
            {
                solution = newsolution;
                List<Vertex> A = new List<Vertex>();
                List<Vertex> B = new List<Vertex>();
                for (int i = 0; i < 500; i++)
                {
                    if (solution.solution[i] == '0')
                        A.Add(new Vertex(i, VertexGain(solution.solution, i)));
                    else
                        B.Add(new Vertex(i, VertexGain(solution.solution, i)));
                }

                List<Individual> solutions = new List<Individual>();
                String binary;
                while (A.Count > 0)
                {
                    //get max gain from A
                    Vertex maxA = A.OrderByDescending(x => x.gain).First();

                    //move maxA to B
                    A.Remove(maxA);
                    char[] a = solution.solution.ToCharArray();
                    a[maxA.id] = '1';
                    binary = new String(a);

                    //recompute gains B
                    B = RecomputeGains(B, binary);

                    //get max gain from B
                    Vertex maxB = B.OrderByDescending(x => x.gain).First();

                    //move maxB to A
                    B.Remove(maxB);
                    char[] b = binary.ToCharArray();
                    b[maxB.id] = '0';
                    binary = new String(b);

                    //recompute gains A for next iteration
                    if (A.Count > 0)
                        A = RecomputeGains(A, binary);

                    solution = new Individual(binary, graph.calculateFitness(binary));
                    solutions.Add(solution);
                }
                newsolution = solutions.OrderBy(x => x.connections).First();
            }
            return newsolution;
        }

        public int VertexGain(String input, int vertexid)
        {
            int E = 0; //external cost
            int I = 0; //internal cost
            Char subset = input[vertexid];
            foreach (int n in graph.graph[vertexid].neighbours)
            {
                if (input[n] == subset)
                    I++;
                else
                    E++;
            }
            return E - I;
        }

        private List<Vertex> RecomputeGains(List<Vertex> arrayGains, String input)
        {
            List<Vertex> newGains = new List<Vertex>();
            foreach (Vertex v in arrayGains)
            {
                newGains.Add(new Vertex(v.id, VertexGain(input, v.id)));
            }
            return newGains;
        }

    }
}
