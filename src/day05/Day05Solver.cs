using System.Collections;

namespace aoc_2023.src.day05
{
    public class Day05Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/5

        public override int Day => 5;
        public override int Part => 1;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input", true);

            var parts = lines[0].Split(':');
            if (parts[0] != "seeds") throw new Exception("WTF");

            long[] seeds = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            for (int i = 0; i < seeds.Length; i++)
            {
                Console.Write(seeds[i] + " ");
            }
            Console.WriteLine();

            var transformations = new List<long[]>() { seeds };
            long[] currentSources = Array.Empty<long>();
            bool[] hasBeenMapped = new bool[seeds.Length];

            for (int i = 2; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Contains(':')) // Map section start
                {
                    var previousSources = transformations.Last();
                    currentSources = new long[previousSources.Length];
                    for (int j = 0; j < currentSources.Length; j++)
                    {
                        currentSources[j] = previousSources[j];
                        hasBeenMapped[j] = false;
                    }
                }
                else if (string.IsNullOrEmpty(line)) // end map
                {
                    transformations.Add(currentSources);
                    for (int j = 0; j < currentSources.Length; j++)
                    {
                        Console.Write(currentSources[j] + " ");
                    }
                    Console.WriteLine();
                }
                else
                {
                    var mapSections = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

                    long length = mapSections[2];

                    long destinationRangeStart = mapSections[0];
                    long destinationRangeEnd = destinationRangeStart + length;

                    long sourceRangeStart = mapSections[1];
                    long sourceRangeEnd = sourceRangeStart + length;

                    long mapTransfo = destinationRangeStart - sourceRangeStart;

                    for (int j = 0; j < currentSources.Length; j++)
                    {
                        var item = currentSources[j];
                        if (item >= sourceRangeStart && item <= sourceRangeEnd && !hasBeenMapped[j])
                        {
                            long mappedItem = item + mapTransfo;
                            currentSources[j] = mappedItem;
                            hasBeenMapped[j] = true;
                        }
                    }
                }
            }

            var finalResults = transformations.Last();
            var minNum = finalResults.Min();

            Console.WriteLine($"Result is {minNum}");
        }

        private class SeedMap
        {
            public List<SeedMapping> Mappings { get; set; } = new List<SeedMapping>();

            public void AddRawMapping(string rawLine)
            {
                var newMapping = new SeedMapping(rawLine);
                Mappings.Add(newMapping);
                Mappings.Sort();
            }

            public long Map(long item)
            {
                for (int i = 0; i < Mappings.Count; i++)
                {
                    var mapping = Mappings[i];
                    if (item < mapping.SourceStart) return item;

                    if (item >= mapping.SourceStart && item < mapping.SourceEnd)
                    {
                        return mapping.TrustedTransform(item);
                    }
                }

                return item;
            }
        }

        private class SeedMapping : IComparable<SeedMapping>
        {
            public long SourceStart { get; set; }
            public long SourceEnd { get; set; }
            public long DestinationStart { get; set; }
            public long DestinationEnd { get; set; }
            public long Length { get; set; }

            public SeedMapping(int sourceStart, int destinationStart, int length)
            {
                SourceStart = sourceStart;
                SourceEnd = sourceStart + length;
                DestinationStart = destinationStart;
                DestinationEnd = destinationStart + length;
                Length = length;
            }

            public SeedMapping(string rawLine)
            {
                var parts = rawLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(part => long.Parse(part)).ToArray();
                Length = parts[2];

                SourceStart = parts[1];
                SourceEnd = parts[1] + parts[2];
                DestinationStart = parts[0];
                DestinationEnd = parts[0] + parts[2];
            }

            public long TrustedTransform(long item)
            {
                long diff = DestinationStart - SourceStart;
                return item + diff;
            }

            public int CompareTo(SeedMapping? other)
            {
                if (other == null) return 1;

                if (other.SourceStart > SourceStart) return -1;
                if (other.SourceStart < SourceStart) return 1;

                return 0;
            }

            public override string ToString()
            {
                return $"{SourceStart} - {SourceEnd} : {DestinationStart} - {DestinationEnd} ({Length})";
            }
        }
    }
}
