namespace Advent2024.Days;

public class Day02_RedNosedReports
{
    public static string[] Input { get; set; } = SantasLittleHelpers.ReadFileRows("2024-12-02.txt");

    public static void Puzzle1()
    {
        var reports = Input
            .Select(report => report.Split(" ")
                .Select(int.Parse)
                .ToList())
            .ToList();

        var safeReports = reports.Count(IsReportSafe);

        Console.WriteLine($"Answer: The number of safe reports is {safeReports}.");
    }

    public static void Puzzle2()
    {
        var reports = Input
            .Select(report => report.Split(" ")
                .Select(int.Parse)
                .ToList())
            .ToList();

        var safeReports = reports.Count(IsReportSafeWithProblemDampener);
        Console.WriteLine($"Answer: The number of safe reports with the problem dampener applied is {safeReports}.");
    }

    private static bool IsReportSafe(IReadOnlyList<int> report)
    {
        for (var i = 0; i < report.Count - 1; i++)
        {
            var isValidChange = Math.Abs(report[i + 1] - report[i]) is (1 or 2 or 3);
            var isMonotonic = i == 0 || Math.Sign(report[i + 1] - report[i]) == Math.Sign(report[i] - report[i - 1]);
            
            if (!isValidChange || !isMonotonic)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsReportSafeWithProblemDampener(IReadOnlyList<int> report)
    {
        if (IsReportSafe(report))
        {
            return true;
        }

        for (var i = 0; i < report.Count; i++)
        {
            var modifiedReport = report.Where((_, index) => index != i).ToList();

            if (IsReportSafe(modifiedReport))
            {
                return true;
            }
        }

        return false;
    }
}