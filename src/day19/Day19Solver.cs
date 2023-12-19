namespace aoc_2023.src.day19
{
    public class Day19Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/19

        public override int Day => 19;
        public override int Part => 1;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input", keepEmptyLines: true);
            int partsDefLineStart = -1;

            var workflows = new List<Workflow>();
            var ratings = new List<RatingEntry>();
            var accepted = new List<RatingEntry>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrEmpty(line))
                {
                    partsDefLineStart = i + 1;
                    break;
                }

                var workflow = new Workflow(line);
                workflows.Add(workflow);
            }

            for (int i = partsDefLineStart; i < lines.Length; i++)
            {
                var line = lines[i];
                ratings.Add(new RatingEntry(line));
            }

            foreach (var rating in ratings)
            {
                var currentWorkflow = workflows.Single(wf => wf.Name == "in");
                while (true)
                {
                    var action = currentWorkflow.Handle(rating);
                    if (action == "A")
                    {
                        accepted.Add(rating);
                        break;
                    }
                    else if (action == "R") break;
                    else
                    {
                        currentWorkflow = workflows.Single(wf => wf.Name == action);
                    }
                }
            }

            int sum = 0;
            foreach (var entry in accepted)
            {
                sum += entry.PartRatings.Sum(kvp => kvp.Value);
            }

            Console.WriteLine($"Result for part {Part} is {sum}");
        }

        private class RatingEntry
        {
            public ICollection<KeyValuePair<string, int>> PartRatings { get; set; } = new List<KeyValuePair<string, int>>();

            public RatingEntry(string raw)
            {
                string inner = raw[1..^1];
                var ratingsRaw = inner.Split(',');
                foreach (var rating in ratingsRaw)
                {
                    var parts = rating.Split("=");
                    PartRatings.Add(new KeyValuePair<string, int>(parts[0], int.Parse(parts[1])));
                }
            }
        }

        private class Workflow
        {
            public string Name { get; set; }
            public ICollection<Rule> Rules { get; set; } = new List<Rule>();

            public Workflow(string raw)
            {
                int openBrPos = raw.IndexOf('{');
                int closeBrPos = raw.LastIndexOf('}');

                Name = raw[..openBrPos];
                string innerRulesRaw = raw[(openBrPos + 1)..closeBrPos];
                var parts = innerRulesRaw.Split(',');

                foreach (var part in parts)
                {
                    Rule rule = part.Contains(':') ? new ComparisonRule(part) : new SimpleRule(part);
                    Rules.Add(rule);
                }
            }

            public string Handle(RatingEntry ratingEntry)
            {
                foreach (var rule in Rules)
                {
                    var action = rule.Apply(ratingEntry);
                    if (!string.IsNullOrEmpty(action)) return action;
                }

                throw new Exception("At least one rule should have matched!");
            }
        }

        private abstract class Rule
        {
            public string Action { get; set; }

            public abstract string? Apply(RatingEntry ratingEntry);
        }

        private class SimpleRule : Rule
        {
            public SimpleRule(string raw)
            {
                Action = raw;
            }

            public override string? Apply(RatingEntry ratingEntry) => Action;
        }

        private class ComparisonRule : Rule
        {
            public string PartCategory { get; set; }
            public bool IsGreaterCompare { get; set; }
            public int ComparisonNumber { get; set; }

            public ComparisonRule(string raw)
            {
                var parts = raw.Split(':');
                Action = parts[1];

                if (parts[0].Contains('<'))
                {
                    parts = parts[0].Split('<');
                    IsGreaterCompare = false;
                }
                else
                {
                    parts = parts[0].Split('>');
                    IsGreaterCompare = true;
                }

                PartCategory = parts[0];
                ComparisonNumber = int.Parse(parts[1]);
            }

            public bool Apply(int val)
            {
                if (IsGreaterCompare) return val > ComparisonNumber;
                else return val < ComparisonNumber;
            }

            public override string? Apply(RatingEntry ratingEntry)
            {
                var part = ratingEntry.PartRatings.Single(pr => pr.Key == PartCategory);
                if (IsGreaterCompare)
                {
                    if (part.Value > ComparisonNumber) return Action;
                    else return null;
                }
                else
                {
                    if (part.Value < ComparisonNumber) return Action;
                    else return null;
                }
            }
        }
    }
}
