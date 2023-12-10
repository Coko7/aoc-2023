namespace aoc_2023.src.day10
{
    public class Day10Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/10

        public override int Day => 10;
        public override int Part => 1;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input");

            MetalPipe startPipe = null;
            var map = new MetalPipe?[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                map[i] = new MetalPipe[lines[i].Length];

                for (int j = 0; j < lines[i].Length; j++)
                {
                    MetalPipe? pipe;
                    char c = lines[i][j];

                    if (c == '.') pipe = null;
                    else
                    {
                        pipe = new MetalPipe(j, i, c);
                        if (c == 'S') startPipe = pipe;
                    }

                    map[i][j] = pipe;
                }
            }

            var loop = FindLoop(startPipe, map);

            UpdateDistances(loop);
            var furthest = loop.MaxBy(p => p.DistanceFromOrigin);

            Console.WriteLine($"Result for part {Part} is {furthest}");
        }

        private static void UpdateDistances(List<MetalPipe> loop)
        {
            for (int i = 1; i < loop.Count - 1; i++)
            {
                int distFromStart = i;
                int distFromEnd = loop.Count - i;

                loop[i].DistanceFromOrigin = Math.Min(distFromStart, distFromEnd);
            }
        }

        private List<MetalPipe> FindLoop(MetalPipe startPipe, MetalPipe?[][] map)
        {
            var surroundingPipes = GetSurroundingPipes(startPipe, map);

            foreach (var pipe in surroundingPipes)
            {
                if (!CanConnect(startPipe, pipe)) continue;

                MetalPipe previous = startPipe;
                MetalPipe current = pipe;
                var chain = new List<MetalPipe> { startPipe, current };

                while (true)
                {
                    var next = FindNext(current, previous, map);

                    if (next == null) break;
                    if (next == startPipe) return chain;

                    chain.Add(next);
                    previous = current;
                    current = next;
                }
            }

            return null;
        }

        // DOES NOT WORK FOR S
        private MetalPipe? FindNext(MetalPipe current, MetalPipe previous, MetalPipe?[][] map)
        {
            var surroundingPipes = GetSurroundingPipes(current, map);
            var candidates = surroundingPipes.Where(pipe => pipe != previous);

            foreach (var candidate in candidates)
            {
                if (CanConnect(current, candidate)) return candidate;
            }

            return null;
        }

        private ICollection<MetalPipe> GetSurroundingPipes(MetalPipe pipe, MetalPipe?[][] map)
        {
            var surrounding = new List<MetalPipe>();
            if (pipe.Position.X - 1 >= 0)
            {
                var other = map[pipe.Position.Y][pipe.Position.X - 1];
                if (other != null) surrounding.Add(other);
            }

            if (pipe.Position.X + 1 < map[0].Length)
            {
                var other = map[pipe.Position.Y][pipe.Position.X + 1];
                if (other != null) surrounding.Add(other);
            }

            if (pipe.Position.Y - 1 >= 0)
            {
                var other = map[pipe.Position.Y - 1][pipe.Position.X];
                if (other != null) surrounding.Add(other);
            }

            if (pipe.Position.Y + 1 < map.Length)
            {
                var other = map[pipe.Position.Y + 1][pipe.Position.X];
                if (other != null) surrounding.Add(other);
            }

            return surrounding;
        }

        private bool CanConnect(MetalPipe a, MetalPipe b)
        {
            var canNorth = new char[] { 'S', '|', 'L', 'J' };
            var canSouth = new char[] { 'S', '|', '7', 'F' };
            var canEast = new char[] { 'S', '-', 'L', 'F' };
            var canWest = new char[] { 'S', '-', 'J', '7' };

            if (a.Position == b.Position) throw new Exception("A pipe cannot connect to itself!");

            if (b.Position.X == a.Position.X) // Same column
            {
                if (b.Position.Y == a.Position.Y - 1) // B north of A
                {
                    return canNorth.Contains(a.Type) && canSouth.Contains(b.Type);
                }
                else if (b.Position.Y == a.Position.Y + 1) // B south of A
                {
                    return canSouth.Contains(a.Type) && canNorth.Contains(b.Type);
                }
                else return false;
            }
            else if (b.Position.Y == a.Position.Y) // Same line
            {
                if (b.Position.X == a.Position.X - 1) // B west of A
                {
                    return canWest.Contains(a.Type) && canEast.Contains(b.Type);
                }
                else if (b.Position.X == a.Position.X + 1) // B east of A
                {
                    return canEast.Contains(a.Type) && canWest.Contains(b.Type);
                }
                else return false;
            }
            else return false;
        }

        private class MetalPipe
        {
            public Pos Position { get; set; }
            public char Type { get; set; }
            public int DistanceFromOrigin { get; set; }

            public MetalPipe(int x, int y, char type)
            {
                Position = new Pos(x, y);
                Type = type;
            }

            public override bool Equals(object? obj)
            {
                MetalPipe other = obj as MetalPipe;
                if (other == null) return false;

                return Type == other.Type && Position == other.Position;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString() => $"{Type} {Position} [{DistanceFromOrigin}]";
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
