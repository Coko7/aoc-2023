# aoc-2023

My code for the [Advent of Code - 2023 edition](https://adventofcode.com/2023)

## 🛠️ Proper workspace

When I first began doing AOC challenges in 2022, I had no setup. I would create each file manually when it was time to do the daily challenge.
It worked but it made me lose some time everyday. Usually, I would create the file before the daily challenge came out but sometimes I forgot.
Also, I did not have any proper utility classes and I would copy the same code to read and parse the file from the previous day.

This year, I have done things a little differently.
I have created proper structure for my project and wrote utility classes to automatically generate the needed files, easily read the inputs and run the code.
Thanks to that, I can jump straight to solving the daily challenge instead of losing time doing things that could have been done automatically.

What the workspace takes care of:
- Automatically generating empty input files (example, input, input2, etc.) for the current day. I still need to manually copy my input data from AOC website inside the file but that takes like 5 seconds. *I am not so good at solving AOC programming challenges that these lost seconds matter anyway :)*
- Automatically generating a skeleton solver file (C# class file) for the current day:
  ```csharp
  namespace aoc_2023.src.dayXX
  {
      public class DayXXSolver : AocSolver
      {
          // Link to the puzzle: https://adventofcode.com/2023/day/XX
  
          public override int Day => XX;
          public override int Part => 1;
  
          public override void Solve()
          {
              string[] input = ReadInput("input");
              string[] lines = ReadInputLines("input");
  
              Console.WriteLine($"Result for Part {Part} is ");
          }
      }
  }
  ```
- When the project is run, the daily solver will be called and its code will be executed automatically
- Once I am done with the first part of the challenge, I just need to change a boolean in the main function and the solver for part 2 will be automatically generated next time I run the project
- The solver created for part 2 is a direct copy of the solver file for part 1 but with some tiny adjustments (mostly file/class name and `Part` value)

## 💼 Workflow

When it's time to tackle the daily challenges:
- I run the app once to generate the daily files (data folder and solver skeleton for part 1)
- I read the challenge description and start implementing a solution
- I test the solution against example data and make sure it works before testing with my own input
- Once I get the golden star for part 1, I push the code to Git
- Then, I change a boolean in my main function to indicate that I am ready for part 2
- Upon running the project, the solver for part 2 gets automatically generated and it contains the same code as part 1
- I tweak the code of the new generated solver so that it works for part 2
- I test with my inputs and edit the code until I find something that works and I get my second star
- Finally, I push code to Git
