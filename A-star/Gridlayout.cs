using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace A_star
{
    public partial class Gridlayout : Form
    {
        private Bitmap baseBitmap;
        private Bitmap gridBitmap;
        private Bitmap cellBitmap;
        private Bitmap pathBitmap;
        private int[,] map;
        private HashSet<(int, int)> obstacles;
        private int squares;
        private int[] startEnd;
        private bool PLACE_OBSTACLE = true;
        private bool PLACE_START;
        private int GridX;
        private int GridY;

        public const int SPACE = 100;
        public const int OBSTACLE = 101;
        public const int START = 102;
        public const int END = 103;

        public Gridlayout()
        {
            InitializeComponent();
            InitializeBitmaps();
        }

        private void InitializeBitmaps()
        {

            int width = panel1.Width;
            int height = panel1.Height;

            baseBitmap = new Bitmap(width, height);

            gridBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(gridBitmap))
            {
                g.Clear(Color.Transparent);
            }

            cellBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(cellBitmap))
            {
                g.Clear(Color.Transparent);
            }

            pathBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(pathBitmap))
            {
                g.Clear(Color.Transparent);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearPath();
            try { squares = Convert.ToInt32(GridSizeInp.Text); }
            catch (FormatException)
            {
                MessageBox.Show("Invalid number of squares input");
                return;
            }

            GridX = GridY = squares;

            DrawGrid();
            DrawCells();
            panel1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridSizeInp.Text = "1";
            squares = Convert.ToInt32(GridSizeInp.Text);
            startEnd = new int[4] { -1, -1, -1, -1 };

            map = new int[,] { { 0 } };
            obstacles = new HashSet<(int, int)>();
            GridX = GridY = squares;

            this.Size = new System.Drawing.Size(800, 800);
        }

        private void Form1_Paint(object sender, PaintEventArgs e) { }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ClearPath();
            panel1.Top = this.Height / 20;
            panel1.Left = this.Width / 30;
            panel1.Size = new System.Drawing.Size(
                Math.Min(9 * this.Width / 10, 13 * this.Height / 15),
                Math.Min(9 * this.Width / 10, 13 * this.Height / 15)
            );

            InitializeBitmaps();
            DrawCells();
            DrawGrid();
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Draw images without using 'using' statement
            g.DrawImage(baseBitmap, 0, 0);
            g.DrawImage(gridBitmap, 0, 0);
            g.DrawImage(cellBitmap, 0, 0);
            g.DrawImage(pathBitmap, 0, 0);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Left) return;

            ClearPath();
            int x = e.X;
            int y = e.Y;

            int square_l;
            if (!int.TryParse(GridSizeInp.Text, out square_l) || square_l <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer in the text box.");
                return;
            }

            int gridSize = panel1.Width / square_l;

            int square_top = y / gridSize;
            int square_left = x / gridSize;

            if (PLACE_OBSTACLE)
            {
                if (obstacles.Contains((square_top, square_left)))
                {
                    obstacles.Remove((square_top, square_left));
                    DrawCells();
                    return;
                }
                else if (startEnd[0] == square_top && startEnd[1] == square_left)
                {
                    startEnd[0] = startEnd[1] = -1;
                }
                else if (startEnd[2] == square_top && startEnd[3] == square_left)
                {
                    startEnd[2] = startEnd[3] = -1;
                }

                obstacles.Add((square_top, square_left));
            }
            else
            {
                if (PLACE_START)
                {
                    startEnd[0] = square_top;
                    startEnd[1] = square_left;
                }
                else
                {
                    startEnd[2] = square_top;
                    startEnd[3] = square_left;
                }

                PLACE_OBSTACLE = true;
            }

            DrawCells();
            panel1.Invalidate();
        }

        private void DrawCells()
        {
            using (Graphics g = Graphics.FromImage(cellBitmap))
            {
                g.Clear(Color.Transparent);

                int squares;
                try { squares = Convert.ToInt32(GridSizeInp.Text); }
                catch (FormatException)
                {
                    MessageBox.Show("Invalid number of squares input");
                    return;
                }

                int squareSize = panel1.Width / squares;

                if (startEnd[0] != -1)
                    g.FillEllipse(new SolidBrush(Color.Green), new Rectangle(
                        new Point(startEnd[1] * squareSize + squareSize / 4, startEnd[0] * squareSize + squareSize / 4),
                        new Size(squareSize / 2, squareSize / 2)
                    ));
                if (startEnd[2] != -1)
                    g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(
                        new Point(startEnd[3] * squareSize + squareSize / 4, startEnd[2] * squareSize + squareSize / 4),
                        new Size(squareSize / 2, squareSize / 2)
                    ));

                foreach(var item in obstacles)
                {
                    g.FillRectangle(new SolidBrush(Color.Red), new Rectangle(
                        new Point(item.Item2 * squareSize, item.Item1 * squareSize), // Corrected the x, y coordinates
                        new Size(squareSize, squareSize)
                    ));
                }
            }
        }

        private void DrawGrid()
        {
            using (Graphics g = Graphics.FromImage(gridBitmap))
            {
                g.Clear(Color.Transparent);
                Pen pen = new Pen(Color.Blue, 2);
                int px_per_sq = panel1.Width / squares;

                for (int i = 0; i <= squares; ++i)
                {
                    g.DrawLine(pen, new Point(i * px_per_sq, 0), new Point(i * px_per_sq, panel1.Height));
                    g.DrawLine(pen, new Point(0, i * px_per_sq), new Point(panel1.Width, i * px_per_sq));
                }
            }
        }

        private void ClearPath()
        {
            using (Graphics graphics = Graphics.FromImage(pathBitmap)) { graphics.Clear(Color.Transparent); }
            panel1.Invalidate();
        }

        public void DrawCell(int y, int x, int y1, int x1, Color? color = null)
        {
            Color actualColor = color ?? Color.Green;

            int square_l = (int)(panel1.Width / squares);

            using (Graphics g = Graphics.FromImage(pathBitmap))
            {
                Pen pen = new Pen(actualColor, Math.Min(10, square_l / 5));
                g.DrawLine(pen,
                    new Point(x * square_l + square_l / 2, y * square_l + square_l / 2),
                    new Point(x1 * square_l + square_l / 2, y1 * square_l + square_l / 2)
                );
                pen.Dispose();
            }
            panel1.Invalidate(true);
        }

        private void clearPathBtn_Click(object sender, EventArgs e)
        {
            ClearPath();
        }

        private void placeStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PLACE_OBSTACLE = false;
            PLACE_START = true;
        }

        private void placeEndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PLACE_OBSTACLE = false;
            PLACE_START = false;
        }

        private async void aStToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearPath();

            PathFindingAlg pathFinder = new PathFindingAlg(squares, squares, obstacles);
            await pathFinder.A_Star(startEnd, this);
        }

        private async void bFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearPath();

            BFSAlg pathFinder = new BFSAlg(squares, squares, obstacles);
            await pathFinder.BFS(startEnd, this);
        }

        private async void dijkstraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearPath();

            DijkstraAlg pathFinder = new DijkstraAlg(squares, squares, obstacles);
            await pathFinder.Dijkstra(startEnd, obstacles.Count, this);
        }

        private void graphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            GraphLayout graphLayout = new GraphLayout();
            graphLayout.ShowDialog();
            this.Close();
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Gridlayout());
        }
    }
}
