using aoc_2023.src.Utils;

namespace aoc_2023.src.day20
{
    public class Day20Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/20

        public override int Day => 20;
        public override int Part => 2;

        public override void Solve()
        {
            string[] lines = ReadInputLines("input");
            var modules = new List<Module>();
            foreach (var line in lines)
            {
                var mod = ModulesFactory.BuildModule(line);
                modules.Add(mod);
            }

            BroadcastMod broadcaster = (BroadcastMod)modules.Single(m => m.Name == "broadcaster");
            var button = new ButtonMod(broadcaster);

            broadcaster.InputModules.Add(button);
            foreach (var line in lines)
            {
                var parts = line.Split("->");
                var id = parts[0].Trim();

                if (id.StartsWith('%') || id.StartsWith('&')) id = id[1..];
                var outputModNames = parts[1].Trim().Split(',');

                var mod = modules.Single(m => m.Name == id);
                foreach (var outputModName in outputModNames)
                {
                    var trimmed = outputModName.Trim();
                    var outputMod = modules.SingleOrDefault(m => m.Name == trimmed);
                    if (outputMod == null)
                    {
                        outputMod = new VoidMod(trimmed);
                        modules.Add(outputMod);
                    }

                    mod.OutputModules.Add(outputMod);
                    if (!outputMod.InputModules.Contains(mod)) outputMod.InputModules.Add(mod);
                }
            }

            foreach (var mod in modules)
            {
                if (mod is ConjunctionMod conj) conj.Init();
            }

            long lowCount = 0;
            long highCount = 0;

            long btnPresses = 0;

            var cycleDict = new Dictionary<Module, ICollection<long>>();

            var rxMod = modules.Single(m => m.Name == "rx");

            // In my case, the "rx" module is a void module with a single input.
            // Its input is a conjunction module, which means it will only output a low pulse if all the stored pulses are high.
            // Similarly to day 8, it looks like LCM is what we need to use here.
            ConjunctionMod conjMod = (ConjunctionMod)rxMod.InputModules.First();

            var conjCycles = new Dictionary<Module, long>();
            foreach (var mod in conjMod.InputModules) conjCycles.Add(mod, -1);

            // Loop until all cycles have been found 
            while (conjCycles.Any(kvp => kvp.Value == -1))
            {
                var bufferedPulses = new Queue<Tuple<Module, Module, bool>>();
                bufferedPulses.Enqueue(button.Push());
                btnPresses++;

                while (bufferedPulses.Count > 0)
                {
                    var pulse = bufferedPulses.Dequeue();
                    var fromMod = pulse.Item1;
                    var toMod = pulse.Item2;
                    var val = pulse.Item3;

                    if (val) highCount++;
                    else lowCount++;

                    var niuPulses = toMod.HandlePulse(fromMod, val);

                    // Check for cycles
                    if (toMod.Name == conjMod.Name)
                    {
                        foreach (var kvp in conjMod.Memory)
                        {
                            // if high value is stored in memory then save the current number of button presses
                            if (kvp.Value && conjCycles[kvp.Key] == -1) conjCycles[kvp.Key] = btnPresses;
                        }
                    }

                    foreach (var niuPulse in niuPulses) bufferedPulses.Enqueue(niuPulse);
                }
            }

            // Use LCM to find the least button press required
            var lbp = MathUtils.Lcm(conjCycles.Values);
            Console.WriteLine($"Result for part {Part} is {lbp}");
        }

        private static class ModulesFactory
        {
            public static Module BuildModule(string raw)
            {
                var parts = raw.Split("->");
                var id = parts[0].Trim();
                //var outputs = parts[1].Trim();

                if (id == "broadcaster") return new BroadcastMod(id);
                else
                {
                    char type = id[0];
                    var name = id[1..];
                    if (type == '%') return new FlipFlopMod(name);
                    else if (type == '&') return new ConjunctionMod(name);
                    else throw new Exception("Invalid module!");
                }
            }
        }

        private abstract class Module
        {
            public string Name { get; private set; }
            public ICollection<Module> InputModules { get; set; }
            public ICollection<Module> OutputModules { get; set; }
            public abstract ICollection<Tuple<Module, Module, bool>> HandlePulse(Module from, bool pulse);

            public ICollection<Tuple<Module, Module, bool>> GenerateOutputPulses(bool pulse)
            {
                var pulses = new List<Tuple<Module, Module, bool>>();
                foreach (var mod in OutputModules) pulses.Add(new Tuple<Module, Module, bool>(this, mod, pulse));
                return pulses;
            }

            public Module(string name)
            {
                Name = name;
                InputModules = new List<Module>();
                OutputModules = new List<Module>();
            }

            public override string ToString() => $"{Name} -> {string.Join(", ", OutputModules.Select(m => m.Name))}";
        }

        private class ButtonMod : Module
        {
            public ButtonMod(BroadcastMod broadcastModule) : base("Button")
            {
                OutputModules.Add(broadcastModule);
            }

            public Tuple<Module, Module, bool> Push() => new(this, OutputModules.First(), false);

            public override ICollection<Tuple<Module, Module, bool>> HandlePulse(Module from, bool pulse)
            {
                throw new NotImplementedException();
            }
        }

        private class BroadcastMod : Module
        {
            public BroadcastMod(string name) : base(name) { }

            public override ICollection<Tuple<Module, Module, bool>> HandlePulse(Module from, bool pulse) => GenerateOutputPulses(pulse);
        }

        private class VoidMod : Module
        {
            public VoidMod(string name) : base(name) { }

            public override ICollection<Tuple<Module, Module, bool>> HandlePulse(Module from, bool pulse) => new List<Tuple<Module, Module, bool>>();
        }

        private class FlipFlopMod : Module
        {
            public bool IsOn { get; set; }

            public FlipFlopMod(string name) : base(name)
            {
                IsOn = false;
            }

            public override ICollection<Tuple<Module, Module, bool>> HandlePulse(Module from, bool pulse)
            {
                if (pulse) return new List<Tuple<Module, Module, bool>>();

                IsOn = !IsOn;
                return GenerateOutputPulses(IsOn);
            }
        }

        private class ConjunctionMod : Module
        {
            public IDictionary<Module, bool> Memory { get; set; }

            public ConjunctionMod(string name) : base(name)
            {
                Memory = new Dictionary<Module, bool>();
            }

            public void Init()
            {
                Memory = new Dictionary<Module, bool>();
                foreach (var mod in InputModules)
                {
                    Memory.Add(mod, false);
                }
            }

            public override ICollection<Tuple<Module, Module, bool>> HandlePulse(Module from, bool pulse)
            {
                Memory[from] = pulse;

                var outputPulse = !Memory.All(p => p.Value);
                return GenerateOutputPulses(outputPulse);
            }
        }
    }
}
