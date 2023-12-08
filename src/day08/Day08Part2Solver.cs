using aoc_2023.src.Utils;

namespace aoc_2023.src.day08
{
    public class Day08Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/8

        public override int Day => 8;
        public override int Part => 2;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input2", true);

            string instructions = lines[0];

            var startPositions = new List<string>();
            var map = new string[lines.Length - 2][];
            for (int i = 2; i < lines.Length; i++)
            {
                var parts = lines[i].Split('=');

                string pos = parts[0].Trim();
                var leftAndRight = parts[1].Split(",");

                string left = leftAndRight[0].Trim();
                string right = leftAndRight[1].Trim();

                if (pos.EndsWith('A')) startPositions.Add(pos);

                map[i - 2] = new string[] { pos, left, right };
            }

            Console.WriteLine("Map parsed");

            int moveIt = 0;
            int totalMoves = 0;
            bool allZFound = false;

            var currentPositions = new string[startPositions.Count];
            for (int i = 0; i < startPositions.Count; i++) currentPositions[i] = startPositions[i];

            var movesToZ = new long[startPositions.Count];
            for (int i = 0; i < startPositions.Count; i++) movesToZ[i] = 0;

            Console.WriteLine("Starting all ghosts");

            // Loop until all paths have reached 'Z' once (they don't need to reach it at the same time)
            while (!allZFound)
            {
                char nextMove = instructions[moveIt];
                int nextPosI = nextMove == 'L' ? 1 : 2;

                for (int i = 0; i < currentPositions.Length; i++)
                {
                    string curPos = currentPositions[i];
                    var curNode = FindNode(curPos, map)!;
                    string nextPos = curNode[nextPosI];

                    currentPositions[i] = nextPos;

                    // Make note of how many moves were needed to reach 'Z'
                    if (nextPos[^1] == 'Z') movesToZ[i] = totalMoves + 1; // I do +1 here because the totalMoves are updated later in the loop
                }

                allZFound = true;

                // Check if all paths have reached 'Z' at least once
                for (int i = 0; i < movesToZ.Length; i++) allZFound &= movesToZ[i] != 0;

                moveIt = (moveIt + 1) % instructions.Length;
                totalMoves++;
            }

            long lcm = MathUtils.Lcm(movesToZ);
            Console.WriteLine($"Result for part {Part} is {lcm}");
        }

        private static string[]? FindNode(string node, string[][] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i][0] == node) return nodes[i];
            }

            return null;
        }
    }
}
