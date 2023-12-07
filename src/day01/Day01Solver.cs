using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc_2023.src.day01
{
    public class Day01Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/1

        public override int Day => 1;
        public override int Part => 1;

        public override void Solve()
        {
            string input = ReadInput("input");
            string[] lines = ReadInputLines("input");

            int sum = 0;
            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    int number = ExtractAndCombineDigits(line);
                    Console.WriteLine(number);
                    sum += number;
                }
            }

            Console.WriteLine($"Sum is: {sum}");
        }

        private static int ExtractAndCombineDigits(string line)
        {
            string digits = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (char.IsDigit(line[i])) digits += line[i];
            }

            char first = digits[0];
            char last = digits[^1];
            string firstAndLast = $"{first}{last}";
            return int.Parse(firstAndLast);
        }
    }
}
