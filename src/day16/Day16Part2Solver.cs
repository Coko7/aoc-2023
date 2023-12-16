namespace aoc_2023.src.day16
{
    public class Day16Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/16

        public override int Day => 16;
        public override int Part => 2;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input");

            int xMax = lines[0].Length - 1;
            int yMax = lines.Length - 1;

            var startConfigs = new List<KeyValuePair<Position, Direction>>
            {
                new KeyValuePair<Position, Direction>(new Position(0, 0), Direction.Right),
                new KeyValuePair<Position, Direction>(new Position(0, 0), Direction.Down),

                new KeyValuePair<Position, Direction>(new Position(xMax, 0), Direction.Left),
                new KeyValuePair<Position, Direction>(new Position(xMax, 0), Direction.Down),

                new KeyValuePair<Position, Direction>(new Position(0, yMax), Direction.Right),
                new KeyValuePair<Position, Direction>(new Position(0, yMax), Direction.Up),

                new KeyValuePair<Position, Direction>(new Position(xMax, yMax), Direction.Left),
                new KeyValuePair<Position, Direction>(new Position(xMax, yMax), Direction.Up),
            };

            for (int y = 1; y < yMax; y++)
            {
                startConfigs.Add(new KeyValuePair<Position, Direction>(new Position(0, y), Direction.Right));
                startConfigs.Add(new KeyValuePair<Position, Direction>(new Position(xMax, y), Direction.Left));
            }

            for (int x = 1; x < xMax; x++)
            {
                startConfigs.Add(new KeyValuePair<Position, Direction>(new Position(x, 0), Direction.Down));
                startConfigs.Add(new KeyValuePair<Position, Direction>(new Position(x, yMax), Direction.Up));
            }

            int max = 0;
            foreach (var config in startConfigs)
            {
                var pos = config.Key;
                var dir = config.Value;
                var res = RunBeamSimulation(lines, pos, dir);
                if (res > max) max = res;
            }

            Console.WriteLine($"Result for part {Part} is {max}");
        }

        private static int RunBeamSimulation(string[] lines, Position startPos, Direction dir)
        {
            var grid = new Tile[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                grid[i] = new Tile[lines[i].Length];
                for (int j = 0; j < lines[i].Length; j++)
                {
                    grid[i][j] = new Tile
                    {
                        Pos = new Position(j, i),
                        Type = lines[i][j],
                        CurrentPassingBeams = new List<Direction>(),
                        PastPassingBeams = new List<Direction>()
                    };
                }
            }

            grid[startPos.Y][startPos.X].CurrentPassingBeams.Add(dir);
            while (AnyActiveBeam(grid))
            {
                for (int i = 0; i < grid.Length; i++)
                {
                    for (int j = 0; j < grid[i].Length; j++)
                    {
                        var tile = grid[i][j];
                        if (!tile.CurrentPassingBeams.Any()) continue;

                        foreach (var passingBeam in tile.CurrentPassingBeams)
                        {
                            if (tile.Type == '|')
                            {
                                if (passingBeam == Direction.Up || passingBeam == Direction.Down)
                                {
                                    var nextTile = tile.GetNextTile(grid, passingBeam);
                                    nextTile?.TryAddBeam(passingBeam);
                                }
                                else
                                {
                                    var beamA = Direction.Up;
                                    var beamB = Direction.Down;

                                    var nextTileA = tile.GetNextTile(grid, beamA);
                                    nextTileA?.TryAddBeam(beamA);

                                    var nextTileB = tile.GetNextTile(grid, beamB);
                                    nextTileB?.TryAddBeam(beamB);
                                }
                            }
                            else if (tile.Type == '-')
                            {
                                if (passingBeam == Direction.Left || passingBeam == Direction.Right)
                                {
                                    var nextTile = tile.GetNextTile(grid, passingBeam);
                                    nextTile?.TryAddBeam(passingBeam);
                                }
                                else
                                {
                                    var beamA = Direction.Left;
                                    var beamB = Direction.Right;

                                    var nextTileA = tile.GetNextTile(grid, beamA);
                                    nextTileA?.TryAddBeam(beamA);

                                    var nextTileB = tile.GetNextTile(grid, beamB);
                                    nextTileB?.TryAddBeam(beamB);
                                }
                            }
                            else // '.' or '/' or '\'
                            {
                                var transfoBeam = tile.Transform(passingBeam);

                                var nextTile = tile.GetNextTile(grid, transfoBeam);
                                nextTile?.TryAddBeam(transfoBeam);
                            }
                        }

                        foreach (var beam in tile.CurrentPassingBeams) tile.PastPassingBeams.Add(beam);
                        tile.CurrentPassingBeams = new List<Direction>();
                    }
                }
            }

            return CountEnergy(grid);
        }

        private static int CountEnergy(Tile[][] grid)
        {
            int energy = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j].IsEnergized) energy++;
                }
            }

            return energy;
        }

        private static bool AnyActiveBeam(Tile[][] grid)
        {
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j].CurrentPassingBeams.Any()) return true;
                }
            }

            return false;
        }

        private static void DisplayGrid(Tile[][] grid)
        {
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    var tile = grid[i][j];
                    if (tile.CurrentPassingBeams.Any())
                    {
                        if (tile.CurrentPassingBeams.Count == 1)
                        {
                            var beam = tile.CurrentPassingBeams.First();
                            Console.Write(DirectionToString(beam));
                        }
                        else Console.Write(tile.CurrentPassingBeams.Count);
                    }
                    else if (tile.IsEnergized) Console.Write('#');
                    else Console.Write(tile.Type);
                }
                Console.WriteLine();
            }
        }

        private static char DirectionToString(Direction dir)
        {
            return dir switch
            {
                Direction.Left => '<',
                Direction.Right => '>',
                Direction.Up => '^',
                Direction.Down => 'v',
                _ => throw new Exception("Unsupported direction")
            };
        }

        private class Tile
        {
            public Position Pos { get; set; }
            public char Type { get; set; }
            public ICollection<Direction> CurrentPassingBeams { get; set; } = new List<Direction>();
            public ICollection<Direction> PastPassingBeams { get; set; } = new List<Direction>();

            public bool IsEnergized => PastPassingBeams.Any();

            public void TryAddBeam(Direction beam)
            {
                // We check past passing beams because there is no point adding a beam similar to a past beam.
                // Since the grid does not change, a beam with the same direction as a past beam will just have the same behaviour and end up at the same place.
                if (!PastPassingBeams.Contains(beam) && !CurrentPassingBeams.Contains(beam))
                {
                    CurrentPassingBeams.Add(beam);
                }
            }

            public Direction Transform(Direction beam)
            {
                Direction niu = beam;

                if (Type == '\\')
                {
                    if (beam == Direction.Right) niu = Direction.Down;
                    else if (beam == Direction.Left) niu = Direction.Up;
                    else if (beam == Direction.Up) niu = Direction.Left;
                    else if (beam == Direction.Down) niu = Direction.Right;
                }
                else if (Type == '/')
                {
                    if (beam == Direction.Right) niu = Direction.Up;
                    else if (beam == Direction.Left) niu = Direction.Down;
                    else if (beam == Direction.Up) niu = Direction.Right;
                    else if (beam == Direction.Down) niu = Direction.Left;
                }

                return niu;
            }

            public Tile? GetNextTile(Tile[][] grid, Direction beam)
            {
                Position nextPos = Pos;
                if (beam == Direction.Left)
                {
                    nextPos = new Position(Pos.X - 1, Pos.Y);
                    if (nextPos.X < 0) return null;
                }
                else if (beam == Direction.Right)
                {
                    nextPos = new Position(Pos.X + 1, Pos.Y);
                    if (nextPos.X >= grid[0].Length) return null;
                }
                else if (beam == Direction.Up)
                {
                    nextPos = new Position(Pos.X, Pos.Y - 1);
                    if (nextPos.Y < 0) return null;
                }
                else if (beam == Direction.Down)
                {
                    nextPos = new Position(Pos.X, Pos.Y + 1);
                    if (nextPos.Y >= grid.Length) return null;
                }

                return grid[nextPos.Y][nextPos.X];
            }
        }

        private enum Direction { Left, Right, Up, Down }

        private class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override bool Equals(object? obj)
            {
                var other = (Position)obj;
                if (other == null) return false;

                return X == other.X && Y == other.Y;
            }
        }
    }
}
