namespace aoc_2023.src.day05
{
	public class Day05Solver : AocSolver
	{
		// Link to the puzzle: https://adventofcode.com/2023/day/5

		public override int Day => 5;
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

					for (int j = 0; j < seeds.Length; j++)
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

		public override void SolvePart2()
		{
			string input = ReadInput("example");
			string[] lines = ReadInputLines("example");

			Console.WriteLine($"Result for part 2 is ");
		}
	}
}
