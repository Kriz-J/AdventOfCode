using Position = (int Row, int Column);

namespace Advent2025.Days;

public static class Day07_Laboratories
{
    private static readonly string[] Input = SantasLittleHelpers.ReadFileRows("2025-12-07.txt");

    private static readonly HashSet<(int Row, int Column)> Splits = [];

    private static readonly Dictionary<Position, long> Timelines = [];

    public static void Puzzle1()
    {
        var start = Input[0].IndexOf('S');

        AdvanceBeam(1, start);

        Console.WriteLine($"Answer: There are {Splits.Count} splits.");
    }

    public static void Puzzle2()
    {
        var current = new Position(1, Input[0].IndexOf('S'));

        Timelines.Add((current.Row, current.Column), 1);

        while (true)
        {
            var currentTimelines = Timelines[current];

            if (Input[current.Row + 1][current.Column] == '^')
            {
                var leftPath = new Position(current.Row + 2, current.Column - 1);
                if (!Timelines.TryAdd(leftPath, currentTimelines))
                {
                    Timelines[leftPath] += currentTimelines;
                }

                var rightPath = new Position(current.Row + 2, current.Column + 1);
                if (!Timelines.TryAdd(rightPath, currentTimelines))
                {
                    Timelines[rightPath] += currentTimelines;
                }
            }
            else
            {
                var timeline = new Position(current.Row + 2, current.Column);
                if (!Timelines.TryAdd(timeline, currentTimelines))
                {
                    Timelines[timeline] += currentTimelines;
                }
            }
            
            var next = Timelines.Keys.FirstOrDefault(key => key.Row == current.Row && key.Column > current.Column);
            if (next is (0, 0))
            {
                next = Timelines.Keys.First(kvp => kvp.Row == (current.Row + 2) && kvp.Column > 0);
            }

            current = next;
            if (current.Row == Input.Length - 1)
            {
                break;
            }
        }

        var timelines = Timelines.Where(kvp => kvp.Key.Row == current.Row).Select(kvp => kvp.Value).Sum();

        Console.WriteLine($"Answer: There are {timelines} timelines.");
    }

    private static void AdvanceBeam(int row, int column)
    {
        if (row == Input.Length - 1)
        {
            return;
        }

        if (Input[row + 1][column] == '^')
        {
            Splits.Add((row + 1, column));

            if (!Splits.Contains((row + 1, column + 2)))
            {
                AdvanceBeam(row + 2, column + 1);
            }

            if (!Splits.Contains((row + 1, column - 2)))
            {
                AdvanceBeam(row + 2, column - 1);
            }
        }
        else
        {
            AdvanceBeam(row + 2, column);
        }
    }
}