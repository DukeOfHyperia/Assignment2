using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Metaheuristic
    {
        public Metaheuristic() { }

        public String VSN(String input)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            int first = rand.Next(0, input.Length);
            int second = first;
            while (second == first || input[first] == input[second])
                second = rand.Next(0, input.Length);

            String s1 = input[first].ToString();
            String s2 = input[second].ToString();

            return input.Remove(first,1).Insert(first, s2).Remove(second, 1).Insert(second, s2);
        }

        public String Recombination(String parent1, String parent2)
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
            Random rand = new Random(DateTime.Now.Millisecond);
            foreach(int i in cr)
            {
                if (c1 == parent1.Length / 2)
                    child = child.Insert(i, "0");
                else if (c0 == parent1.Length / 2)
                    child = child.Insert(i, "1");
                else
                {
                    int value = rand.Next(0, 2);
                    child = child.Insert(i, value.ToString());
                    if (value == 1)
                        c1++;
                    else
                        c0++;
                }
            }
            return child;
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
    }
}
