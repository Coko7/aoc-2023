using System.Collections;

namespace aoc_2023.src.day05
{
    public class Day05Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/5

        public override int Day => 5;
        public override int Part => 2;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input", true);

            var parts = lines[0].Split(':');
            if (parts[0] != "seeds") throw new Exception("WTF");

            long[] seedDescriptors = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

            long totalSizeArr = 0;
            for (int i = 1; i < seedDescriptors.Length; i += 2)
            {
                totalSizeArr += seedDescriptors[i];
            }
            Console.WriteLine($"Total seeds: {totalSizeArr}");

            long[] seeds = new long[totalSizeArr];
            int arrayIt = 0;
            Console.WriteLine($"Generating the {totalSizeArr} seeds...");

            for (int i = 0; i < seedDescriptors.Length; i += 2)
            {
                long rangeStart = seedDescriptors[i];
                long rangeLen = seedDescriptors[i + 1];

                totalSizeArr += rangeLen;

                for (long j = rangeStart; j < rangeStart + rangeLen; j++)
                {
                    seeds[arrayIt++] = j;
                }
            }

            Console.WriteLine("Full seed array generated");

            long[] currentSources = seeds;

            var currentMap = new SeedMap();
            for (int i = 2; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Contains(':')) // Map section start
                {
                    currentMap = new SeedMap();
                }
                else if (string.IsNullOrEmpty(line)) // end map
                {
                    Console.WriteLine("First map constructed");
                    for (int j = 0; j < currentSources.Length; j++)
                    {
                        long item = currentSources[j];
                        long transformed = currentMap.Map(item);
                        currentSources[j] = transformed;
                        //Console.Write(transformed + " ");
                    }
                    Console.WriteLine("Finished first mapping, line is " + i);
                }
                else
                {
                    currentMap.AddRawMapping(line);
                }
            }

            var finalResults = currentSources;// transformations.Last();
            var minNum = finalResults.Min();

            Console.WriteLine($"Result for part 2 is {minNum}");
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
