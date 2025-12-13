namespace Advent2025.Days;

public class Day02_GiftShop
{
    public static string Input { get; set; } = SantasLittleHelpers.ReadFile("2025-12-02.txt");

    public static void Puzzle1()
    {
        var ranges = Input.Split(',');

        long invalidIdsSum = 0;

        foreach (var range in ranges)
        {
            var start = long.Parse(range.Split('-')[0]);
            var end = long.Parse(range.Split('-')[1]);

            for (var i = start; i <= end; i++)
            {
                var id = i.ToString();
                var mid = id.Length / 2;

                if (id[..mid] == id[mid..])
                {
                    invalidIdsSum += i;
                }
            }
        }

        Console.WriteLine($"Answer: The sum of invalid IDs is {invalidIdsSum}.");
    }

    public static void Puzzle2()
    {
        var ranges = Input.Split(',');

        long invalidIdsSum = 0;

        foreach (var range in ranges)
        {
            var start = long.Parse(range.Split('-')[0]);
            var end = long.Parse(range.Split('-')[1]);

            for (var i = start; i <= end; i++)
            {
                var id = i.ToString();
                var idLength = id.Length;

                List<int> validDenominators = [];
                for (var j = idLength; j > 1; j--)
                {
                    if (idLength % j == 0)
                    {
                        validDenominators.Add(j);
                    }
                }

                foreach (var denominator in validDenominators)
                {
                    var addId = true;

                    for (var j = 0; j < denominator - 1; j++)
                    {
                        var stepSize = idLength / denominator;

                        if (id.Substring(j * stepSize, stepSize) != id.Substring((j + 1) * stepSize, stepSize))
                        {
                            addId = false;
                            break;
                        }
                    }

                    if (addId)
                    {
                        invalidIdsSum += i;
                        break;
                    }
                }
            }

        }

        Console.WriteLine($"Answer: The sum of invalid IDs is {invalidIdsSum}.");
    }
}