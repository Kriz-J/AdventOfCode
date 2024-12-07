namespace Advent2024.Days;

public class Day01_HistorianHysteria
{
    public static string[] Input { get; set; } = SantasLittleHelpers.ReadFileRows("2024-12-01.txt");

    public static void Puzzle1()
    {
        var (left, right) = CreateSortedLists();

        var totalDistance = left
            .Zip(right, (l, r) => Math.Abs(l - r))
            .Sum();

        Console.WriteLine($"Answer: Total distance between the lists is {totalDistance}.");
    }

    public static void Puzzle2()
    {
        var (left, right) = CreateSortedLists();

        var totalSimilarity = left
            .Select(l => l * right.Count(r => r == l))
            .Sum();

        Console.WriteLine($"Answer: Total similarity of the lists is {totalSimilarity}.");
    }

    private static  (List<int> Left, List<int> Right) CreateSortedLists()
    {
        var locationIDs = Input
            .SelectMany(row => row.Split("   "))
            .Select(int.Parse)
            .ToList();

        var (left, right) = PartitionList(locationIDs);

        left.Sort();
        right.Sort();

        return (left, right);
    }

    private static (List<int> Left, List<int> Right) PartitionList(IReadOnlyList<int> list)
    {
        var left = new List<int>();
        var right = new List<int>();

        for (var i = 0; i < list.Count; i++) // Optimal to do both in one loop compared to .Where()
        {
            if (i % 2 == 0)
            {
                left.Add(list[i]);
            }
            else
            {
                right.Add(list[i]);
            }
        }

        return (left, right);
    }
}