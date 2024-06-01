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
        private Bitmap drawingBitmap;
        private List<List<int>> map;
        private int squares;
        private int[] startEnd;

        public const int SPACE = 0;
        public const int OBSTACLE = 1;
        public const int START = 2;
        public const int END = 3;

        public Form1()
        {
            InitializeComponent();
            drawingBitmap = new Bitmap(panel1.Width, panel1.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try { squares = Convert.ToInt32(textBox1.Text); }
            catch (FormatException)
            {
                MessageBox.Show("Invalid number of squares input");
                return;
            }

            map = new List<List<int>>(squares);

            for (int i = 0; i < squares; ++i)
            {
                map.Add(new List<int>(squares));
                for(int j = 0; j < squares; ++j)
                {
                    map[i].Add(SPACE);
                }
            }

            DrawGrid();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "1";
            squares = Convert.ToInt32(textBox1.Text);
            startEnd = new int[4];

            for(int i = 0; i < startEnd.Length; ++i)
            {
                startEnd[i] = -1;
            }

            map = new List<List<int>>(squares);

            for (int i = 0; i < squares; ++i)
            {
                map.Add(new List<int>(squares));
                for (int j = 0; j < squares; ++j)
                {
                    map[i].Add(SPACE);
                }
            }

            this.Size = new System.Drawing.Size(800, 800);
        }

        private void Form1_Paint(object sender, PaintEventArgs e) { }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Top = this.Height / 10;
            panel1.Left = this.Width / 15;
            panel1.Size = new System.Drawing.Size(
                Math.Min(8 * this.Width / 10, 12 * this.Height / 15), 
                Math.Min(8 * this.Width / 10, 12 * this.Height / 15)
            );
            
            DrawGrid();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(drawingBitmap, Point.Empty);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            int square_l;
            if (!int.TryParse(textBox1.Text, out square_l) || square_l <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer in the text box.");
                return;
            }

            int gridSize = panel1.Width / square_l;

            int square_top = y / gridSize;
            int square_left = x / gridSize;            

            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                Pen pen = new Pen(Color.Blue, 10);
                Brush brush = new SolidBrush(Color.Red);

                if(e.Button == MouseButtons.Left) 
                {
                    if (map[square_top][square_left] == OBSTACLE)
                    {
                        map[square_top][square_left] = SPACE;
                        DrawGrid();
                        return;
                    }
                    else if(map[square_top][square_left] == START)
                    {
                        startEnd[0] = startEnd[1] = -1;
                    }
                    else if(map[square_top][square_left] == END)
                    {
                        startEnd[2] = startEnd[3] = -1;
                    }
                    
                    g.FillRectangle(brush, new Rectangle(
                        new Point(gridSize * square_left, gridSize * square_top),
                        new Size(gridSize, gridSize)
                    ));

                    map[square_top][square_left] = OBSTACLE;
                }
                else if(e.Button == MouseButtons.Right) 
                {
                    if (map[square_top][square_left] == OBSTACLE)
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
            if (!int.TryParse(textBox1.Text, out squareSize) || squareSize <= 0)
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
                if(startEnd[0] == row && startEnd[1] == col) //Erase start point if pressed twice 
                {
                    map[startEnd[0]][startEnd[1]] = SPACE;
                    startEnd[0] = startEnd[1] = -1;
                }
                else
                {
                    if (startEnd[0] != -1) //check if we need to erase previous start point
                    {
                        map[startEnd[0]][startEnd[1]] = SPACE;
                    }
                    startEnd[0] = row;
                    startEnd[1] = col;
                    map[row][col] = START;
                }
            }
            else if (e.Button == MouseButtons.Right) // Set end point
            {
                if (startEnd[2] == row && startEnd[3] == col) //Erase end point if pressed twice 
                {
                    map[startEnd[2]][startEnd[3]] = SPACE;
                    startEnd[2] = startEnd[3] = -1;
                }
                else
                {
                    if (startEnd[2] != -1) //check if we need to erase previous end point
                    {
                        map[startEnd[2]][startEnd[3]] = 0;
                    }
                    startEnd[2] = row;
                    startEnd[3] = col;
                    map[row][col] = END;
                }
            }
            
            panel1.Controls.Clear();
            DrawGrid();
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        { panel1.Controls.Clear(); }

        private void DrawGrid()
        {
            // Initialize a new bitmap to clear previous drawings
            drawingBitmap = new Bitmap(panel1.Width, panel1.Height);

            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                Pen pen = new Pen(Color.Blue, 2); // Reduced pen width for grid lines
                Brush brush = new SolidBrush(Color.Red);

                int squares;
                try
                {
                    squares = Convert.ToInt32(textBox1.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Invalid number of squares input");
                    return;
                }

                int px_per_sq = panel1.Width / squares;

                // Draw vertical and horizontal grid lines
                for (int i = 0; i <= squares; ++i)
                {
                    g.DrawLine(pen, new Point(i * px_per_sq, 0), new Point(i * px_per_sq, panel1.Height));
                    g.DrawLine(pen, new Point(0, i * px_per_sq), new Point(panel1.Width, i * px_per_sq));
                }

                // Fill the rectangles based on the map
                for (int i = 0; i < squares; ++i)
                {
                    for (int j = 0; j < squares; ++j)
                    {
                        if (map[i][j] == 1)
                        {
                            g.FillRectangle(brush, new Rectangle(
                                new Point(j * px_per_sq, i * px_per_sq), // Corrected the x, y coordinates
                                new Size(px_per_sq, px_per_sq)
                            ));
                        }
                    }
                }

                DrawStartEnd(startEnd[0], startEnd[1], Color.Green);
                DrawStartEnd(startEnd[2], startEnd[3], Color.Red);

                pen.Dispose();
                brush.Dispose();
            }

            // Invalidate the panel to force a repaint
            panel1.Invalidate();
        }

        private void DrawStartEnd(int row, int col, Color color)
        {
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                Brush brush = new SolidBrush(color);

                int squares;
                try
                {
                    squares = Convert.ToInt32(textBox1.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Invalid number of squares input");
                    return;
                }

                int squareSize = panel1.Width / squares;
                g.FillEllipse(brush, new Rectangle(
                    new Point(col * squareSize + squareSize / 4, row * squareSize + squareSize / 4),
                    new Size(squareSize / 2, squareSize / 2)
                ));
                brush.Dispose();
            }

            panel1.Invalidate();
        }

        public void DrawCell(int y, int x, int y1, int x1, Color color = default(Color))
        {
            if (color == default(Color))
            {
                color = Color.Green;
            }

            int square_l = panel1.Width / map.Count;

            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                Pen pen = new Pen(color, 10);
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
            if (startEnd[0] < 0)
            {
                MessageBox.Show("Choose start point");
                return;
            }
            else if (startEnd[2] < 0)
            {
                MessageBox.Show("Choose end point");
                return;
            }

            DrawGrid();
            
            PathFinder pathFinder = new PathFinder(map);

            await pathFinder.FindPath(startEnd, this);
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

        public PathFinder(List<List<int>> map) 
        {
            ROW = map.Count;
            COL = map[0].Count;

            cells = new Cell[ROW, COL];
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    cells[i, j] = new Cell(map[i][j]);
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

                                await Task.Delay(400);
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
