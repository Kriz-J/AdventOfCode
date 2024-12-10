using System.Text.RegularExpressions;

namespace Advent2024.Days;

public class Day03_MullItOver
{
    public static string Input { get; set; } = SantasLittleHelpers.ReadFile("2024-12-03.txt");
    private const string FirstFactor = @"(?<=mul\()\d{1,3}(?=,\d{1,3}\))";
    private const string SecondFactor = @"(?<=mul\(\d{1,3},)\d{1,3}(?=\))";

    public static void Puzzle1()
    {
        var regex = new Regex($"{FirstFactor}|{SecondFactor}");
        var factors = regex
            .Matches(Input)
            .Select(m => int.Parse(m.Value))
            .ToList();

        var mulSum = factors
            .Select((value, index) => (Value: value, Index: index))
            .Where(pair => pair.Index % 2 == 0)
            .Select(pair => pair.Value * factors[pair.Index + 1])
            .Sum();

        Console.WriteLine($"Answer: The sum of all multiplications is {mulSum}");
    }

    public static void Puzzle2()
    {
        var regex = new Regex($@"do\(\)|don't\(\)|{FirstFactor}|{SecondFactor}");
        var instructions = regex.Matches(Input).Select(m => m.Value).ToList();

        var i = 0;
        var mulSum = 0;
        while (i < instructions.Count)
        {
            switch (instructions[i])
            {
                case "don't()":
                    while (i < instructions.Count && instructions[i] != "do()")
                    {
                        i++;
                    }
                    break;
                case "do()":
                    i++;
                    break;
                default:
                    mulSum += int.Parse(instructions[i]) * int.Parse(instructions[i + 1]);
                    i += 2;
                    break;
            }
        }

        Console.WriteLine($"Answer: The sum of all multiplications is {mulSum}");
    }
}