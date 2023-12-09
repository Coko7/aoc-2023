namespace aoc_2023.src.day09
{
	public class Day09Solver : AocSolver
	{
		// Link to the puzzle: https://adventofcode.com/2023/day/9

		public override int Day => 9;
		public override int Part => 1;

		public override void Solve()
		{
			string[] lines = ReadInputLines("input");

			long sum = 0;
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				long[] sequence = line.Split(' ').Select(num => long.Parse(num)).ToArray();

				long extrapolator = FindExtrapolator(sequence);
				sum += extrapolator;
			}

			Console.WriteLine($"Result for part {Part} is {sum}");
		}

		private static long FindExtrapolator(long[] sequence)
		{
			var list = new List<long[]>();

			long[] currentSeq = sequence;
			list.Add(currentSeq);
			while (!currentSeq.All(num => num == 0))
			{
				var childSeq = GetChildSequence(currentSeq);	
				list.Add(childSeq);
				currentSeq = childSeq;
			}

			var arr = list.ToArray();

			return FindExtra(arr, 0);
		}

		private static long FindExtra(long[][] arr, int i)
		{
			if (i == arr.Length - 1) return 0;
			long last = arr[i].Last();
			return last + FindExtra(arr, i + 1);
		}

		private static long[] GetChildSequence(long[] sequence)
		{
			long[] childSeq = new long[sequence.Length - 1];

			for (int i = 0; i < sequence.Length - 1; i++)
			{
				childSeq[i] = sequence[i + 1] - sequence[i];
			}

			return childSeq;
		}
	}
}
