namespace aoc_2023.src.day11
{
    public class Day11Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/11

        public override int Day => 11;
        public override int Part => 2;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input");

            var galaxies = new List<Galaxy>(); 
            var linesToExpand = new List<int>();
            var columnsToExpand = new List<int>();

            // Find lines to expand and register galaxies
            for (int i = 0; i < lines.Length; i++)
            {
                bool expandLine = true;
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                    {
                        expandLine = false;
                        galaxies.Add(new Galaxy(galaxies.Count, j, i));
                    }
                }

                if (expandLine) linesToExpand.Add(i);
            }

            // Find columns to expand
            for (int i = 0; i < lines[0].Length; i++)
            {
                bool expandCol = true;
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[j][i] == '#')
                    {
                        expandCol = false;
                        break;
                    } 
                }

                if (expandCol) columnsToExpand.Add(i);
            }

            var res = ComputeShortestPathsSum(galaxies, linesToExpand, columnsToExpand);

            Console.WriteLine($"Result for part {Part} is {res}");
        }

        private static long ComputeShortestPathsSum(List<Galaxy> galaxies, List<int> linesToExp, List<int> colsToExp)
        {
            long sum = 0;
            foreach (var galaxy in galaxies) galaxy.DistanceToNeighbors = new long?[galaxies.Count];

            for (int i = 0; i < galaxies.Count; i++)
            {
                var current = galaxies[i];
                for (int j = 0; j < galaxies.Count; j++)
                {
                    if (i == j) continue; // Don't compare galaxy with itself

                    var other = galaxies[j];

                    if (other.DistanceToNeighbors[i] != null) current.DistanceToNeighbors[j] = other.DistanceToNeighbors[i];
                    else
                    {
                        var dist = current.GetDistanceTo(other, linesToExp, colsToExp);
                        current.DistanceToNeighbors[j] = dist;
                        sum += dist;
                    }
                }
            }

            return sum;
        }

        private class Galaxy
        {
            public long Id { get; set; }
            public long X { get; set; }
            public long Y { get; set; }
            public long?[] DistanceToNeighbors { get; set; }

            public Galaxy(long id, long x, long y)
            {
                Id = id;
                X = x;
                Y = y;
            }

            public long GetDistanceTo(Galaxy other, List<int> linesToExpand, List<int> columnsToExpand)
            {
                int expandRate = 999999; // 1000000 - 1 because otherwise we would count one too much
                long realX = X + columnsToExpand.Where(col => col < X).Count() * expandRate;
                long realY = Y + linesToExpand.Where(line => line < Y).Count() * expandRate;

                long realOtherX = other.X + columnsToExpand.Where(col => col < other.X).Count() * expandRate;
                long realOtherY = other.Y + linesToExpand.Where(line => line < other.Y).Count() * expandRate;

                return Math.Abs(realOtherX - realX) + Math.Abs(realOtherY - realY);
                //return (long)Math.Truncate(Math.Sqrt(Math.Pow(realOtherX - realX, 2) + Math.Pow(realOtherY - realY, 2)));
            }
        }
    }
}
