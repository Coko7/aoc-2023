using System.Text;

namespace aoc_2023.src.day15
{
    public class Day15Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/15

        public override int Day => 15;
        public override int Part => 2;

        public override void Solve()
        {
            string input = ReadInput("input");
            var sequences = input.Split(',');
            var boxes = new Dictionary<int, List<Lens>>();

            for (int i = 0; i < sequences.Length; i++)
            {
                var seq = sequences[i];
                var parts = seq.Split(new char[] { '=', '-' });
                string label = parts[0];

                var box = ComputeHash(label);

                if (seq.Contains('-'))
                {
                    if (boxes.ContainsKey(box))
                    {
                        var content = boxes[box];
                        boxes[box] = boxes[box].Where(lens => lens.Label != label).ToList();
                    }
                }
                else if (seq.Contains('='))
                {
                    int focalLen = int.Parse(parts[1]);
                    if (boxes.ContainsKey(box))
                    {
                        var existing = boxes[box].Find(lens => lens.Label == label);
                        if (existing != null)
                        {
                            existing.FocalLength = focalLen;
                        }
                        else
                        {
                            boxes[box].Add(new Lens(label, focalLen));
                        }
                    }
                    else
                    {
                        boxes.Add(box, new List<Lens> { new Lens(label, focalLen) });
                    }
                }

                Console.WriteLine($"After \"{seq}\":");
                DisplayAllBoxes(boxes);
                Console.WriteLine();
            }

            long sum = 0;
            for (int i = 0; i < 256; i++)
            {
                if (boxes.ContainsKey(i))
                {
                    var lenses = boxes[i];
                    for (int j = 0; j < lenses.Count; j++)
                    {
                        var focusPow = (i + 1) * (j + 1) * lenses[j].FocalLength;
                        sum += focusPow;
                    }
                }
            }

            Console.WriteLine($"Result for part {Part} is {sum}");
        }

        private static int ComputeHash(string str)
        {
            int val = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c == '\n') continue;
                val += c;
                val *= 17;
                val %= 256;
            }

            return val;
        }

        private static void DisplayAllBoxes(Dictionary<int, List<Lens>> boxes)
        {
            for (int i = 0; i < 256; i++)
            {
                if (boxes.ContainsKey(i) && boxes[i].Any())
                {
                    var sb = new StringBuilder($"Box {i}: ");
                    foreach (var lens in boxes[i]) sb.Append(lens.ToString() + " ");
                    Console.WriteLine(sb.ToString());
                }
            }
        }

        private class Lens
        {
            public string Label { get; set; }
            public int FocalLength { get; set; }

            public Lens(string label, int focalLength)
            {
                Label = label;
                FocalLength = focalLength;
            }

            public override string ToString() => $"[{Label} {FocalLength}]";
        }
    }
}
