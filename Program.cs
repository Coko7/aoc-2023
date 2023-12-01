using aoc_2023.src;

namespace aoc_2023
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Setup();
            SolveDailyPuzzle();
        }

        private static void SolveDailyPuzzle()
        {
            var now = DateTime.Now;
            int month = now.Month;
            if (month != 12) throw new Exception($"Nothing to solve yet... Come back in December {now.Year}!");

            int day = now.Day;
            if (day > 25) throw new Exception($"No more puzzle for this year's AOC! Come back in {now.Year + 1}");

            var dailySolver = CommonUtils.InstantiateSolver(day);

            // Execute the code to solve today's puzzle

            dailySolver.Solve();
            //dailySolver.SolvePart2();
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