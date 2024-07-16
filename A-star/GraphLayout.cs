using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace A_star
{
    public partial class GraphLayout : Form
    {

        private graph Graph; //List of edges uses nodes ID
        /// <summary>
        /// maps value to a node
        /// </summary>
        private Dictionary<int, NodeControl> nodes;
        private Bitmap baseBitmap;
        private List<EdgeControl> edges = new List<EdgeControl>();
        private NodeControl firstSelectedNode;
        /// <summary>
        /// Stores edges that need to be paited in different color
        /// </summary>
        HashSet<(int, int)> EdgesToPaint;
        private Point currentMousePosition = new Point(0, 0);
        private const int nodeSize = 50;
        private int Id = 0;
        /// <summary>
        /// Contains IDs of start and end nodes
        /// </summary>
        private int[] startEnd = new int[2];


        public const int NodeEnlargement = 10;

        public GraphLayout()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            int width = Canvas.Width;
            int height = Canvas.Height;

            baseBitmap = new Bitmap(width, height);
            Graph = new graph();
            nodes = new Dictionary<int, NodeControl>();
            EdgesToPaint = new HashSet<(int, int)>();

            Canvas.Top = this.Height / 20;
            Canvas.Left = this.Width / 30;
            Canvas.Size = new System.Drawing.Size(
                9 * this.Width / 10 - 210,
                13 * this.Height / 15
            );

            Result.Top = Canvas.Top;
            Result.Left = Canvas.Left + Canvas.Width + 10;
            Result.Height = Canvas.Height;

            startEnd[0] = -1;
            startEnd[1] = -1;

            Canvas.Paint += new PaintEventHandler(Canvas_Paint);

            DefaultGraph();
        }
        /// <summary>
        /// Creates a new node on canvas with specified value
        /// </summary>
        /// <param name="val">Value that will be displayed on node when it`s created</param>
        /// <param name="newX">Optional parameter for x coordinate of a new node</param>
        /// <param name="newY">Optional parameter for y coordinate of a new node</param>
        private void CreateNode(int val, double newX = 10, double newY = 10)
        {
            Graph.AddNode();

            FirstNodeComboBox.Items.Add(val);
            SecondNodeComboBox.Items.Add(val);
            DeleteNodeComboBox.Items.Add(val);

            NodeControl node = new NodeControl(val, Id);
            Id++;

            bool positionFound = false;

            while (!positionFound)
            {
                positionFound = true;

                // Check if the new position overlaps with any existing nodes
                foreach (var item in nodes)
                {
                    double distanceX = Math.Abs(newX - item.Value.Location.X);
                    double distanceY = Math.Abs(newY - item.Value.Location.Y);

                    if (distanceX < nodeSize && distanceY < nodeSize)
                    {
                        // Adjust the position if there's an overlap
                        newX += nodeSize;
                        if (newX + nodeSize > Canvas.Width)
                        {
                            newX = 10;
                            newY += nodeSize;
                        }
                        positionFound = false;
                        break;
                    }
                }
            }

            node.Location = new Point((int)newX, (int)newY); // Set the initial location
            nodes[val] = node;
            Canvas.Controls.Add(node);
            Canvas.Invalidate(true); // Trigger a repaint to ensure the node is drawn
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.DrawImage(baseBitmap, Point.Empty);
            
            if (firstSelectedNode != null)
            {
                int fx = firstSelectedNode.Location.X + nodeSize / 2;
                int fy = firstSelectedNode.Location.Y + nodeSize / 2;

                using (Pen pen = new Pen(Color.LightBlue, 2))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawLine(pen, new Point(fx, fy), currentMousePosition);
                }
            }

            int circleSize = 30; // Adjust the size of the circle as needed
            int x = currentMousePosition.X - circleSize / 2;
            int y = currentMousePosition.Y - circleSize / 2;

            if (settingStart)
            {
                using (Brush brush = new SolidBrush(Color.Green))
                {
                    e.Graphics.FillEllipse(brush, new Rectangle(x, y, circleSize, circleSize));
                }
            }
            else if(settingEnd)
                using (Brush brush = new SolidBrush(Color.Red))
                {
                    e.Graphics.FillEllipse(brush, new Rectangle(x, y, circleSize, circleSize));
                }
        }

        private void GraphLayout_Resize(object sender, EventArgs e)
        {
            Canvas.Top = this.Height / 20;
            Canvas.Left = this.Width / 30;
            Canvas.Size = new System.Drawing.Size(
                9 * this.Width / 10 - 210,
                13 * this.Height / 15
            );

            Result.Top = Canvas.Top;
            Result.Left = Canvas.Left + Canvas.Width + 10;
            Result.Height = Canvas.Height;

            Canvas.Invalidate(true);
        }

        private void RemoveItemFromComboBox(ToolStripComboBox comboBox, string itemValue)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i].ToString() == itemValue)
                {
                    comboBox.Items.RemoveAt(i);
                    break;
                }
            }
        }

        private void addEdge_Click(object sender, EventArgs e)
        {
            NodeControl vertex1 = nodes[int.Parse(FirstNodeComboBox.SelectedItem.ToString())];
            NodeControl vertex2 = nodes[int.Parse(SecondNodeComboBox.SelectedItem.ToString())];
            int weight = -1;
            if (WeightTextBox.Text.Length != 0 && !int.TryParse(WeightTextBox.Text.ToString(), out weight))
            {
                MessageBox.Show("Enter valid wieght(int)");
                return;
            }

            if (vertex1 == vertex2)
            {
                MessageBox.Show("Cannot connect a node to itself.");
                return;
            }

            Graph.AddEdge(vertex1.NodeID, vertex2.NodeID, weight);
            DeleteEdgeComboBox.Items.Add(vertex1.ToString() + "-" + vertex2.ToString());

            var edgeControl = new EdgeControl(vertex1, vertex2, weight);
            edges.Add(edgeControl);
            Canvas.Controls.Add(edgeControl);
            Canvas.Invalidate(true);
        }//Duplicate changes to NodeControl_MouseClick

        public NodeControl GetNodeControlByID(int nodeID)
        {
            return nodes.Values.FirstOrDefault(node => node.NodeID == nodeID);
        }

        private void changeLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the current form
            Gridlayout gridLayout = new Gridlayout(); // Show the current form again when GridLayout is closed
            gridLayout.ShowDialog(); // Show the GridLayout form non-modally
            this.Close();
        }
        /// <summary>
        /// Choosing first node for edge
        /// </summary>
        public void NodeControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NodeControl node = sender as NodeControl;
            if (firstSelectedNode == null)
            {
                firstSelectedNode = node;
                node.ResizeNode(NodeEnlargement);
            }
            else
            {
                firstSelectedNode.ResizeNode(-NodeEnlargement);
                firstSelectedNode = null;
            }
        }
        /// <summary>
        /// Choosing second node for edge and creating edge
        /// </summary>
        public void NodeControl_MouseClick(object sender, MouseEventArgs e)
        {
            NodeControl node = sender as NodeControl;

            if (firstSelectedNode != null && firstSelectedNode != node)
            {
                var InputDialog = new InputDialog("Enter weight of an edge: ");

                NodeControl vertex1 = firstSelectedNode;
                NodeControl vertex2 = node;
                int weight = -1;

                if (InputDialog.ShowDialog() != DialogResult.OK)
                    return;

                if (!int.TryParse(InputDialog.InputText, out weight))
                {
                    MessageBox.Show("Invalid input. Please enter a valid integer.");
                    return;
                }

                if (Graph.AddEdge(vertex1.NodeID, vertex2.NodeID, weight))
                    DeleteEdgeComboBox.Items.Add(firstSelectedNode.NodeValue.ToString() + "-" + node.NodeValue.ToString());

                var edgeControl = new EdgeControl(vertex1, vertex2, weight);
                edges.Add(edgeControl);
                Canvas.Controls.Add(edgeControl);

                firstSelectedNode.ResizeNode(-NodeEnlargement);
                firstSelectedNode = null;
            }
            else if (settingStart || settingEnd)
            {
                int settingWhat = settingStart ? 0 : 1;

                if (startEnd[settingWhat] != -1)
                {
                    nodes[startEnd[settingWhat]].state = NodeControl.PossibleState.inert;
                }

                startEnd[settingWhat] = node.NodeID;
                node.state = settingWhat == 0 ? NodeControl.PossibleState.start : NodeControl.PossibleState.end;

                if (settingWhat == 0)
                {
                    settingStart = false;
                }
                else
                {
                    settingEnd = false;
                }
            }
            Canvas.Invalidate(true);
        }//ALSO ADDS AN EDGE!!!
        /// <summary>
        /// Interrupts active functions 
        /// </summary>
        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (firstSelectedNode != null)
            {
                firstSelectedNode.ResizeNode(-NodeEnlargement);
                firstSelectedNode = null;
            }
            settingEnd = false;
            settingStart = false;
        }

        private void DeleteEdge_Click(object sender, EventArgs e)
        {
            if (DeleteEdgeComboBox.SelectedItem == null)
            {
                return;
            }

            string[] edgeNodes = DeleteEdgeComboBox.SelectedItem.ToString().Split('-');
            int firstNodeID = int.Parse(edgeNodes[0]);
            int secondNodeID = int.Parse(edgeNodes[1]);

            NodeControl firstNode = nodes[firstNodeID];
            NodeControl secondNode = nodes[secondNodeID];

            if (firstNode != null && secondNode != null)
            {
                // Remove the edge from the graph data structure
                Graph.RemoveEdge(firstNodeID, secondNodeID);

                // Define a predicate to find the matching edge
                Func<EdgeControl, bool> func = edge =>
                    (edge.start == firstNode && edge.end == secondNode) ||
                    (edge.start == secondNode && edge.end == firstNode);

                // Find the edge control to remove
                EdgeControl edgeToRemove = edges.FirstOrDefault(func);

                if (edgeToRemove != null)
                {
                    // Remove the edge control from the canvas and the list
                    Canvas.Controls.Remove(edgeToRemove);
                    edges.Remove(edgeToRemove);

                    // Remove the edge from the combo box
                    DeleteEdgeComboBox.Items.Remove(DeleteEdgeComboBox.SelectedItem);

                    // Invalidate the canvas to trigger a repaint
                    Canvas.Invalidate(true);
                }
                else
                {
                    // Debug information if edge not found
                    MessageBox.Show("Edge not found in the list.");
                }
            }
            else
            {
                // Debug information if nodes are not found
                MessageBox.Show("One or both nodes not found.");
            }
        }

        private void AddNode_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(NodeVal.Text, out int val))
                return;

            if (nodes.ContainsKey(val))
            {
                MessageBox.Show("Node values can't repeat");
                return;
            }

            CreateNode(val);
        }

        private void DeleteNode_Click(object sender, EventArgs e)
        {
            if (DeleteNodeComboBox.SelectedItem == null)
            {
                return;
            }

            int nodeID = int.Parse(DeleteNodeComboBox.SelectedItem.ToString());
            NodeControl nodeToDelete = GetNodeControlByID(nodeID);

            if (nodeToDelete != null)
            {
                foreach (var edge in edges.Where(edge => edge.start == nodeToDelete || edge.end == nodeToDelete).ToList())
                {
                    Graph.RemoveEdge(edge.start.NodeID, edge.end.NodeID);
                    edges.Remove(edge);
                    Canvas.Controls.Remove(edge);
                    DeleteEdgeComboBox.Items.Remove(edge.start.NodeValue.ToString() + "-" + edge.end.NodeValue.ToString());
                }

                Graph.RemoveNode();
                nodes.Remove(nodeID);
                Canvas.Controls.Remove(nodeToDelete);
                RemoveItemFromComboBox(FirstNodeComboBox, nodeID.ToString());
                RemoveItemFromComboBox(SecondNodeComboBox, nodeID.ToString());
                RemoveItemFromComboBox(DeleteNodeComboBox, nodeID.ToString());

                if (startEnd[0] == nodeID)
                {
                    startEnd[0] = -1;
                }
                else if (startEnd[1] == nodeID)
                {
                    startEnd[1] = -1;
                }

                Canvas.Invalidate(true);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePosition = e.Location;
            Canvas.Invalidate(true);
        }

        private void NormalizeID(int deletedID)
        {
            foreach (var node in nodes.Values)
            {
                if (node.NodeID > deletedID)
                {
                    node.NodeID--;
                }
            }
        }
        /// <summary>
        /// Used to display a message on form
        /// </summary>
        /// <param name="s">Any typeof(string)</param>
        public static void Mgs(string s)
        {
            MessageBox.Show(s);
        }
        /// <summary>
        /// Function for external use.
        /// Changes the color of node that has certain ID
        /// </summary>
        /// <param name="NodeID">Id of node</param>
        public void DrawNode(int NodeID)
        {
            NodeControl currNode = GetNodeControlByID(NodeID);
            currNode.state = NodeControl.PossibleState.selected;
            Canvas.Invalidate(true);
        }
        /// <summary>
        /// Draws an edges between two nodes
        /// </summary>
        /// <param name="start">Id of first node</param>
        /// <param name="end">id of second node</param>
        public void DrawEdge(int start, int end)
        {
            NodeControl startNode = GetNodeControlByID(start);
            NodeControl endNode = GetNodeControlByID(end);

            for(int i = 0; i < edges.Count; i++)
            {
                if ((edges[i].start == startNode || edges[i].start == endNode) && (edges[i].end == startNode || edges[i].end == endNode))
                {
                    edges[i].PaintEdge(Color.Orange);
                }
            }
        }
        /// <summary>
        /// Changes state of all nodes to default
        /// </summary>
        /// <param name="deselect_start">determines whether start will be removed</param>
        /// <param name="deselect_end">determines whether end will be removed</param>
        public void DeselectAll(bool deselect_start = false, bool deselect_end = false)
        {
            foreach (var item in this.nodes)
            {
                if (item.Value.NodeID == startEnd[0] && !deselect_start) item.Value.state = NodeControl.PossibleState.start;
                else if (item.Value.NodeID == startEnd[1] && !deselect_end) item.Value.state = NodeControl.PossibleState.end;
                else
                { item.Value.state = NodeControl.PossibleState.inert; }
            }

            foreach (var edge in edges)
            {
                edge.PaintEdge(Color.LightBlue);
            }
            Canvas.Invalidate(true);
        }
        /// <summary>
        /// Function for external use. 
        /// 
        /// </summary>
        /// <param name="predicate">determines which edges will be removed, if no edges specified - removes all edges</param>
        public void RemoveEdges(Func<EdgeControl, bool> predicate = null)
        {
            // If no predicate is provided, remove all edges
            if (predicate == null)
            {
                DeselectAll();
                return;
            }

            // Create a list of edges to remove based on the predicate
            var edgesToRemove = edges.Where(predicate).ToList();

            // Remove the edges
            foreach (var edge in edgesToRemove)
            {
                edge.PaintEdge(Color.LightBlue);
            }

            Canvas.Invalidate(true); // Redraw the canvas
        }

        bool settingStart = false;

        private void setStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DeselectAll();
            settingStart = true;
            settingEnd = false;
        }

        bool settingEnd = false;

        private void setEndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DeselectAll();
            settingEnd = true;
            settingStart = false;
        }
        /// <summary>
        /// Shows the results of a fnction in Result RichTextBox
        /// </summary>
        /// <param name="res">distance array</param>
        public void ShowResult(int[] res)
        {
            Result.Text = "";

            if (startEnd[1] >= 0)
                Result.Text = "Distance from start to end is " + res[startEnd[1]].ToString() + '\n';

            for (int i = 0; i < res.Length; i++)
            {
                if (i == startEnd[0]) continue;
                Result.Text += "Distance from " + GetNodeControlByID(startEnd[0]).Display() +
                                " to " + GetNodeControlByID(i).Display() + ": " + res[i];
                Result.Text += '\n';
            }

            this.Controls.Add(Result);
        }
        /// <summary>
        /// Complements Result RichTextBox with new information
        /// </summary>
        /// <param name="res"></param>
        public void AddResult(int[] res)
        {
            RichTextBox result = this.Controls.OfType<RichTextBox>().FirstOrDefault();

            if (startEnd[1] >= 0)
                result.Text += "\n+-----+\nDistance from start to end is " + res[startEnd[1]].ToString() + '\n';
            result.Text += "+-----+\n";
            for (int i = 0; i < res.Length; i++)
            {
                if (i == startEnd[0]) continue;
                
                result.Text += "Distance from " + GetNodeControlByID(startEnd[0]).Display() +
                                " to " + GetNodeControlByID(i).Display() + ": " + res[i];
                result.Text += '\n';
            }
        }

        private async void dFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Result.Text = "";
            DFSGraph dfs = new DFSGraph(Graph.vertices, Graph.edges);
            await dfs.Execute(startEnd, this);
        }

        private async void bFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Result.Text = "";
            BFSGraph bfs = new BFSGraph(Graph.vertices, Graph.edges);
            await bfs.Execute(startEnd, this);
        }
        /// <summary>
        /// Creates a simple default graph with 9 nodes
        /// </summary>
        private void DefaultGraph()
        {
            double x = 60, y = this.Canvas.Height / 3 - nodeSize / 2;
            CreateNode(1, x, y);
            CreateNode(2, x += nodeSize * 1.8, y -= nodeSize * 1.8);
            CreateNode(3, x += nodeSize * 1.8, y);
            CreateNode(4, x += nodeSize * 1.8, y);
            CreateNode(5, x += nodeSize * 1.8, y += nodeSize * 1.8);
            CreateNode(6, x -= nodeSize * 1.8, y += nodeSize * 1.8);
            CreateNode(7, x -= nodeSize * 1.8, y);
            CreateNode(8, x -= nodeSize * 1.8, y);
            CreateNode(9, x += nodeSize * 0.9, y -= nodeSize * 1.8);

            Graph.AddEdge(0, 1, 4);
            Graph.AddEdge(0, 7, 8);
            Graph.AddEdge(1, 7, 11);
            Graph.AddEdge(1, 2, 8);
            Graph.AddEdge(8, 7, 7);
            Graph.AddEdge(6, 7, 1);
            Graph.AddEdge(6, 8, 6);
            Graph.AddEdge(2, 8, 2);
            Graph.AddEdge(2, 5, 4);
            Graph.AddEdge(2, 3, 7);
            Graph.AddEdge(6, 5, 2);
            Graph.AddEdge(3, 5, 14);
            Graph.AddEdge(3, 4, 9);
            Graph.AddEdge(5, 4, 10);

            foreach (Edge edge in Graph.edges)
            {
                EdgeControl newEdge = new EdgeControl(edge, this);
                Canvas.Controls.Add(newEdge);
                edges.Add(newEdge);
                DeleteEdgeComboBox.Items.Add(newEdge.start.NodeValue.ToString() + "-" + newEdge.end.NodeValue.ToString());
            }

            Canvas.Invalidate(true);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DeselectAll();
        }

        public void EdgeClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This Edge will be deleted forever", "Delete Edge?", MessageBoxButtons.YesNo);

            EdgeControl MyEdge = sender as EdgeControl;

            if(result == DialogResult.Yes)
            {
                NodeControl firstNode = MyEdge.start;
                NodeControl secondNode = MyEdge.end;

                if (firstNode != null && secondNode != null)
                {
                    // Remove the edge from the graph data structure
                    Graph.RemoveEdge(firstNode.NodeID, secondNode.NodeID);

                    // Find the edge control to remove
                    EdgeControl edgeToRemove = edges[0];

                    for (int i = 0; i < edges.Count; i++)
                    {
                        if ((edges[i].start == firstNode || edges[i].end == firstNode) && (edges[i].start == secondNode || edges[i].end == secondNode))
                            edgeToRemove = edges[i];
                    }

                    if (edgeToRemove != null)
                    {
                        // Remove the edge control from the canvas and the list
                        Canvas.Controls.Remove(edgeToRemove);
                        edges.Remove(edgeToRemove);

                        // Remove the edge from the combo box
                        DeleteEdgeComboBox.Items.Remove(DeleteEdgeComboBox.SelectedItem);

                        // Invalidate the canvas to trigger a repaint
                        Canvas.Invalidate(true);
                    }
                    else
                    {
                        // Debug information if edge not found
                        MessageBox.Show("Edge not found in the list.");
                    }
                }
                else
                {
                    // Debug information if nodes are not found
                    MessageBox.Show("One or both nodes not found.");
                }
            }
            else if(result == DialogResult.No)
            {
                return;
            }

        }

        private async void dijksraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Result.Text = "";
            try
            {
                DijkstraGraph dijkstra = new DijkstraGraph(Graph.vertices, Graph.edges);
                await dijkstra.ExecuteAsync(startEnd, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void astarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Result.Text = "";
            try
            {
                A_StarGraph dijkstra = new A_StarGraph(Graph.vertices, Graph.edges);
                await dijkstra.ExecuteAsync(startEnd, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
