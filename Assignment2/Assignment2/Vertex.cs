using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Vertex
    {
        public int id;
        public List<int> neighbours;

        public Vertex(String[] data)
        {
            id = Convert.ToInt32(data[1]) - 1;

            neighbours = new List<int>();
            for (int i = 4; data[3] != "0" && i < data.Length; i++)
                neighbours.Add(Convert.ToInt32(data[i]) - 1);
        }
    }
}
