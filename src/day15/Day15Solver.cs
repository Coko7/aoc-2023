namespace aoc_2023.src.day15
{
    public class Day15Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/15

        public override int Day => 15;
        public override int Part => 1;

        public override void Solve()
        {
            string input = ReadInput("input");
            var sequences = input.Split(',');
            long sum = 0;

            for (int i = 0; i < sequences.Length; i++)
            {
                long val = 0;
                for (int j = 0; j < sequences[i].Length; j++)
                {
                    char c = sequences[i][j];
                    if (c == '\n') continue;
                    val += c;
                    val *= 17;
                    val %= 256;
                }
                sum += val;
            }

            Console.WriteLine($"Result for part {Part} is {sum}");
        }
    }
}
