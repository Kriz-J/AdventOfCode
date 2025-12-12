namespace Advent2025.Days;

public class Day01_SecretEntrance
{
    public static string[] Input { get; set; } = SantasLittleHelpers.ReadFileRows("2025-12-01.txt");

    public static void Puzzle1()
    {
        var zeroCount = 0;
        var value = 50;

        foreach (var rotation in Input)
        {
            var turns = int.Parse(rotation[1..]);

            if (rotation[0] == 'L')
            {
                value = Modulo(value - turns, 100);
            }
            else if (rotation[0] == 'R')
            {
                value = Modulo(value + turns, 100);
            }

            if (value == 0)
            {
                zeroCount++;
            }
        }

        Console.WriteLine($"Answer: The password is {zeroCount}.");
    }

    public static void Puzzle2()
    {
        var zeroTouch = 0;
        var value = 50;

        foreach (var rotation in Input)
        {
            var turns = int.Parse(rotation[1..]);

            if (rotation[0] == 'L')
            {
                var revolutions = turns / 100;
                zeroTouch += revolutions;

                if (value != 0 && value - (turns % 100) <= 0)
                {
                    zeroTouch++;
                }

                value = Modulo(value - turns, 100);
            }
            else if (rotation[0] == 'R')
            {
                zeroTouch += (value + turns) / 100;
                value = Modulo(value + turns, 100);
            }
        }

        Console.WriteLine($"Answer: The password is {zeroTouch}.");
    }

    private static int Modulo(int dividend, int divisor)
    {
        return ((dividend % divisor) + divisor) % divisor;
    }
}