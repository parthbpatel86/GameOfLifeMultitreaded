using System;
using System.Collections.Concurrent;

namespace GameOfLife
{
    public class GameOfLifeSystem
    {
        // states of grid
        private Grid[] _grids;
        private int _currentGridStateIdx = 0;
        private int _previousStateIdx = 0;
        private const int TotalGridState = 2;

        // Multithreading variables for debug
        private int _maxThreadUsed = 0;
        public int GetMaxThreadUsed () { return _maxThreadUsed; }

        public GameOfLifeSystem(int rows, int columns)
        {
            _currentGridStateIdx = 0;
            _previousStateIdx = 1;
            _grids = new Grid[TotalGridState];
            for (int i = 0; i < TotalGridState; i++)
            {
                _grids[i] = new Grid(rows, columns);
            }
        }

        public void Initialize(int startRow, int startCol, bool[,] aliveInitialPattern)
        {
            _grids[_currentGridStateIdx].SetInitialPattern(startRow, startCol, aliveInitialPattern);  // Set glider pattern at the center of the grid
        }

        public void Tick(int generations, bool multithreading, bool displayMode = false)
        {
            for (int i = 1; i <= generations; i++)
            {
                _previousStateIdx = _currentGridStateIdx;
                _currentGridStateIdx = i % 2;

                if (displayMode)
                {
                    Console.Clear();
                    _grids[_previousStateIdx].PrintGrid();
                }

                if (multithreading)
                {
                    var threadUsed = _grids[_currentGridStateIdx].NextGenerationParallel(_grids[_previousStateIdx]);
                    if (threadUsed > _maxThreadUsed)
                    {
                        _maxThreadUsed = threadUsed;
                    }
                }
                else
                {
                    _grids[_currentGridStateIdx].NextGeneration(_grids[_previousStateIdx]);
                }

                if (displayMode)
                    Thread.Sleep(100);  // Pause for a bit
            }
        }
        public Grid GetCurrentGrid()
        {
            return _grids[_currentGridStateIdx];
        }
    }
}