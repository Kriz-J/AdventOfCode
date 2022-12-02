namespace AdventOfCode2022;

public class Day1_CalorieCounting
{
    public static void Part1And2()
    {
        var inputData = File.ReadAllLines(@"..\..\..\Resources\Day1_Puzzle_Input.txt");

        var elvesCalories = new List<int>();

        var elfCalories = 0;
        
        foreach (var row in inputData)
        {

            if (string.IsNullOrWhiteSpace(row))
            {
                elvesCalories.Add(elfCalories);
                elfCalories = 0;
                continue;
            }

            if (!int.TryParse(row, out var itemCalories))
            {
                throw new ArgumentException("Error: Cannot process row string to integer.");
            }

            elfCalories += itemCalories;
        }
        
        Console.WriteLine(@$"The answer to the first puzzle is: {elvesCalories.Max()}");
        Console.WriteLine(@$"The answer to the second puzzle is: {elvesCalories.OrderByDescending(e => e).Take(3).Sum()}");
    }
}