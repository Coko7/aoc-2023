namespace aoc_2023.src.day02
{
	public class Day02Solver : AocSolver
	{
		// Link to the puzzle: https://adventofcode.com/2023/day/2
		
		public override int Day => 2;
		public override void Solve()
		{
			string[] lines = ReadInputLines("input");
			var bagLoad = new Dictionary<string, int>()
			{
				{ "red", 12 },
				{ "green", 13 },
				{ "blue", 14 },
			};

			int sum = 0;
			foreach (var line in lines)
			{
				var game = CubeGame.CreateFromLine(line);
				var possible = game.IsGamePossible(bagLoad);
				if (possible)
				{
					sum += game.Id;
					Console.WriteLine($"Game {game.Id} is possible");
				}
			}
			
			Console.WriteLine($"Result is {sum}");
		}
		
		public override void SolvePart2()
		{
			string[] lines = ReadInputLines("input");

			int powSum = 0;
			foreach (var line in lines)
			{
				var game = CubeGame.CreateFromLine(line);
				int power = game.GetGamePower();
				powSum += power;
				Console.WriteLine($"Game {game.Id}: {power}");
				//if (possible)
				//{
				//	sum += game.Id;
				//	Console.WriteLine($"Game {game.Id} is possible");
				//}
			}
			
			Console.WriteLine($"Result is {powSum}");
		}
	}

	public class CubeGame
	{
		public int Id { get; set; }
		public ICollection<CubeSet> CubeSets { get; set; }

		public IDictionary<string, int> CountMaxValues()
		{
			var dict = new Dictionary<string, int>();
			foreach (var set in CubeSets)
			{
				foreach (var kvp in set.CubeSubSets)
				{
                    if (!dict.TryGetValue(kvp.Value, out int existingVal))
                    {
                        existingVal = 0;
                    }

                    if (kvp.Key > existingVal)
					{
						dict[kvp.Value] = kvp.Key;
					}
				}
			}

			return dict;
		}

		public int GetGamePower()
		{
			int power = 1;
			var maxValues = CountMaxValues();
			foreach (var kvp in maxValues)
			{
				power *= kvp.Value;
				Console.WriteLine($"\t - {kvp.Key}: {kvp.Value}");
			}

			return power;
		}

		public bool IsGamePossible(IDictionary<string, int> bagLoad)
		{
			var maxValues = CountMaxValues();
			foreach (var kvp in bagLoad)
			{
				string color = kvp.Key;
				int loadVal = kvp.Value;

                if (!maxValues.TryGetValue(color, out int maxVal))
                {
                    maxVal = 0;
                }

                if (maxVal > loadVal) return false;
			}

			return true;
		}

		public static CubeGame CreateFromLine(string line)
		{
			string prefix = "Game ";
			string formattedLine = line[(line.IndexOf(prefix) + prefix.Length)..];
			int colonIdx = formattedLine.IndexOf(':');

			string gameIdStr = formattedLine[..colonIdx];
			int gameId = int.Parse(gameIdStr);

			var game = new CubeGame
			{
				Id = gameId,
				CubeSets = ParseSets(formattedLine[(colonIdx + 1)..])
			};

			return game;
        }

		public static ICollection<CubeSet> ParseSets(string str)
		{
			string[] rawSets = str.Split(";");
			var cubeSets = new List<CubeSet>();

			foreach (var rawSet in rawSets)
			{
				var cubeSet = ParseSet(rawSet);
				cubeSets.Add(cubeSet);
			}

			return cubeSets;
		}

		public static CubeSet ParseSet(string str)
		{
			string formattedStr = str.Trim();
			string[] splitByColors = formattedStr.Split(',');

			var cubeSet = new CubeSet
			{
				CubeSubSets = new List<KeyValuePair<int, string>>()
			};

			foreach (var split in splitByColors)
			{
				var pair = split.Trim().Split(' ');
				int qty = int.Parse(pair[0]);
				var kvp = new KeyValuePair<int, string>(qty, pair[1]);
				cubeSet.CubeSubSets.Add(kvp);
			}

			return cubeSet;
		}
	}

    public class CubeSet
    {
        public ICollection<KeyValuePair<int, string>> CubeSubSets { get; set; }
    }
}
