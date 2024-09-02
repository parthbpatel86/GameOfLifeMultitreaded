using System.Diagnostics;

namespace GameOfLife
{
    // Contains the main logic to run the Game of Life
    public class GameOfLifeController
    {
        static void Main(string[] args)
        {
            // Default values for grid size and number of grids
            int rows = 25;
            int columns = 25;
            int generations = 100;
            bool display = true;
            bool multithreading = true;

            // Parse command-line arguments if provided
            if (args.Length >= 2)
            {
                rows = int.Parse(args[0]);
                columns = int.Parse(args[1]);
                if (args.Length >= 3)
                {
                    generations = int.Parse(args[2]);
                }                
                if (args.Length >= 4)
                {
                    display = bool.Parse(args[3]);
                }
                if (args.Length >= 5)
                {
                    multithreading = bool.Parse(args[4]);
                }
            }

            // Start a stopwatch to measure performance
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Capture initial memory usage
            long initialMemory = GC.GetTotalMemory(true);

            // Glider pattern
            GameOfLifeSystem game = new GameOfLifeSystem(rows, columns);
            bool[,] glider = new bool[,]
            {
                { false, true, false },
                { false, false, true },
                { true, true, true }
            };
            float startRowPatternIdx = (rows / 2f) - (glider.GetLength(0) / 2f) - 1;
            float startColPatternIdx = (columns / 2f) - (glider.GetLength(1) / 2f) - 1;

            // Initialize and Run
            game.Initialize((int)startRowPatternIdx, (int)startColPatternIdx, glider);
            game.Tick(generations, multithreading, display);

            // Stop the stopwatch
            stopwatch.Stop();

            // Capture final memory usage
            long finalMemory = GC.GetTotalMemory(true);

            // Calculate performance metrics
            TimeSpan timeElapsed = stopwatch.Elapsed;
            long memoryUsed = finalMemory - initialMemory;

            // Output the results
            Console.WriteLine($"Time Elapsed: {timeElapsed.TotalMilliseconds} ms");
            Console.WriteLine($"Memory Used: {memoryUsed / 1024.0} KB");
            Console.WriteLine($"Max thread used: {game.GetMaxThreadUsed()}");
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: GameOfLife.exe <rows> <columns> <generations> <display> <multithreading>");
                Console.WriteLine($"Using default values: {rows}x{columns} {generations}generations, display={display}, multithreading={multithreading}");
            }

            /* Performance analysis
            //250x250 100 generation
            Time Elapsed: 2010.8948 ms
            Memory Used: 152.953125 KB
            Max thread used: 0

            //Multithreading 250x250 100 generation
            Time Elapsed: 2828.8033 ms
            Memory Used: 172.078125 KB
            Max thread used: 21

            //2500x2500 100 generation
            Time Elapsed: 49,742.7665 ms
            Memory Used: 12,259.421875 KB
            Max thread used: 23
            
            //2500x2500 100 generation
            Time Elapsed: 131,962.7435 ms
            Memory Used: 12,233.2421875 KB
            Max thread used: 0

            //Multithreading with Cell as struct
            Time Elapsed: 52,753.2639 ms
            Memory Used: 12,260.28125 KB
            Max thread used: 24

            //Multithreading with Cell as Class
            Time Elapsed: 45,833.5369 ms
            Memory Used: 390,675.015625 KB
            Max thread used: 19
            */
        }
    }
}