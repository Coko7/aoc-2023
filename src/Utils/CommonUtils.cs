using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace aoc_2023.src.Utils
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
        private static string GetDayLabel(int day) => day < 10 ? $"0{day}" : $"{day}";

        public static AocSolver InstantiateSolver(int day, bool isPart2)
        {
            string dayLabel = GetDayLabel(day);
            string className = $"aoc_2023.src.day{dayLabel}.Day{dayLabel}{(isPart2 ? "Part2" : "")}Solver";

            var type = Type.GetType(className);
            if (type == null)
            {
                // Solver not found, setup data and solver for today
                Console.WriteLine($"Solver not found: day{dayLabel}{(isPart2 ? " part 2" : "")}! Attempting to generate it...");
                SetupDataFilesForDay(day);
                SetupSolverForDay(day, isPart2);

                // Get type again
                type = Type.GetType(className);
            }

            if (type == null) throw new Exception($"Failed to get solver class for day {day}");

            var instance = (AocSolver)Activator.CreateInstance(type)!;
            if (instance == null) throw new Exception($"Failed to instantiate solver for day {day}");

            return instance;
        }

        public static void SetupAOCWorkspace()
        {
            Console.WriteLine("Setting up AOC work environment...");

            SetupAOCDataFull();
            SetupAOCSolversFull();

            Console.WriteLine("AOC work environment successfully setup!");
        }

        private static void SetupAOCDataFull()
        {
            for (int day = 1; day <= 25; day++)
            {
                SetupDataFilesForDay(day);
            }
        }

        private static void SetupDataFilesForDay(int day)
        {
            string dirName = $"day{GetDayLabel(day)}";
            string dayDirPath = Path.Combine(DataDir, dirName);
            if (!Directory.Exists(dayDirPath))
            {
                Console.WriteLine($"Creating data directory for day {day}...");
                Directory.CreateDirectory(dayDirPath);
            }

            CreateFileIfMissing(Path.Combine(dayDirPath, "example"));
            CreateFileIfMissing(Path.Combine(dayDirPath, "example2"));

            CreateFileIfMissing(Path.Combine(dayDirPath, "input"));
            CreateFileIfMissing(Path.Combine(dayDirPath, "input2"));

            CreateFileIfMissing(Path.Combine(dayDirPath, "custom"));
        }

        private static void SetupAOCSolversFull()
        {
            for (int day = 1; day <= 25; day++)
            {
                SetupSolverForDay(day, false);
            }
        }

        private static void SetupSolverForDay(int day, bool isPart2)
        {
            string dirName = $"day{GetDayLabel(day)}";
            string dayDirPath = Path.Combine(SrcDir, dirName);
            if (!Directory.Exists(dayDirPath))
            {
                Console.WriteLine($"Creating src directory for day {day}...");
                Directory.CreateDirectory(dayDirPath);
            }

            if (isPart2) CreatePart2SolverIfMissing(dayDirPath, day);
            else CreateSolverIfMissing(dayDirPath, day);
        }

        private static void CreatePart2SolverIfMissing(string dirPath, int day)
        {
            string label = GetDayLabel(day);

            string className = $"Day{label}Solver";
            string fileName = $"{className}.cs";
            string filePath = Path.Combine(dirPath, fileName);

            string className2 = $"Day{label}Part2Solver";
            string fileName2 = $"{className2}.cs";
            string filePath2 = Path.Combine(dirPath, fileName2);

            if (File.Exists(filePath2)) return;

            if (!File.Exists(filePath)) throw new Exception($"Unable to generate part 2 solver if part 1 is missing: {className} not found!");

            string fileContent = File.ReadAllText(filePath);
            string fileContent2 = fileContent.Replace(className, className2);
            fileContent2 = fileContent2.Replace("int Part => 1;", "int Part => 2;");

            File.WriteAllText(filePath2, fileContent2);
            Console.WriteLine($"Successfully generated new solver: " + className2);
        }

        private static void CreateSolverIfMissing(string dirPath, int day)
        {
            string label = GetDayLabel(day);
            string className = $"Day{label}Solver";
            string fileName = $"{className}.cs";
            string filePath = Path.Combine(dirPath, fileName);

            if (File.Exists(filePath)) return;

            string lf = "\n";

            string t = "\t";
            string tt = t + t;
            string ttt = t + t + t;

            string ob = "{";
            string cb = "}";

            var sb = new StringBuilder();
            sb.AppendLine($"namespace aoc_2023.src.day{label}");
            sb.AppendLine($"{ob}");
            sb.AppendLine($"{t}public class {className} : AocSolver");
            sb.AppendLine($"{t}{ob}");
            sb.AppendLine($"{tt}// Link to the puzzle: https://adventofcode.com/2023/day/{day}");
            sb.AppendLine();
            sb.AppendLine($"{tt}public override int Day => {day};");
            sb.AppendLine($"{tt}public override int Part => 1;");
            sb.AppendLine();
            sb.AppendLine($"{tt}public override void Solve()");
            sb.AppendLine($"{tt}{ob}");
            sb.AppendLine($"{ttt}string input = ReadInput(\"example\");");
            sb.AppendLine($"{ttt}string[] lines = ReadInputLines(\"example\");");
            sb.AppendLine();
            sb.AppendLine($"{ttt}Console.WriteLine" + "($\"Result for part {Part} is \");");
            sb.AppendLine($"{tt}{cb}");
            sb.AppendLine($"{t}{cb}");
            sb.AppendLine($"{cb}");

            File.WriteAllText(filePath, sb.ToString());
            Console.WriteLine($"Successfully generated new solver: " + className);
        }

        private static void CreateFileIfMissing(string filePath)
        {
            if (!File.Exists(filePath)) File.Create(filePath).Close();
        }

        private static void FetchInputAndWriteToFile(int day, int part)
        {
            var client = new HttpClient();
            //var response = client.GetAsync($"https://adventofcode.com/2023/day/{day}/input");
            //TODO: write input to file
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
