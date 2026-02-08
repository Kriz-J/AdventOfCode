namespace Advent2025.Days;

public class Day12_ChristmasTreeFarm
{
    private static readonly string[] Input = SantasLittleHelpers.ReadFileRows("2025-12-12.txt");

    public static void Puzzle1()
    {
        //Read shapes
        var shapes = new Dictionary<int, int[][]>();
        var shapeSizes = new Dictionary<int, int>();

        var shapesEndIndexes = Input.Select((row, index) => (row, index)).Where(r => string.IsNullOrWhiteSpace(r.row)).Select(r => r.index).ToArray();

        for (var i = 0; i < shapesEndIndexes.Length; i++)
        {
            var numberIndexEnd = Input[i * 5].IndexOf(':');

            var index = int.Parse(Input[i * 5][..numberIndexEnd]);

            var shape = new int[3][];
            shape[0] = Input[i * 5 + 1].Select(c => c == '#' ? 1 : 0).ToArray();
            shape[1] = Input[i * 5 + 2].Select(c => c == '#' ? 1 : 0).ToArray();
            shape[2] = Input[i * 5 + 3].Select(c => c == '#' ? 1 : 0).ToArray();

            shapes[index] = shape;
            shapeSizes[index] = shape.Sum(r => r.Sum(c => c));
        }

        //Read instructions
        var regions = new List<(int Width, int Length, int[] Quantities)>();

        for (var i = shapesEndIndexes.Max() + 1; i < Input.Length; i++)
        {
            var widthEndPosition = Input[i].IndexOf('x');
            var lengthEndPosition = Input[i].IndexOf(':');

            var width = int.Parse(Input[i][..widthEndPosition]);
            var length = int.Parse(Input[i][(widthEndPosition + 1)..lengthEndPosition]);

            var quantitiesStartPosition = Input[i].IndexOf(':') + 2;
            var quantities = Input[i][quantitiesStartPosition..].Split(' ').Select(int.Parse).ToArray();

            regions.Add((width, length, quantities));
        }

        var total = 0;
        foreach (var region in regions)
        {
            var size = region.Width * region.Length;

            var sizeOfPresents = region.Quantities.Select((q, i) => q * shapeSizes[i]).Sum();

            if (sizeOfPresents <= size) // If sum of the sizes doesn't even fit, then it's impossible any solution does.
            {
                total++;
            }
        }

        Console.WriteLine($"Answer: The number of regions which can fit the listed quantities of presents is: {total}");
    }

    public static void Puzzle2()
    {
        // Free star!
    }
}