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
        public int vertices { get; private set; }
        public List<(int, int, int)> edges { get; private set; }

        public graph() 
        {
            vertices = 0;
            edges = new List<(int, int, int)>();
        }

        public bool AddEdge(int u, int v, int w = -1) { 
            if(this.edges.Contains((u, v, w)) || this.edges.Contains((v, u, w))) return false;
            else
            {
                this.edges.Add((u, v, w));
                return true;
            }
            
        }

        public void AddNode() { vertices++; }

        public void RemoveEdge(int u, int v, int w = -1) 
        {
            if (edges.Contains((u, v, w))) edges.Remove((u, v, w));
            else if (edges.Contains((v, u, w))) edges.Remove((v, u, w));
        }

        public void RemoveNode() 
        {
            vertices--;
        }

        public int GetWeight(int u, int v)
        {
            foreach (var edge in edges)
            {
                if((u == edge.Item1 || u == edge.Item2) && (v == edge.Item1 || v == edge.Item2))
                {
                    return edge.Item3;
                }
            }
            return -1;
        }
    }

    class DijkstraGraph
    {
        private int[,] adj;
        private HashSet<int> sptSet;
        private int[] dist;
        private int vertices;

        public DijkstraGraph(int vertices, List<(int, int, int)> edges)
        {
            adj = new int[vertices, vertices];

            for(int i = 0; i < vertices; i++)
            {
                for(int j = 0; j < vertices; j++)
                {
                    adj[i, j] = 0;
                }
            }
            
            foreach (var edge in edges)
            {
                int u = edge.Item1;
                int v = edge.Item2;
                int w = edge.Item3;
                adj[u, v] = w;
                adj[v, u] = w;
            }

            sptSet = new HashSet<int>();
            dist = new int[vertices];

            for (int i = 0; i < vertices; i++) dist[i] = int.MaxValue; 

            this.vertices = vertices;
        }

        public void Execute(int start)
        {
            dist[start] = 0;

            while(sptSet.Count != vertices) 
            {
                int u = minDist();

                sptSet.Add(u);

                for(int v = 0; v < vertices; v++)
                {
                    if (!sptSet.Contains(v) && adj[u, v] != 0 &&
                        dist[u] != int.MaxValue && dist[u] + adj[u, v] < dist[v])
                    {
                        dist[v] = dist[u] + adj[u, v];
                    }
                }
            }
        }

        private int minDist()
        {
            // Initialize min value
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < vertices; v++)
                if (!sptSet.Contains(v) && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }
    }

    class A_StarGraph
    {
        
    }
}
