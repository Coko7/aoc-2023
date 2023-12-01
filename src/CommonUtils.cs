using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace aoc_2023.src
{
    public class CommonUtils
    {
        public static string ProjectDirectory
        {
            get
            {
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;
                return projectDirectory;
            }
        }

        public static string DataDir => Path.Combine(ProjectDirectory, "data");
        public static string SrcDir => Path.Combine(ProjectDirectory, "src");

        public static AocSolver InstantiateSolver(int day)
        {
            string dayLabel = GetDayLabel(day);
            string className = $"aoc_2023.src.day{dayLabel}.Day{dayLabel}Solver";

            var type = Type.GetType(className);
            if (type == null) throw new Exception($"Failed to instantiate solver for day {day}");

            var instance = (AocSolver)Activator.CreateInstance(type)!;
            if (instance == null) throw new Exception($"Failed to instantiate solver for day {day}");

            return instance;
        }

        private static string GetDayLabel(int day) => day < 10 ? $"0{day}" : $"{day}";
        public static void SetupAOCWorkspace()
        {
            Console.WriteLine("Setting up AOC work environment...");

            SetupAOCData();
            SetupAOCSolvers();

            Console.WriteLine("AOC work environment successfully setup!");
        }

        private static void SetupAOCData()
        {
            for (int day = 1; day <= 25; day++)
            {
                string dirName = $"day{GetDayLabel(day)}";
                string dayDirPath = Path.Combine(DataDir, dirName);
                if (!Directory.Exists(dayDirPath))
                {
                    Console.WriteLine($"Creating directory for day {day}...");
                    Directory.CreateDirectory(dayDirPath);
                }

                CreateFileIfMissing(Path.Combine(dayDirPath, "example"));
                CreateFileIfMissing(Path.Combine(dayDirPath, "example2"));

                CreateFileIfMissing(Path.Combine(dayDirPath, "input"));
                CreateFileIfMissing(Path.Combine(dayDirPath, "input2"));

                CreateFileIfMissing(Path.Combine(dayDirPath, "custom"));
            }
        }

        private static void SetupAOCSolvers()
        {
            //string dayDirPath = Path.Combine(CODE_DIR, "day02");
            //CreateSolverIfMissing(dayDirPath, 2);

            for (int day = 1; day <= 25; day++)
            {
                string dirName = $"day{GetDayLabel(day)}";
                string dayDirPath = Path.Combine(SrcDir, dirName);
                if (!Directory.Exists(dayDirPath))
                {
                    Console.WriteLine($"Creating directory for day {day}...");
                    Directory.CreateDirectory(dayDirPath);
                }

                CreateSolverIfMissing(dayDirPath, day);
            }
        }

        private static void CreateSolverIfMissing(string dirPath, int day)
        {
            string label = GetDayLabel(day);
            string className = $"Day{label}Solver";
            string fileName = $"{className}.cs";
            string filePath = Path.Combine(dirPath, fileName);

            if (File.Exists(filePath)) return;

            string content = $"namespace aoc_2023.src.day{label}\n" +
                "{\n" +
                "\tpublic class " + className + " : AocSolver\n" +
                "\t{\n" +
                "\t\t// Link to the puzzle: " + $"https://adventofcode.com/2023/day/{day}\n" +
                "\t\t\n" +
                "\t\tpublic override int Day => " + day + ";\n" +
                "\t\tpublic override void Solve()\n" +
                "\t\t{\n" +
                "\t\t\tstring input = ReadInput(\"example\");\n" +
                "\t\t\tstring[] lines = ReadInputLines(\"example\");\n" +
                "\t\t\t\n" +
                "\t\t\tConsole.WriteLine($\"Result is \");\n" +
                "\t\t}\n" +
                "\t\t\n" +
                "\t\tpublic override void SolvePart2()\n" +
                "\t\t{\n" +
                "\t\t\tstring input = ReadInput(\"example\");\n" +
                "\t\t\tstring[] lines = ReadInputLines(\"example\");\n" +
                "\t\t\t\n" +
                "\t\t\tConsole.WriteLine($\"Result for part 2 is \");\n" +
                "\t\t}\n" +
                "\t}\n" +
                "}\n";

            File.WriteAllText(filePath, content);
        }

        private static void CreateFileIfMissing(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }

        public static string ReadInput(int day, string name)
        {
            string dayLabel = GetDayLabel(day);
            string dirName = $"day{dayLabel}";
            string filePath = Path.Combine(DataDir, dirName, name);
            if (!File.Exists(filePath))
            {
                throw new Exception($"Invalid input file for day {day}: {name}");
            }

            return File.ReadAllText(filePath);
        }
    }
}
