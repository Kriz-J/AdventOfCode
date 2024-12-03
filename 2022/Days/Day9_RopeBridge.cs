namespace Days;

public class Day9_RopeBridge
{
    private static readonly string[] SeriesOfMotion = File.ReadAllLines(@"..\..\..\Resources\Day9_Puzzle_Input.txt");

    private const int NumberOfKnots = 10;
    private static readonly int[,] KnotPositions = new int[NumberOfKnots, 2];
    private static readonly HashSet<string>[] UniquePositionsOfKnots = new HashSet<string>[NumberOfKnots];

    public static void Part1And2()
    {
        InitializeKnotPositions(KnotPositions);

        for (int i = 0; i < NumberOfKnots; i++)
        {
            UniquePositionsOfKnots[i] = new HashSet<string>();
        }

        foreach (var motion in SeriesOfMotion)
        {
            var direction = motion.Split(' ')[0];
            if (!int.TryParse(motion.Split(' ')[1], out var steps))
                throw new Exception($"Can't parse number of steps in direction '{direction}'");

            while (steps-- > 0)
            {
                switch (direction)
                {
                    case "R":
                        KnotPositions[0, 0]++;
                        break;
                    case "L":
                        KnotPositions[0, 0]--;
                        break;
                    case "U":
                        KnotPositions[0, 1]++;
                        break;
                    case "D":
                        KnotPositions[0, 1]--;
                        break;
                    default:
                        throw new Exception("Direction not supported.");
                }
                UniquePositionsOfKnots[0].Add($"{KnotPositions[0, 0]},{KnotPositions[0, 1]}");

                for (int i = 0; i < NumberOfKnots - 1; i++)
                {
                    CalculateKnotPosition(i, i + 1);
                }
            }
        }

        Console.WriteLine(@$"The answer to the first puzzle is: {UniquePositionsOfKnots[1].Count}");
        Console.WriteLine(@$"The answer to the second puzzle is: {UniquePositionsOfKnots[9].Count}");
    }

    private static void InitializeKnotPositions(int[,] positions)
    {
        for (int i = 0; i < positions.GetLength(0); i++)
        {
            for (int j = 0; j < positions.GetLength(1); j++)
            {
                positions[i, j] = 0;
            }
        }
    }

    private static void CalculateKnotPosition(int leadingKnot, int trailingKnot)
    {
        var xLeading = KnotPositions[leadingKnot, 0];
        var yLeading = KnotPositions[leadingKnot, 1];

        var xTrailing = KnotPositions[trailingKnot, 0];
        var yTrailing = KnotPositions[trailingKnot, 1];

        if (Math.Abs(xLeading - xTrailing) > 0 && Math.Abs(yLeading - yTrailing) > 1 ||
            Math.Abs(xLeading - xTrailing) > 1 && Math.Abs(yLeading - yTrailing) > 0)
        {
            if (xLeading > xTrailing)
                xTrailing++;
            if (xLeading < xTrailing)
                xTrailing--;

            if (yLeading > yTrailing)
                yTrailing++;
            if (yLeading < yTrailing)
                yTrailing--;
        }

        if (Math.Abs(xLeading - xTrailing) > 1)
        {
            if (xLeading > xTrailing)
                xTrailing++;
            if (xLeading < xTrailing)
                xTrailing--;
        }

        if (Math.Abs(yLeading - yTrailing) > 1)
        {
            if (yLeading > yTrailing)
                yTrailing++;
            if (yLeading < yTrailing)
                yTrailing--;
        }

        KnotPositions[leadingKnot, 0] = xLeading;
        KnotPositions[leadingKnot, 1] = yLeading;

        KnotPositions[trailingKnot, 0] = xTrailing;
        KnotPositions[trailingKnot, 1] = yTrailing;

        UniquePositionsOfKnots[trailingKnot].Add($"{xTrailing},{yTrailing}");
    }
}