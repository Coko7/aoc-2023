namespace aoc_2023.src.day11
{
    public class Day11Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/11

        public override int Day => 11;
        public override int Part => 1;

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

        private static int ComputeShortestPathsSum(List<Galaxy> galaxies, List<int> linesToExp, List<int> colsToExp)
        {
            int sum = 0;
            foreach (var galaxy in galaxies) galaxy.DistanceToNeighbors = new int?[galaxies.Count];

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
            public int Id { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int?[] DistanceToNeighbors { get; set; }

            public Galaxy(int id, int x, int y)
            {
                Id = id;
                X = x;
                Y = y;
            }

            public int GetDistanceTo(Galaxy other, List<int> linesToExpand, List<int> columnsToExpand)
            {
                int realX = X + columnsToExpand.Where(col => col < X).Count();
                int realY = Y + linesToExpand.Where(line => line < Y).Count();

                int realOtherX = other.X + columnsToExpand.Where(col => col < other.X).Count();
                int realOtherY = other.Y + linesToExpand.Where(line => line < other.Y).Count();

                return Math.Abs(realOtherX - realX) + Math.Abs(realOtherY - realY);
                //return (int)Math.Truncate(Math.Sqrt(Math.Pow(realOtherX - realX, 2) + Math.Pow(realOtherY - realY, 2)));
            }
        }
    }
}
