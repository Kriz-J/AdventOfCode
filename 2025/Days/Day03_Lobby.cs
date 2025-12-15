namespace Advent2025.Days;

public class Day03_Lobby
{
    public static string[] Input { get; set; } = SantasLittleHelpers.ReadFileRows("2025-12-03.txt");

    public static void Puzzle1()
    {
        var totaltOutputJoltage = 0;

        foreach (var bank in Input)
        {
            var battery1 = bank[0];
            var battery2 = '0';

            for (var i = 1; i < bank.Length; i++)
            {
                if (bank[i] > battery2)
                {
                    battery2 = bank[i];
                }

                if (battery2 > battery1 && i < bank.Length - 1)
                {
                    battery1 = battery2;
                    battery2 = '0';
                }
            }

            totaltOutputJoltage += int.Parse($"{battery1.ToString()}{battery2.ToString()}");
        }

        Console.WriteLine($"Answer: The total output joltage is: {totaltOutputJoltage}");
    }

    public static void Puzzle2()
    {
        long outputJoltage = 0;

        foreach (var bank in Input)
        {
            var joltage = string.Empty;
            var index = 0;
            var battery = '0';
            var batteryPosition = 0;
            var upperLimit = bank.Length - (12 - joltage.Length);

            while (index <= upperLimit && joltage.Length != 12)
            {
                if (bank[index] > battery)
                {
                    battery = bank[index];
                    batteryPosition = index;
                }

                index++;

                if (index > upperLimit)
                {
                    joltage += battery;
                    battery = '0';
                    index = batteryPosition + 1;
                    upperLimit = bank.Length - (12 - joltage.Length);
                }
            }

            outputJoltage += long.Parse($"{joltage}");
        }

        Console.WriteLine($"Answer: The total output joltage is: {outputJoltage}");
    }
}