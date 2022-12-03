namespace AdventOfCode2022;

public class Day3_RucksackReorganization
{
    private static readonly string[] Rucksacks = File.ReadAllLines(@"..\..\..\Resources\Day3_Puzzle_Input.txt");
    public static int TotalPriority { get; set; }

    public static void Part1()
    {
        foreach (var rucksack in Rucksacks)
        {
            var compartmentDividerPosition = rucksack.Length / 2;

            var leftCompartment = rucksack[..compartmentDividerPosition];
            var rightCompartment = rucksack[compartmentDividerPosition..];

            var leftCompartmentItems = new HashSet<char>(leftCompartment);
            var rightCompartmentItems = new HashSet<char>(rightCompartment);

            var misplacedItem = leftCompartmentItems.Intersect(rightCompartmentItems).FirstOrDefault();

            TotalPriority += CalculatePriority(misplacedItem);
        }

        Console.WriteLine(@$"The answer to the first puzzle is: {TotalPriority}");
    }

    public static void Part2()
    {
        for (int i = 0; i < Rucksacks.Length; i += 3)
        {
            var rucksack1 = new HashSet<char>(Rucksacks[i]);
            var rucksack2 = new HashSet<char>(Rucksacks[i + 1]);
            var rucksack3 = new HashSet<char>(Rucksacks[i + 2]);

            var groupBadge = rucksack1.Intersect(rucksack2).Intersect(rucksack3).FirstOrDefault();

            TotalPriority += CalculatePriority(groupBadge);
        }

        Console.WriteLine(@$"The answer to the second puzzle is: {TotalPriority}");
    }

    private static int CalculatePriority(char item) => item switch
    {
        >= 'a' and <= 'z' => item - 'a' + 1,
        >= 'A' and <= 'Z' => item - 'A' + 27,
        _ => throw new ArgumentOutOfRangeException()
    };
}