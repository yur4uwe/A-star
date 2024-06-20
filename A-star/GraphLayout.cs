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
        private const int nodeSize = 50;

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

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(NodeVal.Text, out int val))
                return;

            if (nodes.ContainsKey(val))
            {
                MessageBox.Show("Node values can't repeat");
                return;
            }

            Graph.AddNode(val);

            FirstNodeComboBox.Items.Add(val);
            SecondNodeComboBox.Items.Add(val);
            DeleteNodeComboBox.Items.Add(val); 

            DrawNode(val);
        }

        private void DrawNode(int val)
        {
            NodeControl node = new NodeControl(val);
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
            baseBitmap = new Bitmap(Canvas.Width, Canvas.Height);
            DrawEdges(e.Graphics);// Draw edges on the provided Graphics object
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
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
                nodes.Remove(selectedNodeValue);
            }

            // Remove the node value from the combo boxes
            RemoveItemFromComboBox(FirstNodeComboBox, selectedNodeValue);
            RemoveItemFromComboBox(SecondNodeComboBox, selectedNodeValue);
            RemoveItemFromComboBox(DeleteNodeComboBox, selectedNodeValue);

            // Remove edges connected to the node
            Graph.edges.RemoveAll(edge => edge.Item1 == selectedNodeValue || edge.Item2 == selectedNodeValue);

            // Reset the selected index of the delete combo box
            DeleteNodeComboBox.SelectedIndex = -1;

            // Redraw the edges and invalidate the canvas
            DrawEdges(Graphics.FromImage(baseBitmap));
            Canvas.Invalidate(); // Trigger a repaint after modifying the graph

            MessageBox.Show($"Node {selectedNodeValue} deleted successfully.");
        }

        private void RemoveItemFromComboBox(ToolStripComboBox comboBox, int itemValue)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i].ToString() == itemValue.ToString())
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

            if (vertex1 == vertex2)
            {
                MessageBox.Show("Cannot connect a node to itself.");
                return;
            }

            Graph.AddEdge(vertex1, vertex2);
            DrawEdges(Graphics.FromImage(baseBitmap));
            Canvas.Invalidate(); // Trigger a repaint after modifying the graph
        }

        private void DrawEdges(Graphics g)
        {
            foreach (var edge in Graph.edges)
            {
                int firstX = nodes[edge.Item1].Location.X + nodeSize / 2;
                int secondX = nodes[edge.Item2].Location.X + nodeSize / 2;
                int firstY = nodes[edge.Item1].Location.Y + nodeSize / 2;
                int secondY = nodes[edge.Item2].Location.Y + nodeSize / 2;

                using (Pen pen = new Pen(Color.LightBlue, 2))
                {
                    g.DrawLine(pen, firstX, firstY, secondX, secondY);
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
        }

        public void NodeControl_MouseClick(object sender, MouseEventArgs e)
        {
            NodeControl node = sender as NodeControl;

            if (firstSelectedNode != null && firstSelectedNode != node)
            {
                int vertex1 = firstSelectedNode.NodeValue;
                int vertex2 = node.NodeValue;

                Graph.AddEdge(vertex1, vertex2);
                DrawEdges(Graphics.FromImage(baseBitmap));

                firstSelectedNode = null; // Reset the first selected node
            }
            Canvas.Invalidate();
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (firstSelectedNode != null)
                firstSelectedNode = null;
        }
    }
}
