namespace Days;

public class Day4_CampCleanup
{
    private static readonly string[] AssignmentPairs = File.ReadAllLines(@"..\..\..\Resources\Day4_Puzzle_Input.txt");

    public static int FullyContainedRanges { get; set; }
    public static int OverlappingRanges { get; set; }

    public static void Part1And2()
    {
        foreach (var pair in AssignmentPairs)
        {
            var sectionRangesInPair = pair.Split(',');

            var firstElfSections = GetElfSections(sectionRangesInPair[0]);
            var secondElfSections = GetElfSections(sectionRangesInPair[1]);

            if (firstElfSections.IsSupersetOf(secondElfSections))
                FullyContainedRanges++;
            else if (firstElfSections.IsSubsetOf(secondElfSections))
                FullyContainedRanges++;

            if (firstElfSections.Intersect(secondElfSections).Any())
                OverlappingRanges++;

        }

        Console.WriteLine(@$"The answer to the first puzzle is {FullyContainedRanges}");
        Console.WriteLine(@$"The answer to the second puzzle is {OverlappingRanges}");
    }


    private static HashSet<int> GetElfSections(string range)
    {
        var sectionRange = range.Split('-');

        if (!int.TryParse(sectionRange[0], out var startSection))
            throw new ArgumentException("Can't parse start section.");
        if (!int.TryParse(sectionRange[1], out var endSection))
            throw new ArgumentException("Can't parse end section.");

        return new HashSet<int>(Enumerable.Range(startSection, endSection - startSection + 1));
    }
}