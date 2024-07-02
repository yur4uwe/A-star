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
        private NodeControl firstSelectedNode;
        /// <summary>
        /// Stores edges that need to be paited in different color
        /// </summary>
        HashSet<(int, int)> EdgesToPaint;
        private Point? currentMousePosition;
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
            Canvas.Invalidate(); // Trigger a repaint to ensure the node is drawn
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.DrawImage(baseBitmap, Point.Empty);
            DrawEdges(e.Graphics);
            if (firstSelectedNode != null && currentMousePosition.HasValue)
            {
                int fx = firstSelectedNode.Location.X + nodeSize / 2;
                int fy = firstSelectedNode.Location.Y + nodeSize / 2;

                using (Pen pen = new Pen(Color.LightBlue, 2))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawLine(pen, new Point(fx, fy), currentMousePosition.Value);
                }
            }
            
            //e.Graphics.FillEllipse(new SolidBrush(Color.Black), 0, 0, 50, 50);
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

            Canvas.Invalidate();
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
            int vertex1 = nodes[int.Parse(FirstNodeComboBox.SelectedItem.ToString())].NodeID;
            int vertex2 = nodes[int.Parse(SecondNodeComboBox.SelectedItem.ToString())].NodeID;
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

            Graph.AddEdge(vertex1, vertex2, weight);
            DeleteEdgeComboBox.Items.Add(vertex1.ToString() + "-" + vertex2.ToString());
            DrawEdges(Graphics.FromImage(baseBitmap));
            Canvas.Invalidate(); // Trigger a repaint after modifying the graph
        }//Duplicate changes to NodeControl_MouseClick

        private NodeControl GetNodeControlByID(int nodeID)
        {
            return nodes.Values.FirstOrDefault(node => node.NodeID == nodeID);
        }

        private void DrawEdges(Graphics g)
        {
            g.Clear(Color.White);

            foreach (var edge in Graph.edges)
            {
                NodeControl firstNode = GetNodeControlByID(edge.start);
                NodeControl secondNode = GetNodeControlByID(edge.end);

                if (firstNode != null && secondNode != null)
                {
                    int firstX = firstNode.Location.X + nodeSize / 2;
                    int secondX = secondNode.Location.X + nodeSize / 2;
                    int firstY = firstNode.Location.Y + nodeSize / 2;
                    int secondY = secondNode.Location.Y + nodeSize / 2;

                    int midX = (firstX + secondX) / 2;
                    int midY = (firstY + secondY) / 2;

                    if (EdgesToPaint.Contains((firstNode.NodeID, secondNode.NodeID)) || 
                        EdgesToPaint.Contains((secondNode.NodeID, firstNode.NodeID)))
                    {
                        using (Pen pen = new Pen(Color.Orange, 4))
                        {
                            g.DrawLine(pen, firstX, firstY, secondX, secondY);
                        }
                    }
                    else
                    {
                        using (Pen pen = new Pen(Color.LightBlue, 2))
                        {
                            g.DrawLine(pen, firstX, firstY, secondX, secondY);
                        }
                    }
                    
                    using (Font font = new Font("Arial", 10))
                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        g.DrawString(edge.Wieght.ToString(), font, brush, new Point(midX, midY));
                    }
                }
            }
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

                int vertex1 = firstSelectedNode.NodeID;
                int vertex2 = node.NodeID;
                int weight = -1;

                if (InputDialog.ShowDialog() != DialogResult.OK)
                    return;

                if (!int.TryParse(InputDialog.InputText, out weight))
                {
                    MessageBox.Show("Invalid input. Please enter a valid integer.");
                    return;
                }

                if (Graph.AddEdge(vertex1, vertex2, weight))
                    DeleteEdgeComboBox.Items.Add(firstSelectedNode.NodeValue.ToString() + "-" + node.NodeValue.ToString());

                DrawEdges(Graphics.FromImage(baseBitmap));

                firstSelectedNode.ResizeNode(-NodeEnlargement);
                firstSelectedNode = null; // Reset the first selected node
            }
            else if (settingStart || settingEnd)
            {
                int settingWhat = settingStart ? 0 : 1;
                bool startOrEnd = Convert.ToBoolean(settingWhat);

                if (startEnd[settingWhat] >= 0)
                {
                    NodeControl oldNode = GetNodeControlByID(startEnd[settingWhat]);
                    oldNode.state = NodeControl.PossibleState.inert;
                    oldNode.Invalidate();
                }

                node.state = startOrEnd ? NodeControl.PossibleState.end : NodeControl.PossibleState.start;
                startEnd[settingWhat] = node.NodeID;
                if (startEnd[0] == startEnd[1])
                {
                    if (startOrEnd) startEnd[0] = -1;
                    else startEnd[1] = -1;
                }
                node.Invalidate();
                settingStart = false;
                settingEnd = false;
            }
            Canvas.Invalidate();
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
                MessageBox.Show("Please select an edge to delete.");
                return;
            }

            string edge = DeleteEdgeComboBox.SelectedItem.ToString();
            int[] vals = edge.Split('-').Select(int.Parse).ToArray();

            int[] vertices = { nodes[vals[0]].NodeID, nodes[vals[1]].NodeID };

            RemoveItemFromComboBox(DeleteEdgeComboBox, edge);
            DeleteEdgeComboBox.Text = "Choose Edge";

            Graph.RemoveEdge(vertices[0], vertices[1], Graph.GetWeight(vertices[0], vertices[1]));
            DrawEdges(Graphics.FromImage(baseBitmap));
            Canvas.Invalidate();
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
            // Check if a node is selected
            if (DeleteNodeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a node to delete.");
                return;
            }

            // Get the selected node value
            int selectedNodeValue;
            bool isParsed = int.TryParse(DeleteNodeComboBox.SelectedItem.ToString(), out selectedNodeValue);
            if (!isParsed)
            {
                MessageBox.Show("Invalid node value.");
                return;
            }

            // Remove the node control from the canvas and the dictionary
            if (nodes.ContainsKey(selectedNodeValue))
            {
                Canvas.Controls.Remove(nodes[selectedNodeValue]);
                int ID = nodes[selectedNodeValue].NodeID;
                nodes.Remove(selectedNodeValue);
                NormalizeID(ID);
            }

            // Remove the node value from the combo boxes
            RemoveItemFromComboBox(FirstNodeComboBox, selectedNodeValue.ToString());
            RemoveItemFromComboBox(SecondNodeComboBox, selectedNodeValue.ToString());
            RemoveItemFromComboBox(DeleteNodeComboBox, selectedNodeValue.ToString());

            // Remove edges connected to the node
            Graph.edges.RemoveAll(edge => edge.start == selectedNodeValue || edge.end == selectedNodeValue);
            Graph.RemoveNode();

            // Reset the selected index of the delete combo box
            DeleteNodeComboBox.SelectedIndex = -1;

            Canvas.Invalidate(); // Trigger a repaint after modifying the graph

            MessageBox.Show($"Node {selectedNodeValue} deleted successfully.");
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (firstSelectedNode == null)
                return;

            currentMousePosition = e.Location;
            Canvas.Invalidate(); // Invalidate the canvas to trigger a repaint
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

            if (currNode != null)
            {
                using (Graphics g = Graphics.FromImage(baseBitmap))
                {
                    int x = currNode.Location.X;
                    int y = currNode.Location.Y;

                    g.FillEllipse(new SolidBrush(Color.Orange), x, y, nodeSize + 10, nodeSize + 10);
                    g.FillEllipse(new SolidBrush(Color.Black), 0, 0, 50, 50);
                }
                Canvas.Invalidate(true); // Trigger a repaint to ensure the node is drawn
                currNode.Invalidate();
            }
        }
        /// <summary>
        /// Draws an edges between two nodes
        /// </summary>
        /// <param name="start">Id of first node</param>
        /// <param name="end">id of second node</param>
        public void DrawEdge(int start, int end)
        {
            EdgesToPaint.Add((start, end));
            Canvas.Invalidate(true);
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
                if (item.Value.state == NodeControl.PossibleState.start && !deselect_start) continue;
                else if (item.Value.state == NodeControl.PossibleState.end && !deselect_end) continue;
                else
                { item.Value.state = NodeControl.PossibleState.inert; }
            }

            this.RemoveEdges();
            Canvas.Invalidate(true);
        }
        /// <summary>
        /// Function for external use. 
        /// 
        /// </summary>
        /// <param name="predicate">determines which edges will be removed, if no edges specified - removes all edges</param>
        public void RemoveEdges(Func<(int, int), bool> predicate = null)
        {
            // If no predicate is provided, remove all edges
            if (predicate == null)
            {
                EdgesToPaint.Clear();
                Canvas.Invalidate(); // Redraw the canvas
                return;
            }

            // Create a list of edges to remove based on the predicate
            var edgesToRemove = EdgesToPaint.Where(predicate).ToList();

            // Remove the edges
            foreach (var edge in edgesToRemove)
            {
                EdgesToPaint.Remove(edge);
            }

            Canvas.Invalidate(); // Redraw the canvas
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

        private async void findShortestPathToolStripMenuItem_Click(object sender, EventArgs e)
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

            Canvas.Invalidate();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DeselectAll();
        }
    }
}
