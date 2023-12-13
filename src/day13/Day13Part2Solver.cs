using System.Text;

namespace aoc_2023.src.day13
{
    public class Day13Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/13

        public override int Day => 13;
        public override int Part => 2;

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
                var refValVert = patterns[i].FindReflectionValueVert();
                var refValHoriz = patterns[i].FindReflectionValueHoriz();

                if (refValVert != - 1 && (refValHoriz == -1 || refValVert <= refValHoriz))
                {
                    refValVert += 1;
                    vertSum += refValVert;
                    Console.WriteLine($"Pattern {i} has reflection value {refValVert} (vertical)");
                }
                else if (refValHoriz != -1 && (refValVert == -1 || refValHoriz <= refValVert))
                {
                    refValHoriz += 1;
                    horiSum += refValHoriz;
                    Console.WriteLine($"Pattern {i} has reflection value {refValHoriz} (horizontal)");
                }
                else
                {
                    throw new Exception("No reflection found?");
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
                    if (CheckReflectionHoriz(i))
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
                    if (CheckReflectionVert(i))
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
                bool smudgeFound = false;
                for (int i = 0; i <= aboveLine; i++)
                {
                    var currentId = aboveLine - i;
                    if (currentId < 0) break;

                    var current = new string(Content[currentId]);

                    var reflectId = aboveLine + i + 1;
                    if (reflectId >= Content.Length) break;

                    var reflect = new string(Content[reflectId]);

                    int diffs = CountStrDiffs(current, reflect);
                    if (!smudgeFound)
                    {
                        if (diffs == 1)
                        {
                            int smudgePos = LocateSingleDiff(current, reflect);
                            smudgeFound = true;
                            // No need to modify the smudge
                            //if (current[smudgePos] == '#') Content[currentId][smudgePos] = '.';
                            //else Content[currentId][smudgePos] = '#';
                        }
                        else if (diffs > 1) return false;
                    }
                    else
                    {
                        if (diffs > 0) return false;
                    }
                }

                return smudgeFound;
            }

            private static int CountStrDiffs(string a, string b)
            {
                int diffs = 0;
                for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) diffs++;

                return diffs;
            }

            private static int LocateSingleDiff(string a, string b)
            {
                for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) return i;
                return -1;
            }

            private bool CheckReflectionVert(int leftCol)
            {
                bool smudgeFound = false;
                for (int i = 0; i <= leftCol; i++)
                {
                    var currentId = leftCol - i;
                    if (currentId < 0) break;

                    var current = GetColumnAsString(currentId);

                    var reflectId = leftCol + i + 1;
                    if (reflectId >= Content[0].Length) break;

                    var reflect = GetColumnAsString(reflectId);

                    int diffs = CountStrDiffs(current, reflect);
                    if (!smudgeFound)
                    {
                        if (diffs == 1)
                        {
                            int smudgePos = LocateSingleDiff(current, reflect);
                            smudgeFound = true;
                            // No need to modify the smudge
                            //if (current[smudgePos] == '#') Content[smudgePos][currentId] = '.';
                            //else Content[smudgePos][currentId] = '#';
                        }
                        else if (diffs > 1) return false;
                    }
                    else
                    {
                        if (diffs > 0) return false;
                    }
                }

                return smudgeFound;
            }
        }
    }
}
