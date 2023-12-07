namespace aoc_2023.src.day04
{
	public class Day04Part2Solver : AocSolver
	{
		// Link to the puzzle: https://adventofcode.com/2023/day/4
		
		public override int Day => 4;
		public override int Part => 2;

		public override void Solve()
		{
			string[] lines = ReadInputLines("input");

			var cardStorage = new CardStorage(lines);
			while (true)
			{
				int returnCode = cardStorage.ProcessNextCard();
				if (returnCode != 0) break;
			}

			cardStorage.ShowInstances();
		}
		
		private class Card
		{
			public int Id { get; set; }
			public string OriginalStr { get; set; }
			public int[] WinningNumbers { get; set; }
			public int[] MyNumbers { get; set; }

			public int Copies { get; set; }

			public Card(string rawLine)
			{
				OriginalStr = rawLine;
				Copies = 1;
				
				var parts = rawLine.Split(':');
				var idStr = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];
				Id = int.Parse(idStr);

				var numParts = parts[1].Split('|');
				var winNums = numParts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
				var myNums = numParts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

				WinningNumbers = winNums.Select(numStr => int.Parse(numStr)).ToArray();
				MyNumbers = myNums.Select(numStr => int.Parse(numStr)).ToArray();
			}

			public int CountMatches()
			{
				int matches = 0;
				foreach (int winNum in WinningNumbers)
				{
					if (MyNumbers.Any(num => num == winNum))
					{
						matches++;
					}
				}

				return matches;
			}
		}

		private class CardStorage
		{
			public Card[] Cards { get; set; }
			private int _iterator;

			public CardStorage(string[] lines)
			{
				Cards = new Card[lines.Length];
				for (int i = 0; i < lines.Length; i++)
				{
					Cards[i] = new Card(lines[i]);
				}
				_iterator = 0;
			}

			public int ProcessNextCard()
			{
				if (_iterator >= Cards.Length) return 1;
				var current = Cards[_iterator];
				if (current == null) return 1;
                Console.WriteLine($"Processing Card {current.Id}: {current.Copies}");

                for (int j = 0; j < current.Copies; j++)
				{
                    var matches = current.CountMatches();
                    for (int i = 0; i < matches; i++)
                    {
                        Cards[_iterator + (i + 1)].Copies++;
                    }
                }

                _iterator++;
				return 0;
			}

			public void ShowInstances()
			{
				int total = 0;
				for (int i = 0; i < Cards.Length; i++)
				{
					var current = Cards[i];
					Console.WriteLine($"Card {current.Id}: {current.Copies}");
					total += current.Copies;
				}

				Console.WriteLine($"Total copies: {total}");
			}
        }
	}
}
