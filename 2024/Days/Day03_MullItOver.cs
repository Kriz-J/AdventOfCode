using System.Text.RegularExpressions;

namespace Advent2024.Days;

public class Day03_MullItOver
{
    public static string Input { get; set; } = SantasLittleHelpers.ReadFile("2024-12-03.txt");

    public static void Puzzle1()
    {
        const string firstFactor = @"(?<=mul\()\d{1,3}(?=,\d{1,3}\))";
        const string secondFactor = @"(?<=mul\(\d{1,3},)\d{1,3}(?=\))";

        var regex = new Regex($"{firstFactor}|{secondFactor}");
        var factors = regex
            .Matches(Input)
            .Select(m => int.Parse(m.Value))
            .ToList();

        var mulSum = factors
            .Select((value, index) => (Value:value, Index:index))
            .Where(pair => pair.Index % 2 == 0)
            .Select(pair => pair.Value * factors[pair.Index + 1])
            .Sum();

        Console.WriteLine($"Answer: The sum of all multiplications is {mulSum}"); //183788984
    }

    public static void Puzzle2()
    {

    }
}