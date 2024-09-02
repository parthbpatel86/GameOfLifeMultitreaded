using System.Collections.Concurrent;
using System;

namespace GameOfLife
{
    // Represents the grid of cells
    public class Grid
    {
        public int Rows { get; }
        public int Columns { get; }
        private Cell[,] cells;

        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            cells = new Cell[rows, columns];
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    cells[i, j] = new Cell(false);
                }
            }
        }

        public void SetInitialPattern(int startRow, int startColumn, bool[,] pattern)
        {
            for (int i = 0; i < pattern.GetLength(0); i++)
            {
                for (int j = 0; j < pattern.GetLength(1); j++)
                {
                    cells[startRow + i, startColumn + j].IsAlive = pattern[i, j];
                }
            }
        }

        public void NextGeneration(Grid prevStateGrid)
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    UpdateGrid(prevStateGrid, r, c);
                }
            }
        }

        public int NextGenerationParallel(Grid prevStateGrid)
        {
            // ConcurrentDictionary to track the unique thread IDs
            ConcurrentDictionary<int, bool> threadIds = new ConcurrentDictionary<int, bool>();

            Parallel.For(0, Rows, r =>
            {
                Parallel.For(0, Columns, c =>
                {
                    // Track the thread ID
                    threadIds.TryAdd(Thread.CurrentThread.ManagedThreadId, true);

                    UpdateGrid(prevStateGrid, r, c);
                });
            });

            return threadIds.Count;
        }

        private void UpdateGrid(Grid prevStateGrid, int r, int c)
        {
            int liveNeighbors = CountLiveNeighbors(r, c, prevStateGrid);
            bool isAlive = prevStateGrid.GetCell(r, c).IsAlive;

            if (isAlive && (liveNeighbors < 2 || liveNeighbors > 3))
            {
                cells[r, c].IsAlive = false;
            }
            else if (!isAlive && liveNeighbors == 3)
            {
                cells[r, c].IsAlive = true;
            }
            else
            {
                cells[r, c].IsAlive = isAlive;
            }
        }

        public Cell GetCell(int row, int col)
        {
            return cells[row, col];
        }

        public void SetGrid(Cell[,] newGrid)
        {
            cells = newGrid;
        }

        public int CountLiveNeighbors(int row, int col, Grid prevStateGrid)
        {
            int liveCount = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // the element whoes neighbors we are counting. Dont count that.
                    if (i == 0 && j == 0) continue;

                    int r = row + i;
                    int c = col + j;

                    if (r >= 0 && r < Rows && c >= 0 && c < Columns && prevStateGrid.GetCell(r, c).IsAlive)
                    {
                        liveCount++;
                    }
                }
            }

            return liveCount;
        }

        public void PrintGrid()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write(cells[i, j].IsAlive ? "O" : ".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}