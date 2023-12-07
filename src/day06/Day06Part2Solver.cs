namespace aoc_2023.src.day06
{
    public class Day06Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/6

        public override int Day => 6;
        public override int Part => 2;

        public override void Solve()
        {
            // Input was formatted to replace empty spaces ' ' with ''
            // Example:
            // Time:      7  15   30
            // Time:71530
            // In vim, you can do so by highlighting the text and running the following command: :%s/ //g
            string[] lines = ReadInputLines("input");

            long[] times = lines[0].Split(':')[1].Split(",").Select(part => long.Parse(part)).ToArray();
            long[] distances = lines[1].Split(':')[1].Split(",").Select(part => long.Parse(part)).ToArray();

            for (long i = 0; i < times.Length; i++)
            {
                long record = distances[i];
                long maxMs = times[i];
                long ways = 0;
                for (long j = 0; j <= maxMs; j++)
                {
                    long startSpeed = j;
                    long dist = (maxMs - j) * startSpeed;
                    if (dist > record)
                    {
                        ways++;
                    }
                }

                Console.WriteLine($"Result for part 2 is {ways}");
            }
        }
    }
}
