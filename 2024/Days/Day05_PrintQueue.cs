namespace Advent2024.Days;

public class Day05_PrintQueue
{
    public static string[] Input { get; set; } = SantasLittleHelpers.ReadFileRows("2024-12-05.txt");

    public static HashSet<string> Rules { get; set; } = Input
        .TakeWhile(s => s != string.Empty)
        .ToHashSet();

    public static List<string[]> Updates {get; set; } = Input
        .Reverse()
        .TakeWhile(s => s != string.Empty)
        .Select(u => u.Split(','))
        .ToList();

    public static void Puzzle1()
    {
        List<string> middlePageNumbers = [];
        foreach (var update in Updates)
        {
            var i = 0;
            while (i < update.Length - 1 && !Rules.Contains($"{update[i + 1]}|{update[i]}"))
            {
                i++;
            }
            
            if (i == update.Length - 1)
            {
                middlePageNumbers.Add(update[i / 2]);
            }
        }

        var middlePageNumbersSum = middlePageNumbers
            .Select(int.Parse)
            .Sum();

        Console.WriteLine($"Answer: The total sum of all middle page numbers of correct updates is {middlePageNumbersSum}");
    }

    public static void Puzzle2()
    {
        List<string[]> incorrectUpdates = [];
        foreach (var update in Updates)
        {
            var i = 0;
            while (i < update.Length - 1 && !Rules.Contains($"{update[i + 1]}|{update[i]}"))
            {
                i++;
            }

            if (i < update.Length - 1)
            {
                incorrectUpdates.Add(update);
            }
        }

        List<string> middlePageNumbers = [];
        foreach (var update in incorrectUpdates)
        {
            var i = 0;
            while (i < update.Length - 1)
            {
                var j = i;
                while (j < update.Length - 1 && Rules.Contains($"{update[j + 1]}|{update[j]}")) // Search through each update and start from the beginning again if changing place of two pages
                {
                    (update[j], update[j + 1]) = (update[j + 1], update[j]);
                    i = -1;
                    j++;
                }

                i++;
            }
            
            middlePageNumbers.Add(update[i / 2]);
        }

        var middlePageNumbersSum = middlePageNumbers
            .Select(int.Parse)
            .Sum();

        Console.WriteLine($"Answer: The total sum of all middle page numbers of corrected updates is {middlePageNumbersSum}");
    }
}