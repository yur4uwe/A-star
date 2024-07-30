using A_star;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_star
{
    class Cell
    {
        public int type;
        public int parent_x;
        public int parent_y;
    }
    
    internal class PathFindingAlg
    {
        private class A_StCell : Cell
        {
            public double f { get; set; }
            public double g;
            public double h;

            public A_StCell(int type = Gridlayout.SPACE, double g = 0.0, double h = 0.0, double f = Double.MaxValue, int parent_x = -1, int parent_y = -1)
            {
                this.type = type;
                this.f = f;
                this.g = g;
                this.h = h;
                this.parent_x = parent_x;
                this.parent_y = parent_y;
            }
        }

        private A_StCell[,] cells; //???
        private SortedSet<(double, int, int)> OpenList;
        private bool[,] CloseList;

        private int ROW;
        private int COL;

        public PathFindingAlg(int GridX, int GridY, HashSet<(int, int)> obstacles)
        {
            ROW = GridX;
            COL = GridY;

            cells = new A_StCell[ROW, COL];

            foreach (var item in obstacles)
            {
                cells[item.Item1, item.Item2] = new A_StCell(Gridlayout.OBSTACLE);
            }

            OpenList = new
                SortedSet<(double, int, int)>
                (
                    Comparer<(double, int, int)>.Create((a, b) => a.Item1.CompareTo(b.Item1))
                );

            CloseList = new bool[ROW, COL];
        }

        public async Task A_Star(int[] startEnd, Gridlayout form) //now will be real A*
        {
            OpenList.Add((0.0, startEnd[0], startEnd[1]));

            cells[startEnd[0], startEnd[1]] = new A_StCell(Gridlayout.START, 0.0, 0.0, 0.0);

            while (OpenList.Count > 0)
            {
                (double f, int x, int y) p = OpenList.Min;      
                OpenList.Remove(p);
                form.DrawCell(p.x, p.y, Color.LightGreen);

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
                        double dist = (i != 0 && j != 0) ? Math.Sqrt(2) : 1;

                        if (!Valid(newX, newY)) continue;

                        if (cells[newX, newY] == null) 
                            cells[newX, newY] = new A_StCell();

                        if (newX == startEnd[2] && newY == startEnd[3])
                        {
                            form.DrawCell(newX, newY);
                            MessageBox.Show("Destination Reached");
                            cells[newX, newY].parent_x = x;
                            cells[newX, newY].parent_y = y;
                            BackTrack.BacktrackPath(cells, newX, newY, form);
                            return;
                        }

                        if (!CloseList[newX, newY] && cells[newX, newY].type != Gridlayout.OBSTACLE)
                        {
                            double newG = cells[x, y].g + dist;
                            double newH = GetHVal(newX, newY, startEnd[2], startEnd[3]);
                            double newF = newG + newH;

                            if (cells[newX, newY].f > newF)
                            {
                                OpenList.Add((newF, newX, newY));

                                form.DrawCell(newX, newY);

                                cells[newX, newY].f = newF;
                                cells[newX, newY].g = newG;
                                cells[newX, newY].h = newH;
                                cells[newX, newY].parent_x = x;
                                cells[newX, newY].parent_y = y;

                                await Task.Delay(1);
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

    internal class BFSAlg
    {
        class BFSCell : Cell
        {
            public bool visited;

            public BFSCell(int type = 0, bool visited = false, int parent_x = -1, int parent_y = -1)
            {
                this.type = type;
                this.visited = visited;
                this.parent_x = parent_x;
                this.parent_y = parent_y;
            }
        }

        public BFSAlg(int row, int col, HashSet<(int, int)> obstacles)
        {
            ROW = row;
            COL = col;

            this.q = new List<int[]>();
            this.map = new BFSCell[ROW, COL];

            foreach (var item in obstacles)
            {
                map[item.Item1, item.Item2] = new BFSCell(Gridlayout.OBSTACLE);
            }
        }

        private List<int[]> q;
        private BFSCell[,] map;
        private int ROW;
        private int COL;

        public async Task BFS(int[] startEnd, Gridlayout form)
        {
            q.Add(new int[2] { startEnd[0], startEnd[1] });
            this.map[startEnd[0], startEnd[1]] = new BFSCell(Gridlayout.START, true, -1, -1);

            while (q.Count > 0)
            {
                int[] currCell = q[0];
                q.RemoveAt(0);
                form.DrawCell(currCell[0], currCell[1], Color.LightGreen);

                // Define directions for movement (down, up, right, left, and diagonals)
                int[][] directions = new int[][]
                {
                new int[] { 1, 0 }, // down
                new int[] { -1, 0 }, // up
                new int[] { 0, 1 }, // right
                new int[] { 0, -1 }, // left
                new int[] { 1, 1 }, // down-right
                new int[] { 1, -1 }, // down-left
                new int[] { -1, 1 }, // up-right
                new int[] { -1, -1 } // up-left
                };

                foreach (var dir in directions)
                {
                    int newRow = currCell[0] + dir[0];
                    int newCol = currCell[1] + dir[1];

                    if (newRow >= 0 && newRow < ROW && newCol >= 0 && newCol < COL && 
                        this.map[newRow, newCol] == null)
                    {
                        this.map[newRow, newCol] = new BFSCell(Gridlayout.SPACE, true, currCell[0], currCell[1]);

                        q.Add(new int[] { newRow, newCol });
                        form.DrawCell(newRow, newCol);

                        if (newRow == startEnd[2] && newCol == startEnd[3])
                        {
                            MessageBox.Show("Destination found");
                            // Path found, backtrack to the start
                            BackTrack.BacktrackPath(this.map, startEnd[2], startEnd[3], form);
                            return;
                        }

                        await Task.Delay(1);
                    }
                }

            }
            MessageBox.Show("Destination not Found");
        }
    }

    internal class DijkstraAlg
    {
        private class DijCell : Cell
        {
            public double distance;

            public DijCell(int type = 0, double distance = double.MaxValue, int parent_x = -1, int parent_y = -1)
            {
                this.type = type;
                this.parent_x = parent_x;
                this.parent_y = parent_y;
                this.distance = distance;
            }
        }

        private int ROW;
        private int COL;
        private bool[,] sptSet;
        private DijCell[,] Dist;

        public DijkstraAlg(int row, int col, HashSet<(int, int)> obstacles)
        {
            ROW = row;
            COL = col;

            this.sptSet = new bool[ROW, COL];
            this.Dist = new DijCell[ROW, COL];

            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    this.Dist[i, j] = new DijCell();
                }
            }

            foreach (var item in obstacles)
            {
                this.Dist[item.Item1, item.Item2] = new DijCell(Gridlayout.OBSTACLE);
            }
        }

        private (int, int) MinDistance()
        {
            double min = double.MaxValue;
            (int, int) min_index = (-1, -1);

            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    if (!sptSet[i, j] && Dist[i, j].distance < min)
                    {
                        min = Dist[i, j].distance;
                        min_index = (i, j);
                    }
                }
            }

            return min_index;
        }

        public async Task Dijkstra(int[] startEnd, int obstacles, Gridlayout form)
        {
            int startY = startEnd[0];
            int startX = startEnd[1];
            int endX = startEnd[2];
            int endY = startEnd[3];

            this.Dist[startY, startX] = new DijCell(Gridlayout.START, 0);

            for (int count = 0; count < ROW * COL - obstacles; count++)
            {
                var (uX, uY) = MinDistance();

                if(uX < 0 && uY < 0)
                {
                    MessageBox.Show("Destination not found");
                    return;
                }

                if (uX == -1 && uY == -1) break; // No reachable cell found

                sptSet[uX, uY] = true;
                form.DrawCell(uX, uY, Color.LightGreen);

                int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1 };
                int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1 };
                double[] dist = { 1, 1, 1, 1, Math.Sqrt(2), Math.Sqrt(2), Math.Sqrt(2), Math.Sqrt(2) };

                for (int i = 0; i < 8; i++)
                {
                    int newX = uX + dx[i];
                    int newY = uY + dy[i];

                    if (IsValid(newX, newY) && !sptSet[newX, newY] && Dist[newX, newY].type != Gridlayout.OBSTACLE)
                    {
                        double newDist = Dist[uX, uY].distance + dist[i];
                        if (newDist < Dist[newX, newY].distance)
                        {
                            Dist[newX, newY].distance = newDist;
                            Dist[newX, newY].parent_x = uX;
                            Dist[newX, newY].parent_y = uY;

                            form.DrawCell(newX, newY);

                            if (newX == endX && newY == endY)
                            {
                                MessageBox.Show("Destination found");
                                // Path found, backtrack to the start

                                form.Mgs($"The lenfgth of the shortest path is {BackTrack.BacktrackPath(this.Dist, startEnd[2], startEnd[3], form)}");
                                return;
                            }

                            await Task.Delay(1);
                        }
                    }
                }
            }
        }

        private bool IsValid(int x, int y)
        {
            return x >= 0 && x < ROW && y >= 0 && y < COL;
        }
    }

    class BackTrack
    {
        public static int BacktrackPath(Cell[,] cells, int endRow, int endCol, Gridlayout form)
        {
            int pathLength = 0;
            int currRow = endRow;
            int currCol = endCol;
            form.DrawCell(currRow, currCol, Color.Orange);

            while (cells[currRow, currCol].type != Gridlayout.START)
            {
                int parentRow = cells[currRow, currCol].parent_x;
                int parentCol = cells[currRow, currCol].parent_y;
                form.DrawCell(parentRow, parentCol, Color.Orange);
                currRow = parentRow;
                currCol = parentCol;
                pathLength++;
            }

            return pathLength;
        }
    }
    
}

