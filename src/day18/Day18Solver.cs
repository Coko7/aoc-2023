namespace aoc_2023.src.day18
{
    public class Day18Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/18

        public override int Day => 18;
        public override int Part => 1;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input");

            int x = 0;
            int y = 0;

            int xMax = 0;
            int yMax = 0;

            int xMin = 0;
            int yMin = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(' ');
                string dir = parts[0];
                int size = int.Parse(parts[1]);

                if (dir == "R") x += size;
                if (dir == "L") x -= size;
                if (dir == "U") y -= size;
                if (dir == "D") y += size;

                if (x > xMax) xMax = x;
                if (y > yMax) yMax = y;

                if (x < xMin) xMin = x;
                if (y < yMin) yMin = y;
            }

            int padding = 2; // We need padding otherwise spreading technique will not work properly

            int width = xMax + Math.Abs(xMin) + padding;
            int height = yMax + Math.Abs(yMin) + padding;

            var trench = new char[height + 1][];
            for (int i = 0; i < trench.Length; i++)
            {
                trench[i] = new char[width + 1];
                for (int j = 0;  j < trench[i].Length; j++)
                {
                    trench[i][j] = '.';
                }
            }

            x = Math.Abs(xMin) + padding / 2;
            y = Math.Abs(yMin) + padding / 2;
            trench[y][x] = '#';

            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(' ');
                string dir = parts[0];
                int size = int.Parse(parts[1]);
                string hexColor = parts[2][2..^1];

                if (dir == "R") for (int j = 0; j < size; j++) trench[y][++x] = '#';
                if (dir == "L") for (int j = 0; j < size; j++) trench[y][--x] = '#';
                if (dir == "U") for (int j = 0; j < size; j++) trench[--y][x] = '#';
                if (dir == "D") for (int j = 0; j < size; j++) trench[++y][x] = '#';
            }

            SpreadFromOutside(trench);

            int lavaCapacity = 0;
            for (int i = 0; i < trench.Length; i++)
            {
                for (int j = 0; j < trench[i].Length; j++)
                {
                    var tile = trench[i][j];
                    if (tile == '#' || tile == '.') lavaCapacity++;
                    Console.Write($"{tile}");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Result for part {Part} is {lavaCapacity}");
        }

        private static void SpreadFromOutside(char[][] grid)
        {
            grid[0][0] = '0';
            SpreadFromTopLeft(grid);
            SpreadFromTopRight(grid);
            SpreadFromBottomRight(grid);
            SpreadFromBottomLeft(grid);
        }

        private static void ConvertTile(char[][] grid, int x, int y)
        {
            if (grid[y][x] == '#') return;

            if (x > 0 && grid[y][x - 1] == '0') grid[y][x] = '0';
            if (x < grid[0].Length - 1 && grid[y][x + 1] == '0') grid[y][x] = '0';
            if (y > 0 && grid[y - 1][x] == '0') grid[y][x] = '0';
            if (y < grid.Length - 1 && grid[y + 1][x] == '0') grid[y][x] = '0';
        }

        private static void SpreadFromTopLeft(char[][] grid)
        {
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    ConvertTile(grid, x, y);
                }
            }
        }
        private static void SpreadFromTopRight(char[][] grid)
        {
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = grid[y].Length - 1; x >= 0; x--)
                {
                    ConvertTile(grid, x, y);
                }
            }
        }

        private static void SpreadFromBottomLeft(char[][] grid)
        {
            for (int y = grid.Length - 1; y >= 0; y--)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    ConvertTile(grid, x, y);
                }
            }
        }

        private static void SpreadFromBottomRight(char[][] grid)
        {
            for (int y = grid.Length - 1; y >= 0; y--)
            {
                for (int x = grid[y].Length - 1; x >= 0; x--)
                {
                    ConvertTile(grid, x, y);
                }
            }
        }
    }
}
