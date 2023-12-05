namespace aoc_2023.src.day03
{
	public class Day03Solver : AocSolver
	{
		// Link to the puzzle: https://adventofcode.com/2023/day/3
		
		public override int Day => 3;
		public override void Solve()
		{
			string[] lines = ReadInputLines("input");

			var grid = ProcessGrid(lines);

			var partNumbers = grid.FindPartNumbers();
			grid.Display();

			int sum = 0;
			foreach (var num in partNumbers)
			{
				Console.WriteLine(num.NumValue);
				if (num.NumValue < 0) throw new Exception("UNPARSED NUM");
				sum += num.NumValue;
			}
			
			Console.WriteLine($"Result is {sum}");
		}
		
		public override void SolvePart2()
		{
			string[] lines = ReadInputLines("input");

			var grid = ProcessGrid(lines);

			var partNumbers = grid.FindPartNumbers();
			grid.Display();

			int sum = 0;
			for (int y = 0; y < grid.Cells.Length; y++)
			{
				for (int x = 0; x < grid.Cells[y].Length; x++)
				{
					var cell = grid.Cells[y][x];
					if (cell.Content?.Type == "sym")
					{
						var sym = (EngineSymbol)cell.Content;
						if (sym.Value == '*' && sym.AdjacentPartNumbers.Count == 2)
						{
							int gearRatio = 1;
							foreach (var partNum in sym.AdjacentPartNumbers)
							{
								gearRatio *= partNum.NumValue;
							}
							Console.WriteLine("Gear ratio is " + gearRatio);
							sum += gearRatio;
						}
					}
				}
			}
			
			Console.WriteLine($"Result for part 2 is {sum}");
		}

		public static EngineGrid ProcessGrid(string[] lines)
		{
			var grid = new EngineGrid(lines.Length);

			for (int y = 0; y < lines.Length; y++)
			{
				grid.Cells[y] = new EngineCell[lines[y].Length];

				EngineNumber? currentNum = null;
				for (int x = 0; x < lines[y].Length; x++)
				{
					var cell = new EngineCell(x, y);
                    char c = lines[y][x];

					if (char.IsDigit(c)) // digit
					{
                        if (currentNum == null)
						{
							currentNum = new EngineNumber
							{
								StartPos = new Position(x, y),
								AdjacentSymbol = null,
								StrValue = $"{c}",
								NumValue = -1
							};
						}
						else
						{
							currentNum.StrValue = $"{currentNum.StrValue}{c}";
						}

                        var digit = new EngineDigit
                        {
                            Cell = cell,
							Type = "dig",
                            Value = c,
                            ParentNumber = currentNum
                        };
                        cell.Content = digit;
                    }
                    else
					{
						if (currentNum != null)
						{
							currentNum.EndPos = new Position(x, y);
							currentNum.NumValue = int.Parse(currentNum.StrValue);
							currentNum = null;
						}

						if (c == '.') // empty cell
						{
							cell.Content = null;
						}
						else // symbol
						{
                            var symbol = new EngineSymbol
                            {
                                Cell = cell,
								Type = "sym",
                                Value = c,
                                AdjacentPartNumbers = new List<EngineNumber>()
                            };
                            cell.Content = symbol;
                        }
                    }

					grid.Cells[y][x] = cell;
				}

				if (currentNum != null)
				{
					currentNum.EndPos = new Position(lines[y].Length - 1, y);
					currentNum.NumValue = int.Parse(currentNum.StrValue);
				}
			}

			return grid;
		}


		public class EngineGrid
		{
			public EngineCell[][] Cells { get; set; }

			public EngineGrid(int height)
			{
				Cells = new EngineCell[height][];
			}

			public void Display()
			{
				for (int y = 0; y < Cells.Length; y++)
				{
					for (int x = 0; x < Cells[y].Length; x++)
					{
						var content = Cells[y][x].Content;
						string text = "" + (content?.Value ?? '.');

						if (content == null)
						{
							Console.ForegroundColor = ConsoleColor.DarkGray;
						}
						else if (content.Type == "dig")
						{
							var dig = (EngineDigit)content;
							if (dig.ParentNumber.IsPartNumber)
							{
								Console.ForegroundColor = ConsoleColor.Yellow;
							}
							else
							{
								Console.ForegroundColor = ConsoleColor.Gray;
							}
						}
						else if (content.Type == "sym")
						{
							Console.ForegroundColor = ConsoleColor.Red;
						}

						Console.Write(text);
						Console.ResetColor();
						//line += item;
					}
					Console.WriteLine();
					//Console.WriteLine(line);
				}
			}

            public static ICollection<CellContent> GetAdjacentContent(EngineGrid grid, int posX, int posY)
            {
                var nearContents = new List<CellContent>();
                for (int y = posY - 1; y <= posY + 1; y++)
                {
                    if (y < 0 || y >= grid.Cells.Length) continue;
                    for (int x = posX - 1; x <= posX + 1; x++)
                    {
                        if (x < 0 || x >= grid.Cells[y].Length) continue;
						if (y == posY && x == posX) continue;

                        var cell = grid.Cells[y][x];
                        if (cell.Content != null)
                        {
                            nearContents.Add(cell.Content);
                        }
                    }
                }

                return nearContents;
            }

            public ICollection<EngineNumber> FindPartNumbers()
			{
				var numbers = new List<EngineNumber>();
				for (int y = 0; y < Cells.Length; y++)
				{
					for (int x = 0; x < Cells[y].Length; x++)
					{
						var cell = Cells[y][x];
						if (cell.Content != null && cell.Content.Type == "dig")
						{
							var digit = (EngineDigit)cell.Content;
							if (digit.ParentNumber.IsPartNumber) continue;

							var adjacentContents = GetAdjacentContent(this, x, y);
							foreach (var content in adjacentContents)
							{
								if (content.Type == "sym")
								{
									if (!digit.ParentNumber.IsPartNumber)
									{
										var sym = (EngineSymbol)content;
										digit.ParentNumber.AdjacentSymbol = sym;
										sym.AdjacentPartNumbers.Add(digit.ParentNumber);
                                        numbers.Add(digit.ParentNumber);
                                    }
                                }
							}
						}
					}
				}

				return numbers;
			}
		}

		public class EngineCell
		{
			public Position Position { get; set; }
			public CellContent? Content { get; set; }

			public EngineCell(int x, int y)
			{
				Position = new Position(x, y);
			}
		}

		public class CellContent
		{
			public EngineCell Cell { get; set; }
			public char Value { get; set; }
			public string Type { get; set; }
		}

		public class EngineNumber
		{
			public Position StartPos { get; set; }
			public Position EndPos { get; set; }
			public string StrValue { get; set; }
			public int NumValue { get; set; }

			public EngineSymbol? AdjacentSymbol { get; set; }
			public bool IsPartNumber => AdjacentSymbol != null;
		}

		public class EngineDigit : CellContent
		{
			public EngineNumber ParentNumber { get; set; }
		}

		public class EngineSymbol : CellContent
		{
			public bool IsGear { get; set; } = false;
			public ICollection<EngineNumber> AdjacentPartNumbers { get; set; }
		}

		public class Position
		{
			public int X { get; set; }
			public int Y { get; set; }

			public Position(int x, int y)
			{
				X = x;
				Y = y;
			}
		}
	}
}
