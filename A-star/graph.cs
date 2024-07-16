using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace A_star
{
    internal class graph
    {
        public int vertices { get; private set; }
        /// <summary>
        /// Contains 3 params: u(node 1), v(node 2), w(weight)
        /// </summary>
        public List<Edge> edges { get; private set; }
        public bool isWeighted;
        public bool isoriented;

        public graph() 
        {
            vertices = 0;
            edges = new List<Edge>();
        }

        public bool AddEdge(int u, int v, int w = -1) { 
            if(this.edges.Contains(new Edge(u, v, w)) || this.edges.Contains(new Edge(v, u, w))) return false;
            else
            {
                this.edges.Add(new Edge(u, v, w));
                return true;
            }
            
        }

        public void AddNode() { vertices++; }

        public void RemoveEdge(int u, int v, int w = -1) 
        {
            if (edges.Contains(new Edge(u, v, w))) edges.Remove(new Edge(u, v, w));
            else if (edges.Contains(new Edge(v, u, w))) edges.Remove(new Edge(v, u, w));
        }

        public void RemoveNode() 
        {
            vertices--;
        }

        public int GetWeight(int u, int v)
        {
            foreach (var edge in edges)
            {
                if((u == edge.start || u == edge.end) && (v == edge.start || v == edge.end))
                {
                    return edge.Wieght;
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

        public DijkstraGraph(int vertices, List<Edge> edges)
        {
            if (vertices <= 0) throw new ArgumentException("Cannot starn an algoritm without nodes");

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
                int u = edge.start;
                int v = edge.end;
                int w = edge.Wieght;
                adj[u, v] = w;
                adj[v, u] = w;
            }

            sptSet = new HashSet<int>();
            dist = new int[vertices];

            for (int i = 0; i < vertices; i++) dist[i] = int.MaxValue; 

            this.vertices = vertices;
        }

        public async Task ExecuteAsync(int[] startEnd, GraphLayout layout)
        {
            layout.DeselectAll();

            if (startEnd[0] < 0) { return; }

            dist[startEnd[0]] = 0;

            layout.ShowResult(dist);

            while (sptSet.Count != vertices)
            {
                int u = minDist();

                sptSet.Add(u);
                layout.DrawNode(u);

                await Task.Delay(500);

                for (int v = 0; v < vertices; v++)
                {
                    if (!sptSet.Contains(v) && adj[u, v] != 0 &&
                        dist[u] != int.MaxValue && dist[u] + adj[u, v] < dist[v])
                    {
                        dist[v] = dist[u] + adj[u, v];
                        layout.RemoveEdges(edge => edge.start.NodeID == v || edge.end.NodeID == v);
                        layout.DrawEdge(u, v);
                    }
                }
                layout.AddResult(dist);
            }            
            //layout.DeselectAll();
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
        SortedSet<(double f, int vertex)> OpenList = new SortedSet<(double f, int vertex)>
            (
                Comparer<(double f, int vertex)>.Create((a, b) => a.Item1.CompareTo(b.Item1))
            );
        (double f, double g, double h, bool visited)[] ClosedList;
        int[,] adj;

        public A_StarGraph(int vertices, List<Edge> edges)
        {
            ClosedList = new (double f, double g, double h, bool visited)[vertices];
            adj = new int[vertices, vertices];

            for(int i = 0; i < ClosedList.Length; i++)
            {
                ClosedList[i].f = Double.PositiveInfinity;
                ClosedList[i].g = Double.PositiveInfinity;
                ClosedList[i].h = Double.PositiveInfinity;
                ClosedList[i].visited = false;
            }

            for (int i = 0; i < Math.Sqrt(adj.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(adj.Length); j++)
                {
                    adj[i, j] = int.MaxValue;
                }
            }

            foreach (Edge e in edges)
            {
                adj[e.start, e.end] = adj[e.end, e.start] = e.Wieght;
            }
        }

        private double GetHVal(int vertex)
        {
            int minDist = int.MaxValue;

            for(int i  = 0; i < Math.Sqrt(adj.Length); i++) {
                if (adj[i, vertex] < minDist) minDist = adj[i, vertex];
            }

            return minDist;
        }

        public async Task ExecuteAsync(int[] startEnd, GraphLayout layout)
        {
            OpenList.Add((0.0, startEnd[0]));

            ClosedList[startEnd[0]] = (0.0, 0.0, 0.0, true);

            while(OpenList.Count > 0)
            {
                var p = OpenList.Min;
                OpenList.Remove(p);
                layout.DrawNode(p.vertex);

                for(int i = 0; i < Math.Sqrt(adj.Length); i++) 
                {
                    if (adj[p.vertex, i] == int.MaxValue) continue;

                    if (startEnd[1] == i) 
                    { 
                        layout.DrawEdge(p.vertex, i);
                        layout.DrawNode(i);
                        MessageBox.Show($"destination found. Shortest path length is {ClosedList[p.vertex].g + adj[p.vertex, i]}");
                        return;
                    }

                    if (!ClosedList[i].visited)
                    {
                        double newG = ClosedList[p.vertex].g + adj[i, p.vertex];
                        double newH = GetHVal(i);
                        double newF = newG + newH;

                        if (ClosedList[i].f > newF)
                        {
                            OpenList.Add((newF, i));
                            layout.DrawEdge(i, p.vertex);
                            layout.DrawNode(i);

                            ClosedList[i].g = newG;
                            ClosedList[i].f = newF;
                            ClosedList[i].h = newH;

                            await Task.Delay(20);
                        }
                    }
                }
            }
        }
    }

    class BFSGraph
    {
        private List<int> q;
        private int[,] adj;
        private int vertices;
        private bool[] visited;

        public BFSGraph(int vertices, List<Edge> edges)
        {
            if (vertices <= 0) throw new ArgumentException("Cannot starn an algoritm without nodes");

            MessageBox.Show("Graph traversal ignores weights of edges", "Important", MessageBoxButtons.OK, MessageBoxIcon.Information);

            adj = new int[vertices, vertices];

            for (int i = 0; i < vertices; i++)
            {
                for (int j = 0; j < vertices; j++)
                {
                    adj[i, j] = 0;
                }
            }

            foreach (var edge in edges)
            {
                int u = edge.start;
                int v = edge.end;
                int w = edge.Wieght;
                adj[u, v] = w;
                adj[v, u] = w;
            }

            this.vertices = vertices;
            this.visited = new bool[vertices];
            q = new List<int>();
        }

        public async Task Execute(int[] startEnd, GraphLayout layout)
        {
            layout.DeselectAll();

            if (startEnd[0] < 0) { return; }
            int currNode = startEnd[0];

            q.Add(currNode);
            layout.DrawNode(currNode);
            visited[currNode] = true;

            while (q.Count > 0)
            {
                currNode = q[0];
                for (int i = 0; i < vertices; i++)
                {
                    if (!visited[i] && adj[currNode, i] > 0)
                    {
                        q.Add(i);
                        visited[i] = true;
                        layout.DrawNode(i);
                        await Task.Delay(100);

                        layout.DrawEdge(currNode, i); // Log here to ensure it's called
                        await Task.Delay(100);
                    }
                }
                q.RemoveAt(0);
            }
            //layout.DeselectAll();
        }
    }

    class DFSGraph
    {
        private Stack<int> stack = new Stack<int>();
        private int[,] adj;
        private int vertices;
        private bool[] visited;

        public DFSGraph(int vertices, List<Edge> edges)
        {
            if (vertices <= 0) throw new ArgumentException("Cannot starn an algoritm without nodes");

            MessageBox.Show("Graph traversal ignores weights of edges","Important", MessageBoxButtons.OK, MessageBoxIcon.Information);

            adj = new int[vertices, vertices];

            for (int i = 0; i < vertices; i++)
            {
                for (int j = 0; j < vertices; j++)
                {
                    adj[i, j] = 0;
                }
            }

            foreach (var edge in edges)
            {
                int u = edge.start;
                int v = edge.end;
                int w = edge.Wieght;
                adj[u, v] = w;
                adj[v, u] = w;
            }

            this.vertices = vertices;
            visited = new bool[vertices];
        }

        public async Task Execute(int[] startEnd, GraphLayout layout)
        {
            layout.DeselectAll();

            if (startEnd[0] < 0) { return; }

            int currNode = startEnd[0];
            visited[currNode] = true;
            stack.Push(currNode);

            while(stack.Count > 0) 
            {
                currNode = stack.Peek();
                int newNode = findNextNode(currNode);
                if(newNode < 0) stack.Pop();
                else
                {
                    stack.Push(newNode);
                    visited[newNode] = true;
                    layout.DrawNode(newNode);
                    await Task.Delay(100);
                    layout.DrawEdge(currNode, newNode);
                    await Task.Delay(100);
                }
            }
        }

        private int findNextNode(int node)
        {
            for(int u = 0; u < vertices; u++)
            {
                if (!visited[u] && adj[u, node] > 0)
                {
                    return u;
                }
            }
            return -1;
        }
    }
}
