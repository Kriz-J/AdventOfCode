namespace Days;

public class Day6_TuningTrouble
{
    private static readonly string DataStreamBuffer = File.ReadAllText(@"..\..\..\Resources\Day6_Puzzle_Input.txt");

    public static int StartOfPacketMarkerLength => 4;
    public static int StartOfMessageMarkerLength => 14;

    public static void Part1()
    {
        var firstOccurrence = FindEndOfMarker(DataStreamBuffer, StartOfPacketMarkerLength);
        Console.WriteLine(@$"The answer to the first puzzle is {firstOccurrence}");
    }

    public static void Part2()
    {
        var firstOccurrence = FindEndOfMarker(DataStreamBuffer, StartOfMessageMarkerLength);
        Console.WriteLine(@$"The answer to the second puzzle is {firstOccurrence}");
    }

    private static int FindEndOfMarker(string sequence, int markerLength)
    {
        var endOfMarker = markerLength;

        for (int i = markerLength; i <= sequence.Length; i++)
        {
            if (sequence[(i - markerLength)..i].Distinct().Count() == markerLength)
            {
                endOfMarker = i;
                break;
            }
        }

        return endOfMarker;
    }
}