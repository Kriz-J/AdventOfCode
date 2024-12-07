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

    private static bool IsReportSafe(IReadOnlyList<int> report)
    {
        var isIncreasing = true;
        var isDecreasing = true;
        for (var i = 1; i < report.Count; i++)
        {
            var difference = report[i - 1] - report[i];
            if (difference > 0)
            {
                isDecreasing = false;
                if (difference > 3)
                {
                    return false;
                }
            }
            else if (difference < 0)
            {
                isIncreasing = false;
                if (difference < -3)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        return isIncreasing || isDecreasing;
    }


    public static void Puzzle2()
    {
        var reports = Input
            .Select(report => report.Split(" ")
                .Select(int.Parse)
                .ToList())
            .ToList();

        var safeReports = 0;
        foreach (var report in reports.ToList())
        {
            if (report.Count == 1)
            {
                safeReports++;
                continue;
            }

            var increasing = 0;
            var decreasing = 0;
            var i = 0;
            var retry = true;
            while (i < report.Count - 1)
            {
                if (ChangeBetweenLevels(report[i], report[i + 1]) is < 1 or > 3)
                {
                    if (retry)
                    {
                        if (i == 0 && !(ChangeBetweenLevels(report[i + 1], report[i + 2]) is < 1 or > 3))
                        {
                            report.RemoveAt(i);
                        }
                        else if (i > 0 && !(ChangeBetweenLevels(report[i - 1], report[i + 1]) is < 1 or > 3))
                        {
                            report.RemoveAt(i);
                        }
                        else
                        {
                            report.RemoveAt(i + 1);
                        }

                        increasing = 0;
                        decreasing = 0;
                        retry = false;
                        i = -1;
                    }
                    else
                    {
                        break;
                    }
                    i++;
                }
                else if (report[i] + report[i + 1] > 2 * report[i]) //Increasing    // 3 2 6 7 8
                {
                    increasing++;
                    if (decreasing is not 0)
                    {
                        if (retry)
                        {

                            if (report[i + 1] == report[i - 1])
                            {
                                report.RemoveAt(i + 1);
                            }
                            else
                            {
                                report.RemoveAt(i);
                            }

                            increasing = 0;
                            decreasing = 0;
                            retry = false;
                            i = -1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    i++;
                }
                else if (report[i] + report[i + 1] < 2 * report[i]) //Decreasing
                {
                    decreasing++;
                    if (increasing is not 0)
                    {
                        if (retry)
                        {
                            if (report[i + 1] == report[i - 1])
                            {
                                report.RemoveAt(i + 1);
                            }
                            else
                            {
                                report.RemoveAt(i);
                            }

                            increasing = 0;
                            decreasing = 0;
                            retry = false;
                            i = -1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    i++;
                }
                else
                {
                    i++;

                }
                if ((increasing == report.Count - 1 || decreasing == report.Count - 1) && i == report.Count - 1)
                {
                    safeReports++;
                }

            }

        }

        Console.WriteLine($"Answer: The number of safe reports is {safeReports}.");
    }

    private static int ChangeBetweenLevels(int level1, int level2) => Math.Abs(level2 - level1);
    private static bool IsIncreasing(int level1, int level2) => level1 + level2 > 2 * level1;
    private static bool IsDecreasing(int level1, int level2) => level1 + level2 < 2 * level1;
}