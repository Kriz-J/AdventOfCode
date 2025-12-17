using Range = (long Start, long End);

namespace Advent2025.Days;

public static class Day05_Cafeteria
{
    private static string[] Input { get; set; } = SantasLittleHelpers.ReadFileRows("2025-12-05.txt");

    public static void Puzzle1()
    {
        var (freshRanges, ingredientIds) = ParseInput();

        var freshIngredients = 0;

        foreach (var ingredientId in ingredientIds)
        {
            var id = long.Parse(ingredientId);

            foreach (var range in freshRanges)
            {
                var start = long.Parse(range.Split('-')[0]);
                var end = long.Parse(range.Split('-')[1]);

                if (id < start || id > end)
                {
                    continue;
                }

                freshIngredients++;
                break;
            }
        }

        Console.WriteLine($"Answer: There are {freshIngredients} fresh ingredients.");
    }

    public static void Puzzle2()
    {
        var freshRanges = ParseInput().FreshRanges;

        var mergedRanges = new List<Range>();

        // Merge individual fresh ranges
        foreach (var inputRange in freshRanges)
        {
            var start = long.Parse(inputRange.Split('-')[0]);
            var end = long.Parse(inputRange.Split('-')[1]);
            var range = new Range(start, end);

            var merged = false;

            if (!mergedRanges.Any())
            {
                mergedRanges.Add(range);
                continue;
            }

            for (var i = 0; i < mergedRanges.Count; i++)
            {
                if (TryMergeRange(range, mergedRanges[i], out var mergedRange))
                {
                    mergedRanges[i] = mergedRange;
                    merged = true;
                    break;
                }
            }

            if (!merged) // Disjoint
            {
                mergedRanges.Add(range);
            }
        }

        // Consolidate the merged ranges
        for (var i = 0; i < mergedRanges.Count - 1; i++)
        {
            for (var j = i + 1; j < mergedRanges.Count; j++)
            {
                if (TryMergeRange(mergedRanges[i], mergedRanges[j], out var consolidatedRange))
                {
                    mergedRanges.RemoveAt(j);
                    mergedRanges.RemoveAt(i);
                    mergedRanges.Add(consolidatedRange);
                    i--;
                    break;
                }
            }
        }

        var freshIngredientIds = mergedRanges.Sum(range => (range.End - range.Start + 1));

        Console.WriteLine($"Answer: There are {freshIngredientIds} fresh ingredient IDs.");
    }

    private static (string[] FreshRanges, string[] IngredientIds) ParseInput()
    {
        var breakIndex = Array.IndexOf(Input, string.Empty);

        return (Input[..breakIndex], Input[(breakIndex + 1)..]);
    }

    private static bool TryMergeRange(Range range, Range mergedRange, out Range updatedRange)
    {
        if (range.Start < mergedRange.Start && range.End > mergedRange.Start && range.End <= mergedRange.End) // Left overlap
        {
            updatedRange = (range.Start, mergedRange.End);
            return true;
        }

        if (range.End > mergedRange.End && range.Start > mergedRange.Start && range.Start <= mergedRange.End) // Right overlap
        {
            updatedRange = (mergedRange.Start, range.End);
            return true;
        }

        if (range.Start < mergedRange.Start && range.End > mergedRange.End) // Superset
        {
            updatedRange = (range.Start, range.End);
            return true;
        }

        if (range.Start >= mergedRange.Start && range.End <= mergedRange.End) // Subset
        {
            updatedRange = (mergedRange.Start, mergedRange.End);
            return true;
        }

        updatedRange = (range.Start, range.End);
        return false;
    }
}

//350684792662845