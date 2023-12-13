using System.Text;

namespace aoc_2023.src.day13
{
    public class Day13Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/13

        public override int Day => 13;
        public override int Part => 1;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input", true);

            var patterns = new List<Pattern>();
            var patternLines = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    patterns.Add(new Pattern(patternLines.ToArray()));
                    patternLines = new List<string>();
                }
                else
                {
                    patternLines.Add(line);
                }
            }

            patterns.Add(new Pattern(patternLines.ToArray()));
            long vertSum = 0;
            long horiSum = 0;
            for (int i = 0; i < patterns.Count; i++)
            {
                var refVal = patterns[i].FindReflectionValueVert();
                if (refVal != -1)
                {
                    refVal += 1;
                    vertSum += refVal;
                    Console.WriteLine($"Pattern {i} has reflection value {refVal} (vertical)");
                }
                else
                {
                    refVal = patterns[i].FindReflectionValueHoriz();
                    if (refVal == -1) throw new Exception("No reflection?");
                    refVal += 1;
                    horiSum += refVal;
                    Console.WriteLine($"Pattern {i} has reflection value {refVal} (horizontal)");
                }
            }

            long sumOfNotes = horiSum * 100 + vertSum;
            Console.WriteLine($"Result for part {Part} is {sumOfNotes}");
        }

        private class Pattern
        {
            public char[][] Content { get; set; }

            public Pattern(string[] lines)
            {
                Content = new char[lines.Length][];
                for (int i = 0; i < lines.Length; i++)
                {
                    Content[i] = lines[i].ToCharArray();
                }
            }

            public int FindReflectionValueHoriz()
            {
                int reflectVal = -1;
                for (int i = 0; i < Content.Length - 1; i++)
                {
                    var currentLine = new string(Content[i]);
                    var nextLine = new string(Content[i + 1]);

                    if (currentLine == nextLine && CheckReflectionHoriz(i))
                    {
                        reflectVal = i;
                        break;
                    }
                }

                return reflectVal;
            }

            public int FindReflectionValueVert()
            {
                int reflectVal = -1;
                for (int i = 0; i < Content[0].Length - 1; i++)
                {
                    var currentCol = GetColumnAsString(i);
                    var nextCol = GetColumnAsString(i + 1);

                    if (currentCol == nextCol && CheckReflectionVert(i))
                    {
                        reflectVal = i;
                        break;
                    }
                }

                return reflectVal;
            }

            private string GetColumnAsString(int col)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < Content.Length; i++) sb.Append(Content[i][col]);

                return sb.ToString();
            }

            private bool CheckReflectionHoriz(int aboveLine)
            {
                for (int i = 0; i <= aboveLine; i++)
                {
                    var currentId = aboveLine - i;
                    if (currentId < 0) break;

                    var current = new string(Content[currentId]);

                    var reflectId = aboveLine + i + 1;
                    if (reflectId >= Content.Length) break;

                    var reflect = new string(Content[reflectId]);

                    if (current != reflect) return false;
                }

                return true;
            }

            private bool CheckReflectionVert(int leftCol)
            {
                for (int i = 0; i <= leftCol; i++)
                {
                    var currentId = leftCol - i;
                    if (currentId < 0) break;

                    var current = GetColumnAsString(currentId);

                    var reflectId = leftCol + i + 1;
                    if (reflectId >= Content[0].Length) break;

                    var reflect = GetColumnAsString(reflectId);

                    if (current != reflect) return false;
                }

                return true;
            }
        }
    }
}
