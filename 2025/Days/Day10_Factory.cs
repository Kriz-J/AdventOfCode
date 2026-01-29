namespace Advent2025.Days;

public static class Day10_Factory
{
    private static readonly string[] Input = SantasLittleHelpers.ReadFileRows("2025-12-10.txt");

    private static readonly Dictionary<string, List<List<int[]>>> ParityCache = [];

    public static void Puzzle1()
    {
        var (indicatorLightDiagrams, buttonWiringSchematics, _) = ParseInput(Input);

        var fewestPresses = new int[indicatorLightDiagrams.Length];
        for (int i = 0; i < indicatorLightDiagrams.Length; i++)
        {
            fewestPresses[i] = FindFewestPressesForIndicatorLights(indicatorLightDiagrams[i], buttonWiringSchematics[i], indicatorLightDiagrams[i].ArrayXOR(indicatorLightDiagrams[i]), 0, 0);
        }

        Console.WriteLine($"Answer: The sum of the fewest button presses required is {fewestPresses.Sum()}");
    }

    public static void Puzzle2()
    {
        var (indicatorLightDiagrams, buttonWiringSchematics, joltageRequirements) = ParseInput(Input);

        var fewestPresses = new int[indicatorLightDiagrams.Length];
        for (int i = 0; i < indicatorLightDiagrams.Length; i++)
        {
            fewestPresses[i] = FindFewestPressesForJoltageRequirement(joltageRequirements[i], buttonWiringSchematics[i]);
            ParityCache.Clear();
        }

        Console.WriteLine($"Answer: The sum of the fewest button presses required is {fewestPresses.Sum()}");
    }

    private static (int[][] IndicatorLightDiagrams, List<int[]>[] ButtonWiringSchematics, int[][] JoltageRequirements) ParseInput(string[] input)
    {
        var indicatorLightDiagrams = new int[Input.Length][];
        var buttonWiringSchematics = new List<int[]>[Input.Length];
        var joltageRequirements = new int[Input.Length][];

        for (var i = 0; i < Input.Length; i++)
        {
            var machine = Input[i];

            var indicatorLightDiagramEnd = machine.IndexOf(']');
            var joltageRequirementsStart = machine.IndexOf('{');

            var rawIndicatorLightDiagram = machine[1..indicatorLightDiagramEnd];
            var rawButtonWiringSchematic = machine[(indicatorLightDiagramEnd + 1)..joltageRequirementsStart];
            var rawJoltageRequirement = machine[(joltageRequirementsStart + 1)..^1];

            indicatorLightDiagrams[i] = ParseIndicatorLightDiagram(rawIndicatorLightDiagram);
            buttonWiringSchematics[i] = ParseButtonWiringSchematics(rawButtonWiringSchematic, indicatorLightDiagrams[i].Length);
            joltageRequirements[i] = ParseJoltageRequirement(rawJoltageRequirement);
        }

        return (indicatorLightDiagrams, buttonWiringSchematics, joltageRequirements);
    }

    private static int[] ParseIndicatorLightDiagram(string input)
    {
        var indicatorLightDiagram = new int[input.Length];

        for (var j = 0; j < input.Length; j++)
        {
            if (input[j] == '#')
            {
                indicatorLightDiagram[j] = 1;
            }
        }

        return indicatorLightDiagram;
    }

    private static List<int[]> ParseButtonWiringSchematics(string input, int numberOfCounters)
    {
        var buttonWiringSchematics = new List<int[]>();

        for (int j = 0; j < input.Length; j++)
        {
            if (input[j] == '(')
            {
                var buttonWiringSchematic = new int[numberOfCounters];

                while (input[++j] != ')')
                {
                    if (int.TryParse(input[j].ToString(), out var index))
                    {
                        buttonWiringSchematic[index] = 1;
                    }
                }

                buttonWiringSchematics.Add(buttonWiringSchematic);
            }
        }

        return buttonWiringSchematics;
    }

    private static int[] ParseJoltageRequirement(string input)
    {
        return input.Split(',').Select(int.Parse).ToArray();
    }

    private static int FindFewestPressesForIndicatorLights(int[] target, List<int[]> buttons, int[] state, int index, int sum)
    {
        if (state.SequenceEqual(target))
        {
            return sum;
        }

        if (index >= buttons.Count)
        {
            return 1_000_000;
        }

        var with = FindFewestPressesForIndicatorLights(target, buttons, state.ArrayXOR(buttons[index]), index + 1, sum + 1);
        var without = FindFewestPressesForIndicatorLights(target, buttons, state, index + 1, sum);

        return Math.Min(with, without);
    }

    private static int FindFewestPressesForJoltageRequirement(int[] joltageRequirement, List<int[]> buttons)
    {
        if (joltageRequirement.IsOnlyZeros())
        {
            return 0;
        }
        
        var parityArray = joltageRequirement.ToParityArray();
        var zeroArray = parityArray.ArrayXOR(parityArray);

        var cacheKey = string.Join(',', parityArray);
        if (!ParityCache.TryGetValue(cacheKey, out var configurations))
        {
            configurations = FindViableButtonConfigurations(parityArray, buttons, zeroArray);
            ParityCache.Add(cacheKey, configurations);
        }

        var candidates = new List<int>();
        foreach (var configuration in configurations)
        {
            var joltageDifference = configuration.Aggregate(zeroArray, (current, button) => current.ArrayAdd(button));

            var newJoltageRequirement = joltageRequirement.ArraySubtract(joltageDifference);
            if (newJoltageRequirement.Any(i => i < 0))
            {
                continue;
            }

            candidates.Add(FindFewestPressesForJoltageRequirement(newJoltageRequirement.ArrayDivideByHalf(), buttons) * 2 + configuration.Count);
        }

        return candidates.Count == 0 ? 1_000_000 : candidates.Min();
    }

    private static List<List<int[]>> FindViableButtonConfigurations(int[] target, List<int[]> buttons, int[] startState)
    {
        var totalPaths = 1 << buttons.Count;

        var state = new int[startState.Length];

        var buttonConfigurations = new List<List<int[]>>();

        for (var mask = 0; mask < totalPaths; mask++)
        {
            List<int[]> buttonConfiguration = [];

            Array.Copy(startState, state, startState.Length);

            for (var i = 0; i < buttons.Count; i++)
            {
                if (((mask >> i) & 1) == 0)
                {
                    continue;
                }

                state = state.ArrayXOR(buttons[i]);
                buttonConfiguration.Add(buttons[i]);
            }

            if (state.SequenceEqual(target))
            {
                buttonConfigurations.Add(buttonConfiguration);
            }
        }

        return buttonConfigurations;
    }

    extension(int[] input)
    {
        private int[] ArrayXOR(int[] change)
        {
            var result = new int[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                result[i] = input[i] ^ change[i];
            }

            return result;
        }

        private bool IsOnlyZeros()
        {
            return input.All(e => e == 0);
        }

        private int[] ToParityArray()
        {
            var result = new int[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                result[i] = input[i] & 1;
            }

            return result;
        }

        private int[] ArrayDivideByHalf()
        {
            var result = new int[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                result[i] = input[i] / 2;
            }

            return result;
        }

        private int[] ArrayAdd(int[] add)
        {
            var result = new int[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                result[i] = input[i] + add[i];
            }

            return result;
        }

        private int[] ArraySubtract(int[] subtract)
        {
            var result = new int[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                result[i] = input[i] - subtract[i];
            }

            return result;
        }
    }
}