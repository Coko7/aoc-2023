namespace aoc_2023.src.day08
{
    public class Day08Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/8

        public override int Day => 8;
        public override int Part => 1;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input", true);

            string instructions = lines[0];

            string startPos = "";
            var map = new Dictionary<string, KeyValuePair<string, string>>();
            for (int i = 2; i < lines.Length; i++)
            {
                var parts = lines[i].Split('=');

                string pos = parts[0].Trim();
                var leftAndRight = parts[1].Split(",");

                string left = leftAndRight[0].Trim();
                string right = leftAndRight[1].Trim();

                if (pos == "AAA") startPos = pos;

                map.Add(pos, new KeyValuePair<string, string>(left, right));
            }

            int moveIt = 0;
            long totalMoves = 0;
            string currentPos = startPos;
            while (currentPos != "ZZZ")
            {
                char nextMove = instructions[moveIt];

                string nextPos;
                if (nextMove == 'L') nextPos = map[currentPos].Key;
                else nextPos = map[currentPos].Value;

                currentPos = nextPos;
                moveIt = (moveIt + 1) % instructions.Length;
                totalMoves++;
            }

            Console.WriteLine($"Result for part {Part} is {totalMoves}");
        }
    }
}
