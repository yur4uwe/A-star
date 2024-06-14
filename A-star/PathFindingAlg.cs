using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_star
{
    internal class PathFindingAlg
    {
        private class Cell
        {
            public double f { get; set; }
            public double g;
            public double h;
            public int type;
            public int parent_x;
            public int parent_y;

            public Cell(int type = Form1.SPACE, double g = 0.0, double h = 0.0, double f = Double.MaxValue, int parent_x = -1, int parent_y = -1)
            {
                this.type = type;
                this.f = f;
                this.g = g;
                this.h = h;
                this.parent_x = parent_x;
                this.parent_y = parent_y;
            }
        }

        private Cell[,] cells; //???
        private SortedSet<(double, int, int)> OpenList;
        private bool[,] CloseList;

        private int ROW;
        private int COL;

        public PathFindingAlg(int GridX, int GridY, HashSet<(int, int)> obstacles)
        {
            ROW = GridX;
            COL = GridY;

            cells = new Cell[ROW, COL];

            foreach (var item in obstacles)
            {
                cells[item.Item1, item.Item2] = new Cell(Form1.OBSTACLE);
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

            cells[startEnd[0], startEnd[1]] = new Cell(Form1.START, 0.0, 0.0, 0.0);

            while (OpenList.Count > 0)
            {
                (double f, int x, int y) p = OpenList.Min;
                OpenList.Remove(p);

                int x = p.x;
                int y = p.y;

                CloseList[x, y] = true;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;

                        int newX = x + i;
                        int newY = y + j;

                        if (!Valid(newX, newY)) continue;

                        if (cells[newX, newY] == null) 
                            cells[newX, newY] = new Cell();

                        if (newX == startEnd[2] && newY == startEnd[3])
                        {
                            form.DrawCell(x, y, newX, newY);
                            MessageBox.Show("Destination Reached");
                            cells[newX, newY].parent_x = x;
                            cells[newX, newY].parent_y = y;
                            BacktrackPath(newX, newY, form);
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
                                cells[newX, newY].parent_x = x;
                                cells[newX, newY].parent_y = y;

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

        private void BacktrackPath(int endRow, int endCol, Form1 form)
        {
            int currRow = endRow;
            int currCol = endCol;

            while (cells[currRow, currCol].type != Form1.START)
            {
                int parentRow = cells[currRow, currCol].parent_x;
                int parentCol = cells[currRow, currCol].parent_y;
                form.DrawCell(currRow, currCol, parentRow, parentCol, Color.Blue);
                currRow = parentRow;
                currCol = parentCol;
            }
        }
    }
}

