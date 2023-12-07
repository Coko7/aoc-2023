namespace aoc_2023.src.day06
{
    public class Day06Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/6

        public override int Day => 6;
        public override int Part => 1;

        public override void Solve()
        {
            // Input was formatted to replace empty spaces ' ' with ','
            // Example:
            // Time:      7  15   30
            // Time:,7,15,30
            // In vim, you can do so by highlighting the text and running the following command: :%s/ \+/ /g
            string[] lines = ReadInputLines("input");

            int[] times = lines[0].Split(':')[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(part => int.Parse(part)).ToArray();
            int[] distances = lines[1].Split(':')[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(part => int.Parse(part)).ToArray();

            int[] waysToBeat = new int[times.Length];
            for (int i = 0; i < times.Length; i++)
            {
                int record = distances[i];
                int maxMs = times[i];
                int ways = 0;
                for (int j = 0; j <= maxMs; j++)
                {
                    int startSpeed = j;
                    int dist = (maxMs - j) * startSpeed;
                    if (dist > record)
                    {
                        ways++;
                    }
                }
                waysToBeat[i] = ways;

                Console.WriteLine($"Ways for race to beat record {record}: {ways}");
            }

            int prod = waysToBeat.Aggregate((x, y) => x * y);
            Console.WriteLine($"Result is {prod}");
        }
    }
}
