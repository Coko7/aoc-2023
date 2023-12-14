namespace aoc_2023.src.day14
{
    public class Day14Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/14

        public override int Day => 14;
        public override int Part => 1;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input");
            var platform = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                platform[i] = lines[i].ToCharArray();
            }

            DisplayPlatform(platform);
            TiltPlatform(platform);
            Console.WriteLine();
            DisplayPlatform(platform);

            var load = CalculateLoad(platform);
            Console.WriteLine($"Result for part {Part} is {load}");
        }

        private static void DisplayPlatform(char[][] platform)
        {
            for (int i = 0; i < platform.Length; i++)
            {
                for (int j = 0; j < platform[i].Length; j++) Console.Write(platform[i][j]);
                Console.WriteLine();
            }
        }
        
        private static int CalculateLoad(char[][] platform)
        {
            int load = 0;
            for (int i = 0; i < platform.Length; i++)
            {
                for (int j = 0; j < platform[i].Length; j++)
                {
                    if (platform[i][j] == 'O') load += platform.Length - i;
                }
            }

            return load;
        }

        private static void TiltPlatform(char[][] platform)
        {
            for (int i = 1; i < platform.Length; i++)
            {
                for (int j = 0; j < platform[i].Length; j++)
                {
                    var item = platform[i][j];
                    if (item == 'O')
                    {
                        int yPos = i;
                        while (yPos > 0 && platform[yPos - 1][j] == '.') yPos--;
                        platform[i][j] = '.';
                        platform[yPos][j] = 'O';
                    }
                }
            }
        }
    }
}
