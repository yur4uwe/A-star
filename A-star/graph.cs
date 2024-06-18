using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace A_star
{
    internal class graph
    {
        public HashSet<int> vertices { get; private set; }
        public List<(int, int)> edges { get; private set; }

        public graph() 
        {
            vertices = new HashSet<int>();
            edges = new List<(int, int)>();
        }

        public void AddEdge(int u, int v) { this.edges.Add((u, v)); }

        public void AddNode(int x) { vertices.Add(x); }

        public void RemoveEdge(int u, int v) 
        {
            if (edges.Contains((u, v))) edges.Remove((u, v));
            else if (edges.Contains((v, u))) edges.Remove((v, u));
        }

        public void RemoveNode(int x) 
        {
            List<(int, int)> temp = new List<(int, int)>();
            foreach (var edge in this.edges)
            {
                if (edge.Item1 != x && edge.Item2 != x)
                    temp.Add(edge);
            }
            this.edges = temp;

            vertices.Remove(x);
        }
    }

    class DijkstraGraph
    {
        
    }

    class A_StarGraph
    {
        
    }

    class BFSGraph
    {

    }
}
