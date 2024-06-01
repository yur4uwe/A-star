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
    public partial class Form1 : Form
    {
        private Bitmap baseBitmap;
        private Bitmap gridBitmap;
        private Bitmap cellBitmap;
        private Bitmap pathBitmap;
        private int[,] map;
        private int squares;
        private int[] startEnd;

        public const int SPACE = 0;
        public const int OBSTACLE = 1;
        public const int START = 2;
        public const int END = 3;

        public Form1()
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

            map = new int[squares, squares];

            for (int i = 0; i < squares; ++i)
            {
                for (int j = 0; j < squares; ++j)
                {
                    map[i, j] = 0;
                }
            }

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

            this.Size = new System.Drawing.Size(800, 800);
        }

        private void Form1_Paint(object sender, PaintEventArgs e) { }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ClearPath();
            panel1.Top = this.Height / 10;
            panel1.Left = this.Width / 15;
            panel1.Size = new System.Drawing.Size(
                Math.Min(8 * this.Width / 10, 12 * this.Height / 15),
                Math.Min(8 * this.Width / 10, 12 * this.Height / 15)
            );

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

            using (Graphics g = Graphics.FromImage(cellBitmap))
            {
                Pen pen = new Pen(Color.Blue, 10);
                Brush brush = new SolidBrush(Color.Red);

                if (e.Button == MouseButtons.Left)
                {
                    if (map[square_top, square_left] == OBSTACLE)
                    {
                        map[square_top, square_left] = SPACE;
                        DrawCells();
                        return;
                    }
                    else if (map[square_top, square_left] == START)
                    {
                        startEnd[0] = startEnd[1] = -1;
                    }
                    else if (map[square_top, square_left] == END)
                    {
                        startEnd[2] = startEnd[3] = -1;
                    }

                    g.FillRectangle(brush, new Rectangle(
                        new Point(gridSize * square_left, gridSize * square_top),
                        new Size(gridSize, gridSize)
                    ));

                    map[square_top, square_left] = OBSTACLE;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (map[square_top, square_left] == OBSTACLE)
                        return;
                    Panel panel = new Panel();
                    panel.Location = new Point(gridSize * square_left, gridSize * square_top);
                    panel.Width = gridSize;
                    panel.Height = gridSize;
                    panel.MouseClick += new System.Windows.Forms.MouseEventHandler(Panel_MouseClick);
                    panel.MouseLeave += new System.EventHandler(Panel_MouseLeave);

                    Label instructions = new Label();
                    instructions.Text = "LMouseCl for start\nRMouseCl for end";
                    instructions.AutoSize = true;

                    panel.Controls.Add(instructions);
                    panel1.Controls.Add(panel);
                }

                pen.Dispose();
                brush.Dispose();
            }

            panel1.Invalidate();
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            int squareSize;
            if (!int.TryParse(GridSizeInp.Text, out squareSize) || squareSize <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer in the text box.");
                return;
            }

            Panel pnl = (Panel)sender;

            int gridSize = panel1.Width / squareSize;
            int row = pnl.Top / gridSize;
            int col = pnl.Left / gridSize;

            if (e.Button == MouseButtons.Left) // Set start point
            {
                if (startEnd[0] == row && startEnd[1] == col) //Erase start point if pressed twice 
                {
                    map[startEnd[0], startEnd[1]] = SPACE;
                    startEnd[0] = startEnd[1] = -1;
                }
                else
                {
                    if (startEnd[0] != -1) //check if we need to erase previous start point
                    {
                        map[startEnd[0], startEnd[1]] = SPACE;
                    }
                    startEnd[0] = row;
                    startEnd[1] = col;
                    map[row, col] = START;
                }
            }
            else if (e.Button == MouseButtons.Right) // Set end point
            {
                if (startEnd[2] == row && startEnd[3] == col) //Erase end point if pressed twice 
                {
                    map[startEnd[2], startEnd[3]] = SPACE;
                    startEnd[2] = startEnd[3] = -1;
                }
                else
                {
                    if (startEnd[2] != -1) //check if we need to erase previous end point
                    {
                        map[startEnd[2], startEnd[3]] = 0;
                    }
                    startEnd[2] = row;
                    startEnd[3] = col;
                    map[row, col] = END;
                }
            }

            panel1.Controls.Clear();
            DrawCells();
            panel1.Invalidate();
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        { panel1.Controls.Clear(); }

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

                for (int i = 0; i < squares; ++i)
                {
                    for (int j = 0; j < squares; ++j)
                    {
                        if (map[i, j] == 1)
                        {
                            g.FillRectangle(new SolidBrush(Color.Red), new Rectangle(
                                new Point(j * squareSize, i * squareSize), // Corrected the x, y coordinates
                                new Size(squareSize, squareSize)
                            ));
                        }
                    }
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

            int square_l = (int)(panel1.Width / Math.Sqrt(map.Length));

            using (Graphics g = Graphics.FromImage(pathBitmap))
            {
                Pen pen = new Pen(actualColor, 10);
                g.DrawLine(pen,
                    new Point(x * square_l + square_l / 2, y * square_l + square_l / 2),
                    new Point(x1 * square_l + square_l / 2, y1 * square_l + square_l / 2)
                );
                pen.Dispose();
            }
            panel1.Invalidate(true);
        }

        private async void PathBtn_Click(object sender, EventArgs e)
        {
            ClearPath();

            PathFinder pathFinder = new PathFinder(map);
            await pathFinder.FindPath(startEnd, this);
        }

        private void clearPathBtn_Click(object sender, EventArgs e)
        {
            ClearPath();
        }
    }

    class PathFinder
    {
        private class Cell
        {
            public double f { get; set; }
            public double g;
            public double h;
            public int type;

            public Cell(int type = 0, double g = 0.0, double h = 0.0, double f = Double.MaxValue)
            {
                this.type = type;
                this.f = f;
                this.g = g;
                this.h = h;
            }
        }
        
        private Cell[,] cells; //???
        private SortedSet<(double, int, int)> OpenList;
        private bool[,] CloseList;

        private int ROW;
        private int COL;

        public PathFinder(int[,] map) 
        {
            ROW = COL = (int)Math.Sqrt(map.Length);

            cells = new Cell[ROW, COL];
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    cells[i, j] = new Cell(map[i, j]);
                }
            }

            OpenList = new 
                SortedSet<(double, int, int)>
                (
                    Comparer<(double, int, int)>.Create((a, b) => a.Item1.CompareTo(b.Item1))
                );

            CloseList = new bool[ROW, COL];
        }

        public async Task FindPath(int[] startEnd, Form1 form) //now will be real A*
        {
            OpenList.Add((0.0, startEnd[0], startEnd[1]));

            cells[startEnd[0], startEnd[1]].f = 0.0;
            cells[startEnd[0], startEnd[1]].g = 0.0;
            cells[startEnd[0], startEnd[1]].h = 0.0;

            while (OpenList.Count > 0) 
            {
                (double f, int x, int y) p = OpenList.Min;
                OpenList.Remove(p);

                int x = p.x;
                int y = p.y;

                CloseList[x, y] = true;

                for(int i = -1; i <= 1; i++)
                {
                    for(int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;

                        int newX = x + i;
                        int newY = y + j;

                        if(!Valid(newX, newY)) continue;

                        if (newX == startEnd[2] && newY == startEnd[3]) 
                        {
                            form.DrawCell(x, y, newX, newY);
                            MessageBox.Show("Destination Reached"); 
                            return; 
                        }

                        if (!CloseList[newX, newY] && cells[newX, newY].type != Form1.OBSTACLE)
                        {
                            double newG = cells[x, y].g + 1.0;
                            double newH = GetHVal(newX, newY, startEnd[2], startEnd[3]);
                            double newF = newG + newH;

                            if (cells[newX, newY].f > newF)
                            {
                                OpenList.Add((newF, newX, newY));

                                form.DrawCell(x, y, newX, newY);

                                cells[newX, newY].f = newF;
                                cells[newX, newY].g = newG;
                                cells[newX, newY].h = newH;

                                await Task.Delay(100);
                            }
                        }
                    }
                }
            }
            MessageBox.Show("Destination not Reached");
        }

        private bool Valid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < ROW && y < COL;
        }

        private double GetHVal(int x, int y, int destX, int destY)
        {
            return Math.Sqrt(Math.Pow(x - destX, 2) + Math.Pow(y - destY, 2));
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Form1());
        }
    }
}
