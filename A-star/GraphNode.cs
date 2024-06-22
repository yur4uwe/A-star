using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_star
{
    public class NodeControl : UserControl
    {
        public int NodeValue { get; private set; }
        public int NodeID {  get; set; }

        public NodeControl(int value, int ID)
        {
            NodeValue = value;
            NodeID = ID;
            this.Size = new Size(50, 50);
            this.BackColor = Color.Transparent; // Set background color to transparent
            this.DoubleBuffered = true; // Enable double buffering to reduce flicker

            // Set the region to a circle
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            this.Region = new Region(path);

            // Add mouse event handlers for dragging
            this.MouseDown += NodeControl_MouseDown;
            this.MouseMove += NodeControl_MouseMove;
            this.MouseUp += NodeControl_MouseUp;
            this.MouseDoubleClick += NodeControl_MouseDoubleClick;
            this.MouseClick += NodeControl_MouseClick;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not call base.OnPaintBackground to avoid painting the default background
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw a circle
            using (Brush brush = new SolidBrush(Color.LightBlue))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(brush, 0, 0, this.Width, this.Height);
            }

            // Draw the node value
            using (Font font = new Font("Arial", 10))
            using (Brush brush = new SolidBrush(Color.Black))
            {
                string text = NodeValue.ToString();
                SizeF textSize = e.Graphics.MeasureString(text, font);
                PointF textPosition = new PointF(
                    (this.Width - textSize.Width) / 2,
                    (this.Height - textSize.Height) / 2);
                e.Graphics.DrawString(text, font, brush, textPosition);
            }
        }

        private bool isDragging = false;
        private Point dragStartPoint;

        private void NodeControl_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            dragStartPoint = e.Location;
        }

        private void NodeControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var p = this.Parent.PointToClient(MousePosition);
                this.Location = new Point(p.X - dragStartPoint.X, p.Y - dragStartPoint.Y);
                this.Parent.Invalidate(); // Invalidate the parent control to trigger a repaint
            }
        }

        private void NodeControl_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void NodeControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Parent.Parent is GraphLayout graphLayout)
            {
                graphLayout.NodeControl_MouseDoubleClick(sender, e);
                //ResizeNode(20); // Increase the size by 20
            }
        }

        private void NodeControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.Parent.Parent is GraphLayout graphLayout)
            {
                graphLayout.NodeControl_MouseClick(sender, e);
                //ResizeNode(-20); // Reset the size (reduce by 20)
            }
        }

        public void ResizeNode(int change)
        {
            this.Size = new Size(this.Width + change, this.Height + change);

            // Update the region to match the new size
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            this.Region = new Region(path);

            // Adjust the location to keep the node centered
            this.Location = new Point(
                this.Location.X - change / 2,
                this.Location.Y - change / 2
            );

            this.Parent.Invalidate(); // Invalidate the parent control to trigger a repaint
        }

        public override string ToString()
        {
            return "Node ID: " + NodeID.ToString() + "| Node value: " + NodeValue.ToString();
        }
    }
}
