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
        private graph Graph;
        private Dictionary<int, NodeControl> nodes;
        private Bitmap baseBitmap;
        private NodeControl firstSelectedNode;
        private Point? currentMousePosition;
        private const int nodeSize = 50;
        private int Id = 0;

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

            Canvas.Top = this.Height / 20;
            Canvas.Left = this.Width / 30;
            Canvas.Size = new System.Drawing.Size(
                9 * this.Width / 10,
                13 * this.Height / 15
            );

            Canvas.Paint += new PaintEventHandler(Canvas_Paint);
        }

        private void DrawNode(int val)
        {
            NodeControl node = new NodeControl(val, Id);
            Id++;
            int newX = 10, newY = 10;

            bool positionFound = false;

            while (!positionFound)
            {
                positionFound = true;

                // Check if the new position overlaps with any existing nodes
                foreach (var item in nodes)
                {
                    int distanceX = Math.Abs(newX - item.Value.Location.X);
                    int distanceY = Math.Abs(newY - item.Value.Location.Y);

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

            node.Location = new Point(newX, newY); // Set the initial location
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
        }

        private void GraphLayout_Resize(object sender, EventArgs e)
        {
            Canvas.Top = this.Height / 20;
            Canvas.Left = this.Width / 30;
            Canvas.Size = new System.Drawing.Size(
                9 * this.Width / 10,
                13 * this.Height / 15
            );

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
            int vertex1 = int.Parse(FirstNodeComboBox.SelectedItem.ToString());
            int vertex2 = int.Parse(SecondNodeComboBox.SelectedItem.ToString());
            int weight = -1;
            if(WeightTextBox.Text.Length != 0 && !int.TryParse(WeightTextBox.Text.ToString(), out weight))
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

        private void DrawEdges(Graphics g)
        {
            g.Clear(Color.White);

            foreach (var edge in Graph.edges)
            {
                int firstX = nodes[edge.Item1].Location.X + nodeSize / 2;
                int secondX = nodes[edge.Item2].Location.X + nodeSize / 2;
                int firstY = nodes[edge.Item1].Location.Y + nodeSize / 2;
                int secondY = nodes[edge.Item2].Location.Y + nodeSize / 2;

                int midX = (firstX + secondX) / 2;
                int midY = (firstY + secondY) / 2;

                using (Pen pen = new Pen(Color.LightBlue, 2))
                {
                    g.DrawLine(pen, firstX, firstY, secondX, secondY);
                }
                using (Font font = new Font("Arial", 10))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    g.DrawString(edge.Item3.ToString(), font, brush, new Point(midX, midY));
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

        public void NodeControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NodeControl node = sender as NodeControl;
            firstSelectedNode = node; // Set the first selected node
            node.ResizeNode(NodeEnlargement);
        }

        public void NodeControl_MouseClick(object sender, MouseEventArgs e)
        {
            NodeControl node = sender as NodeControl;
            var InputDialog = new InputDialog("Enter weight of an edge: ");

            if (firstSelectedNode != null && firstSelectedNode != node)
            {
                int vertex1 = firstSelectedNode.NodeValue;
                int vertex2 = node.NodeValue;
                int weight = -1;

                if (InputDialog.ShowDialog() != DialogResult.OK)
                    return;

                if(!int.TryParse(InputDialog.InputText, out weight))
                {
                    MessageBox.Show("Invalid input. Please enter a valid integer.");
                    return;
                }   

                if (Graph.AddEdge(vertex1, vertex2, weight))
                    DeleteEdgeComboBox.Items.Add(vertex1.ToString() + "-" + vertex2.ToString());
                
                DrawEdges(Graphics.FromImage(baseBitmap));

                firstSelectedNode.ResizeNode(-NodeEnlargement);
                firstSelectedNode = null; // Reset the first selected node
            }
            Canvas.Invalidate();
        }//ALSO ADDS AN EDGE!!!

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (firstSelectedNode != null)
            {
                firstSelectedNode.ResizeNode(-NodeEnlargement);
                firstSelectedNode = null;
            }
                
        }

        private void DeleteEdge_Click(object sender, EventArgs e)
        {
            if (DeleteEdgeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select an edge to delete.");
                return;
            }

            string edge = DeleteEdgeComboBox.SelectedItem.ToString();
            int[] vertices = edge.Split('-').Select(int.Parse).ToArray();

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

            Graph.AddNode();

            FirstNodeComboBox.Items.Add(val);
            SecondNodeComboBox.Items.Add(val);
            DeleteNodeComboBox.Items.Add(val);

            DrawNode(val);
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
            Graph.edges.RemoveAll(edge => edge.Item1 == selectedNodeValue || edge.Item2 == selectedNodeValue);
            Graph.RemoveNode();

            // Reset the selected index of the delete combo box
            DeleteNodeComboBox.SelectedIndex = -1;

            // Redraw the edges and invalidate the canvas
            DrawEdges(Graphics.FromImage(baseBitmap));
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

    }
}
