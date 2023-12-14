namespace aoc_2023.src.day14
{
    public class Day14Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/14

        public override int Day => 14;
        public override int Part => 2;

        public override void Solve()
        {
            string[] lines = ReadInputLines("example");
            var platform = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                platform[i] = lines[i].ToCharArray();
            }

            DisplayPlatform(platform);

            var maxCycles = 1000000000;
            var loadCycles = new Dictionary<int, List<long>>();
            var loadFreq = new Dictionary<int, long>();

            for (long i = 1; i <= maxCycles; i++)
            {
                TiltPlatformNorth(platform);
                TiltPlatformWest(platform);
                TiltPlatformSouth(platform);
                TiltPlatformEast(platform);

                var load = CalculateLoad(platform);
                if (loadCycles.ContainsKey(load))
                {
                    var lastCycle = loadCycles[load].Last();
                    var diff = i - lastCycle;

                    loadCycles[load].Add(i);
                    loadFreq[load] = Math.Max(loadFreq[load], diff);
                }
                else
                {
                    loadCycles.Add(load, new List<long> { i });
                    loadFreq.Add(load, -1);
                }

                var mods = loadFreq.Values.Where(v => v > 0);
                if (!mods.Any()) continue;

                // Kinda convoluted
                // It works with my input but I am not sure if it will for alternate inputs
                var maxMod = mods
                    .GroupBy(m => m)
                    .ToDictionary(gb => gb.Key, gb => gb.Count())
                    .OrderByDescending(md => md.Value)
                    .First().Key;

                if (maxCycles % maxMod == i % maxMod)
                {
                    Console.WriteLine($"{maxMod}: Load is {load}");
                }
            }

            DisplayPlatform(platform);

            var loadFinal = CalculateLoad(platform);
            Console.WriteLine($"Result for part {Part} is {loadFinal}");
        }

        private static void DisplayPlatform(char[][] platform)
        {
            for (int i = 0; i < platform.Length; i++)
            {
                for (int j = 0; j < platform[i].Length; j++) Console.Write(platform[i][j]);
                Console.WriteLine();
            }
            Console.WriteLine();
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

        private static void TiltPlatformNorth(char[][] platform)
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

        private static void TiltPlatformSouth(char[][] platform)
        {
            for (int i = platform.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < platform[i].Length; j++)
                {
                    var item = platform[i][j];
                    if (item == 'O')
                    {
                        int yPos = i;
                        while (yPos < platform.Length - 1 && platform[yPos + 1][j] == '.') yPos++;
                        platform[i][j] = '.';
                        platform[yPos][j] = 'O';
                    }
                }
            }
        }

        private static void TiltPlatformWest(char[][] platform)
        {
            for (int i = 1; i < platform[0].Length; i++)
            {
                for (int j = 0; j < platform.Length; j++)
                {
                    var item = platform[j][i];
                    if (item == 'O')
                    {
                        int xPos = i;
                        while (xPos > 0 && platform[j][xPos - 1] == '.') xPos--;
                        platform[j][i] = '.';
                        platform[j][xPos] = 'O';
                    }
                }
            }
        }

        private static void TiltPlatformEast(char[][] platform)
        {
            for (int i = platform[0].Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < platform.Length; j++)
                {
                    var item = platform[j][i];
                    if (item == 'O')
                    {
                        int xPos = i;
                        while (xPos < platform[0].Length - 1 && platform[j][xPos + 1] == '.') xPos++;
                        platform[j][i] = '.';
                        platform[j][xPos] = 'O';
                    }
                }
            }
        }
    }
}
