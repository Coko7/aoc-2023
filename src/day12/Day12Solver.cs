namespace aoc_2023.src.day12
{
    public class Day12Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/12

        public override int Day => 12;
        public override int Part => 1;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input");
            int sum = 0;
            foreach (var line in lines)
            {
                int arrangements = CountArrangements(line);
                sum += arrangements;
            }

            Console.WriteLine($"Result for part {Part} is {sum}");
        }

        private static int CountArrangements(string line)
        {
            var parts = line.Split(' ');
            string gears = parts[0];
            int[] brokenGroups = parts[1].Split(',').Select(num => int.Parse(num)).ToArray();

            int validCount = 0;
            var allArrangements = ComputeAllArrangements(gears);
            foreach (var arr in allArrangements)
            {
                if (VerifyArrangement(arr, brokenGroups)) validCount++;
            }

            Console.WriteLine($"{line} => {validCount}");
            return validCount;
        }

        private static ICollection<string> ComputeAllArrangements(string line)
        {
            return ComputeArrangementsInner(line, 0);
        }

        private static ICollection<string> ComputeArrangementsInner(string line, int i)
        {
            if (i == line.Length) return new List<string> { line };

            var list = new List<string>();
            if (line[i] == '?')
            {
                string pre = line[..i];
                string aft = line[(i + 1)..];
                list.AddRange(ComputeArrangementsInner(pre + '#' + aft, i + 1));
                list.AddRange(ComputeArrangementsInner(pre + '.' + aft, i + 1));
            }
            else
            {
                list.AddRange(ComputeArrangementsInner(line, i + 1));
            }

            return list;
        }

        private static bool VerifyArrangement(string arrangement, int[] groupSizes)
        {
            var groups = arrangement.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (groups.Length != groupSizes.Length) return false;
            for (int i = 0; i < groups.Length; i++)
            {
                var group = groups[i];
                if (group.Length != groupSizes[i]) return false;
                if (!group.All(gear => gear == '#')) return false;
            }

            return true;
        }
    }
}
