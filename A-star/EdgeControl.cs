using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_star
{
    public partial class EdgeControl : UserControl
    {
        public NodeControl start { get; private set; }
        public NodeControl end { get; private set; }
        public int weight { get; private set; }
        private const int MinWidth = 10;
        private SizeF textSize;
        private Color EdgeColor = Color.LightBlue;

        public EdgeControl(NodeControl start, NodeControl end, int weight)
        {
            Init(start, end, weight);
        }

        public EdgeControl(Edge edge, GraphLayout layout)
        {
            Init(layout.GetNodeControlByID(edge.start), layout.GetNodeControlByID(edge.end), edge.Wieght);
        }

        private void Init(NodeControl start, NodeControl end, int weight)
        {
            InitializeComponent();
            this.start = start;
            this.end = end;
            this.weight = weight;
            DoubleBuffered = true;
            this.MouseClick += OnClick;
            SubscribeToNodeEvents();
            CalculateTextSize();
            UpdatePositionAndSize();
        }

        private void CalculateTextSize()
        {
            using (Graphics g = CreateGraphics())
            using (Font font = new Font("Arial", 10, FontStyle.Bold))
            {
                textSize = g.MeasureString(weight.ToString(), font);
            }
        }

        public void UpdatePositionAndSize()
        {
            if (this.start == null || this.end == null) return;

            // Calculate the positions of the start and end nodes
            Point startPos = start.NodeCenter;
            Point endPos = end.NodeCenter;

            // Calculate the bounding box that encompasses both nodes
            int minX = Math.Min(startPos.X, endPos.X) - 25;
            int minY = Math.Min(startPos.Y, endPos.Y) - 25;
            int maxX = Math.Max(startPos.X, endPos.X) + 25;
            int maxY = Math.Max(startPos.Y, endPos.Y) + 25;

            // Set location and size, ensuring the width is at least MinWidth
            this.Location = new Point(minX, minY);
            int width = Math.Max(maxX - minX, MinWidth) + 25;
            int height = Math.Max(maxY - minY, MinWidth) + 25;
            this.Size = new Size(width, height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.start == null || this.end == null) return;

            // Calculate the positions of the start and end nodes relative to the control
            Point startPos = start.NodeCenter;
            Point endPos = end.NodeCenter;

            Point relativeStartPos = new Point(startPos.X - this.Location.X, startPos.Y - this.Location.Y);
            Point relativeEndPos = new Point(endPos.X - this.Location.X, endPos.Y - this.Location.Y);

            // Draw the line
            using (Pen pen = new Pen(EdgeColor, 3))
            using (GraphicsPath path = new GraphicsPath())
            {
                e.Graphics.DrawLine(pen, relativeStartPos, relativeEndPos);

                // Add the line to the path
                path.AddLine(relativeStartPos, relativeEndPos);

                // Draw the weight label using AddString
                using (Font font = new Font("Arial", 10, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    string weightText = weight.ToString();

                    // Calculate the midpoint of the line
                    Point midPoint = new Point((relativeStartPos.X + relativeEndPos.X) / 2, (relativeStartPos.Y + relativeEndPos.Y) / 2);

                    // Measure the size of the text
                    SizeF textSize = e.Graphics.MeasureString(weightText, font);

                    // Position the text centered on the midpoint
                    PointF textPosition = new PointF(midPoint.X - textSize.Width / 2, midPoint.Y - textSize.Height / 2);

                    // Create a StringFormat to center the text
                    using (StringFormat format = new StringFormat())
                    {
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;

                        // Draw the string directly on graphics
                        e.Graphics.DrawString(weightText, font, brush, textPosition, format);

                        // Add the string to the path
                        path.AddString(weightText, font.FontFamily, (int)font.Style, font.Size, textPosition, format);
                    }
                }

                // Widen the path to include the line and the text
                path.Widen(new Pen(Color.White, 10));

                // Set the region of the control
                this.Region = new Region(path);
            }
        }

    private void SubscribeToNodeEvents()
        {
            if (start != null)
            {
                start.PositionChanged += Node_PositionChanged;
            }
            if (end != null)
            {
                end.PositionChanged += Node_PositionChanged;
            }
        }

        private void UnsubscribeFromNodeEvents()
        {
            if (start != null)
            {
                start.PositionChanged -= Node_PositionChanged;
            }
            if (end != null)
            {
                end.PositionChanged -= Node_PositionChanged;
            }
        }

        private void Node_PositionChanged(object sender, EventArgs e)
        {
            UpdatePositionAndSize();
            Invalidate();
        }

        public void PaintEdge(Color color)
        {
            this.EdgeColor = color;
            Invalidate();
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (this.Parent.Parent is GraphLayout layout)
            {
                layout.EdgeClick(sender, e);
            }
        }
    }
}
