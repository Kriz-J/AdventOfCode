namespace Advent2025.Days;

public static class Day06_TrashCompactor
{
    private static readonly string[] Input = SantasLittleHelpers.ReadFileRows("2025-12-06.txt");

    public static void Puzzle1()
    {
        var numbers = new int[Input.Length - 1][];
        foreach (var (i, value) in Input[..^1].Select((value, i) => (i, value)))
        {
            numbers[i] = value.Split(' ').Where(n => n != string.Empty).Select(int.Parse).ToArray();
        }
        var operators = Input[^1].Split(' ').Where(o => o != string.Empty).Select(char.Parse).ToArray();

        var total = 0L;
        for (var i = 0; i < operators.Length; i++)
        {
            if (operators[i] == '+')
            {
                total += numbers.Select(n => n[i]).Sum();
            }
            else if (operators[i] == '*')
            {
                total += numbers.Select(n => n[i]).Aggregate(1L, (current, number) => current * number);
            }
        }

        Console.WriteLine($"Answer: The grand total is {total}.");
    }

    public static void Puzzle2()
    {
        var operatorRow = Input[^1];
        var operatorPositions = Enumerable.Range(0, operatorRow.Length).Where(i => operatorRow[i] != ' ').ToArray();
        var operators = operatorPositions.Select(i => operatorRow[i]).ToArray();

        var numbers = new string[Input.Length - 1][];
        for (var i = 0; i < numbers.Length; i++)
        {
            numbers[i] = new string[operators.Length];

            for (var j = 0; j < operatorPositions.Length - 1; j++)
            {
                numbers[i][j] = Input[i][operatorPositions[j]..(operatorPositions[j + 1] - 1)];
            }
            numbers[i][^1] = Input[i][operatorPositions[^1]..];
        }

        var total = 0L;
        for (var i = 0; i < operators.Length; i++)
        {
            var cephalopodNumbers = ConvertToCephalopodNumbers(numbers.Select(n => n[i]).ToArray());

            if (operators[i] == '+')
            {
                total += cephalopodNumbers.Sum();
            }
            else if (operators[i] == '*')
            {
                total += cephalopodNumbers.Aggregate(1L, (current, number) => current * number);
            }
        }

        Console.WriteLine($"Answer: The grand total is {total}.");
    }

    private static int[] ConvertToCephalopodNumbers(string[] numbers)
    {
        var length = numbers[0].Length;

        var output = new int[length];

        for (var i = length - 1; i >= 0; i--)
        {
            var cephalopodNumber = string.Empty;

            foreach (var number in numbers)
            {
                if (number[i] != ' ')
                {
                    cephalopodNumber += number[i];
                }
            }

            output[i] = int.Parse(cephalopodNumber);
        }

        return output;
    }
}
