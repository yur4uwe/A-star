using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_star
{
    public partial class GraphLayout : Form
    {
        private graph Graph;
        private Dictionary<int, NodeControl> nodes;
        private Bitmap baseBitmap;
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
            node.Location = new Point(10, 10); // Set the initial location
            nodes[val] = node;
            Canvas.Controls.Add(node);
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(baseBitmap, Point.Empty);
        }

        private void GraphLayout_Resize(object sender, EventArgs e)
        {
            Canvas.Top = this.Height / 20;
            Canvas.Left = this.Width / 30;
            Canvas.Size = new System.Drawing.Size(
                9 * this.Width / 10,
                13 * this.Height / 15
            );
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

            // Reset the selected index of the delete combo box
            DeleteNodeComboBox.SelectedIndex = -1;

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
    }
}
