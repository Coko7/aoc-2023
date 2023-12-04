﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc_2023.src
{
    public abstract class AocSolver
    {
        public abstract int Day { get; }
        public abstract void Solve();
        public abstract void SolvePart2();

        protected string ReadInput(string input)
        {
            return CommonUtils.ReadInput(Day, input);
        }

        protected string[] ReadInputLines(string input, bool keepEmptyLines = false)
        {
            string content = ReadInput(input);
            string[] lines = Regex.Split(content, "\r\n|\r|\n");
            if (!keepEmptyLines)
            {
                lines = lines.Where(str => !string.IsNullOrEmpty(str)).ToArray();
            }

            return lines;
        }
    }
}