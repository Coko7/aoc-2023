using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc_2023.src.day01
{
    public class Day01Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/1

        public override int Day => 1;
        public override int Part => 2;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input2");

            int sum = 0;
            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    int number = ExtractAndCombinePart2(line);
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

        private static int ExtractAndCombinePart2(string line)
        {
            string[] digitStrings = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string formattedLine = line;

            int? minPos = null;
            string minDigStr = "";
            int? minNum = null;

            for (int i = 0; i < digitStrings.Length; i++)
            {
                string digitStr = digitStrings[i];
                int firstPos = formattedLine.IndexOf(digitStr);

                if (firstPos != -1 && (minPos == null || firstPos < minPos))
                {
                    minPos = firstPos;
                    minDigStr = digitStr;
                    minNum = i;
                }
            }

            int firstDigit = -1;
            for (int i = 0; i < formattedLine.Length ; i++)
            {
                if (char.IsDigit(formattedLine[i]))
                {
                    firstDigit = i;
                    break;
                }
            }

            if (minPos != null && (firstDigit == -1 || minPos < firstDigit))
            {
                formattedLine = formattedLine.Substring(0, minPos.Value)
                    + minNum.Value
                    + formattedLine.Substring(minPos.Value + minDigStr.Length);
            }

            int? maxPos = null;
            string maxDigStr = "";
            int? maxNum = null;
            for (int i = 0; i < digitStrings.Length; i++)
            {
                string digitStr = digitStrings[i];
                int lastPos = formattedLine.LastIndexOf(digitStr);

                if (lastPos != -1 && (maxPos == null || lastPos > maxPos))
                {
                    maxPos = lastPos;
                    maxDigStr = digitStr;
                    maxNum = i;
                }
            }

            int lastDigit = -1;
            for (int i = formattedLine.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(formattedLine[i]))
                {
                    lastDigit = i;
                    break;
                }
            }

            if (maxPos != null && (lastDigit == -1 || maxPos > lastDigit))
            {
                formattedLine = formattedLine.Substring(0, maxPos.Value)
                    + maxNum.Value
                    + formattedLine.Substring(maxPos.Value + maxDigStr.Length);
            }

            // if (!string.IsNullOrEmpty(minDigStr) && !string.IsNullOrEmpty(maxDigStr))
            // {
            //     formattedLine = line.Substring(0, minPos)
            //         + minNum
            //         + line.Substring(minPos + minDigStr.Length, maxPos - (minPos + minDigStr.Length))
            //         + maxNum
            //         + line.Substring(maxPos + maxDigStr.Length);
            // }
            // else if (!string.IsNullOrEmpty(minDigStr))
            // {
            //     formattedLine = line.Substring(0, minPos)
            //         + minNum
            //         + line.Substring(minPos + minDigStr.Length);
            // }
            // else if (!string.IsNullOrEmpty(maxDigStr))
            // {
            //     formattedLine = line.Substring(0, minPos)
            //         + line.Substring(minPos + minDigStr.Length, maxPos - (minPos + minDigStr.Length))
            //         + maxNum
            //         + line.Substring(maxPos + maxDigStr.Length);
            // }

            //Console.WriteLine(formattedLine);

            return ExtractAndCombineDigits(formattedLine);
        }
    }
}
