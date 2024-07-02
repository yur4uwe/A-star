using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_star
{
    public class Edge
    {
        public int Wieght { get; set; }
        public int start { get; set; }
        public int end { get; set; }

        public Edge (int start, int end, int weight = -1) 
        {
            this.start = start;
            this.end = end; 
            this.Wieght = weight;
        }
    }
}
