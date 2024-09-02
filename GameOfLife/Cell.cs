namespace GameOfLife
{
    // Represents a single cell in the grid
    public struct Cell
    {
        public bool IsAlive { get; set; }

        public Cell(bool isAlive)
        {
            IsAlive = isAlive;
        }
    }
}