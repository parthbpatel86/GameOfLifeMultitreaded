namespace GameOfLifeTests;

using Xunit;
using GameOfLife;

public class GameOfLifeTests
{
    [Fact]
    public void Initialize_SetsCorrectPatternInGrid()
    {
        // Arrange
        int rows = 5;
        int columns = 5;
        GameOfLifeSystem game = new GameOfLifeSystem(rows, columns);

        bool[,] glider = new bool[,]
        {
            { false, true, false },
            { false, false, true },
            { true, true, true }
        };

        // Act
        game.Initialize(1, 1, glider);  // Place the glider at position (1, 1) in the grid

        // Assert
        Grid grid = game.GetCurrentGrid();
        Assert.True(grid.GetCell(1, 2).IsAlive);
        Assert.True(grid.GetCell(2, 3).IsAlive);
        Assert.True(grid.GetCell(3, 1).IsAlive);
        Assert.True(grid.GetCell(3, 2).IsAlive);
        Assert.True(grid.GetCell(3, 3).IsAlive);
    }

    [Fact]
    public void NextGeneration_ProducesExpectedGridTest()
    {
        // Arrange
        int rows = 5;
        int columns = 5;
        GameOfLifeSystem game = new GameOfLifeSystem(rows, columns);

        bool[,] glider = new bool[,]
        {
            { false, true, false },
            { false, false, true },
            { true, true, true }
        };

        game.Initialize(1, 1, glider);

        // Act
        game.Tick(1, multithreading: true, displayMode: false); // Run 1 generation

        // Assert
        Grid grid = game.GetCurrentGrid();
        Assert.False(grid.GetCell(1, 2).IsAlive);
        Assert.True(grid.GetCell(2, 1).IsAlive);
        Assert.True(grid.GetCell(2, 3).IsAlive);
        Assert.True(grid.GetCell(3, 2).IsAlive);
        Assert.True(grid.GetCell(3, 3).IsAlive);
    }
}
