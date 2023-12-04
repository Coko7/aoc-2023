namespace aoc_2023.src.day04
{
	public class Day04Solver : AocSolver
	{
		// Link to the puzzle: https://adventofcode.com/2023/day/4
		
		public override int Day => 4;
		public override void Solve()
		{
			string[] lines = ReadInputLines("input");

			int sum = 0;
			foreach (var line in lines)
			{
				int startPos = line.IndexOf(':');
				if (startPos < 0) continue;

				string cardPrefix = line[..startPos];
				string remaining = line[(startPos + 1)..];

				var parts = remaining.Split('|');
				var winningNumbers = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
				var myNumbers = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

				int pointMul = 0;
				foreach (var winNum in  winningNumbers)
				{
					string? match = myNumbers.FirstOrDefault(num => num == winNum);
					if (match != null)
					{
						if (pointMul == 0) pointMul = 1;
						else pointMul *= 2;
					}
				}
				Console.WriteLine("Win points: " + pointMul);

				sum += pointMul;
			}
			
			Console.WriteLine($"Result is {sum}");
		}
		
		public override void SolvePart2()
		{
			string input = ReadInput("example");
			string[] lines = ReadInputLines("example");
			
			Console.WriteLine($"Result for part 2 is ");
		}
	}
}
