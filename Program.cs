using aoc_2023.src;
using aoc_2023.src.day05;

namespace aoc_2023
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Setup();
            SolveDailyPuzzle();
            //SolveSpecificPuzzle(1);
        }

        private static void SolveDailyPuzzle(bool isPart2 = false)
        {
            var now = DateTime.Now;
            int month = now.Month;
            if (month != 12) throw new Exception($"Nothing to solve yet... Come back in December {now.Year}!");

            int day = now.Day;
            if (day > 25) throw new Exception($"No more puzzle for this year's AOC! Come back in {now.Year + 1}");

            // Execute the code to solve today's puzzle

            var dailySolver = CommonUtils.InstantiateSolver(day + 1, isPart2);
            dailySolver.Solve();
        }

        private static void SolveSpecificPuzzle(int day, bool isPart2 = false)
        {
            var solver = CommonUtils.InstantiateSolver(day, isPart2);
            solver.Solve();
        }

        private static void Setup()
        {
            Console.WriteLine("You are about to setup the workspace for the AOC 2023. Are you sure you want to continue? (N/y)");
            string? input = Console.ReadLine();
            string inputLower = string.IsNullOrEmpty(input) ? "" : input.ToLower();

            if (inputLower == "y" || inputLower == "yes")
            {
                Console.WriteLine("Starting AOC 2023 workspace setup...");
                CommonUtils.SetupAOCWorkspace();
            }

            Console.WriteLine("Aborting...");
            Console.WriteLine("Workspace setup was cancelled");
        }
    }
}