using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_star
{
    public class NodeControl : UserControl
    {
        public int NodeValue { get; private set; }

        public NodeControl(int value)
        {
            NodeValue = value;
            this.Size = new Size(50, 50);
            this.BackColor = Color.LightBlue;
            this.BorderStyle = BorderStyle.FixedSingle;

            // Enable double buffering to reduce flicker
            this.DoubleBuffered = true;

            // Add mouse event handlers for dragging
            this.MouseDown += NodeControl_MouseDown;
            this.MouseMove += NodeControl_MouseMove;
            this.MouseUp += NodeControl_MouseUp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

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
            }
        }

        private void NodeControl_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}
