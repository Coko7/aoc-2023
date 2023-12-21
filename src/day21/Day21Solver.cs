namespace aoc_2023.src.day21
{
    public class Day21Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/21

        public override int Day => 21;
        public override int Part => 1;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input");

            Pos start = null;
            var map = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                map[i] = new char[lines[i].Length];
                for (int j = 0; j < lines[i].Length; j++)
                {
                    char c = lines[i][j];

                    if (c == 'S') start = new Pos(j, i);
                    map[i][j] = c;
                }
            }

            var spots = new List<Pos> { start };
            for (int i = 0; i < 64; i++)
            {
                spots = GetReachableTilesMulti(spots, map).ToList();
                //Console.WriteLine($"Spots: {spots.Count}");
                //DisplayMapWithSpots(map, spots);
            }

            DisplayMapWithSpots(map, spots);

            Console.WriteLine($"Result for part {Part} is {spots.Count}");
        }

        private static void DisplayMapWithSpots(char[][] map, ICollection<Pos> spots)
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    var matchingSpot = spots.FirstOrDefault(pos => pos.X == j && pos.Y == i);
                    if (matchingSpot != null) Console.Write("0");
                    else
                    {
                        char c = map[i][j];
                        Console.Write(c);
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static ICollection<Pos> GetReachableTiles(Pos current, char[][] map)
        {
            var xMax = map[0].Length - 1;
            var yMax = map.Length - 1;

            var x = current.X;
            var y = current.Y;

            var reachable = new List<Pos>();
            if (x > 0 && map[y][x - 1] != '#') reachable.Add(new Pos(x - 1, y));
            if (x < xMax && map[y][x + 1] != '#') reachable.Add(new Pos(x + 1, y));

            if (y > 0 && map[y - 1][x] != '#') reachable.Add(new Pos(x, y - 1));
            if (y < yMax && map[y + 1][x] != '#') reachable.Add(new Pos(x, y + 1));

            return reachable;
        }

        private static ICollection<Pos> GetReachableTilesMulti(ICollection<Pos> currents, char[][] map)
        {
            var allSpots = new List<Pos>();
            foreach (var current in currents)
            {
                var spots = GetReachableTiles(current, map);
                allSpots.AddRange(spots);
            }

            return allSpots.Distinct().ToList();
        }

        private class Pos
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Pos(int x, int y) { X = x; Y = y; }

            public override bool Equals(object? obj)
            {
                Pos other = obj as Pos;
                if (other == null) return false;

                return X == other.X && Y == other.Y;
            }

            public override int GetHashCode() => (X << 2) ^ Y;

            public override string ToString() => $"{X}x {Y}y";
        }
    }
}
