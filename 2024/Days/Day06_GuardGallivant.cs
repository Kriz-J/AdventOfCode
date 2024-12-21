namespace Advent2024.Days;

public class Day06_GuardGallivant
{
    public static string[] Input { get; set; } = SantasLittleHelpers.ReadFileRows("2024-12-06.txt");

    public static void Puzzle1()
    {
        var (row, col) = (0, 0);
        for (var i = 0; i < Input.Length; i++)
        {
            for (var j = 0; j < Input[i].Length; j++)
            {
                if (Input[i][j] == '^')
                {
                    (row, col) = (i, j);
                }
            }
        }

        var positions = new HashSet<(int Row, int Column)> { (row, col) };
        while (true)
        {
            while (row > 0 && Input[row - 1][col] is not '#')
            {
                row--;
                positions.Add((row, col));
            }

            if (!InBounds(row, col)) break;

            while (col < Input[row].Length - 1 && Input[row][col + 1] is not '#')
            {
                col++;
                positions.Add((row, col));
            }

            if (!InBounds(row, col)) break;

            while (row < Input.Length - 1 && Input[row + 1][col] is not '#')
            {
                row++;
                positions.Add((row, col));
            }

            if (!InBounds(row, col)) break;

            while (col > 0 && Input[row][col - 1] is not '#')
            {
                col--;
                positions.Add((row, col));
            }

            if (!InBounds(row, col)) break;
        }

        Console.WriteLine($"Answer: The number of distinct positions visited is {positions.Count}");
    }

    private static bool InBounds(int row, int col)
    {
        return row >= 1 && row < Input.Length - 1 &&
               col >= 1 && col < Input[0].Length - 1;
    }


    public static void Puzzle2()
    {
        var (row, col, dir) = (0, 0, 'u');
        for (var i = 0; i < Input.Length; i++)
        {
            for (var j = 0; j < Input[i].Length; j++)
            {
                if (Input[i][j] == '^')
                {
                    (row, col) = (i, j);
                }
            }
        }

        var start = (row, col);
        var obstructions = new HashSet<(int Row, int Column)> { (row, col - 1) }; //add position to the left of start to list immediately
        var positions = new HashSet<(int Row, int Column, char Direction)> { (row, col, dir) };

        (row, col) = start;
        while (true)
        {
            while (row > 0 && Input[row - 1][col] is not '#')
            {
                var current = (row, col);
                positions.Add((row, col, 'u'));
                while (col < Input[row].Length - 1 && Input[row][col + 1] is not '#')
                {
                    col++;
                    if (!positions.Contains((row, col, 'r'))) continue;
                    obstructions.Add((current.row - 1, current.col));
                    break;
                }
                (row, col) = current;
                row--;
                positions.Add((row, col, 'u'));
            }

            if (!InBounds(row, col)) break;
            positions.Add((row, col, 'r'));

            while (col < Input[row].Length - 1 && Input[row][col + 1] is not '#')
            {
                var current = (row, col);
                while (row < Input.Length - 1 && Input[row + 1][col] is not '#')
                {
                    row++;
                    if (!positions.Contains((row, col, 'd'))) continue;
                    obstructions.Add((current.row, current.col + 1));
                    break;
                }
                (row, col) = current;
                col++;
                positions.Add((row, col, 'r'));
            }

            if (!InBounds(row, col)) break;
            positions.Add((row, col, 'd'));

            while (row < Input.Length - 1 && Input[row + 1][col] is not '#')
            {
                var current = (row, col);
                while (col > 0 && Input[row][col - 1] is not '#')
                {
                    col--;
                    if (!positions.Contains((row, col, 'l'))) continue;
                    obstructions.Add((current.row + 1, current.col));
                    break;
                }
                (row, col) = current;
                row++;
                positions.Add((row, col, 'd'));
            }

            if (!InBounds(row, col)) break;
            positions.Add((row, col, 'l'));

            while (col > 0 && Input[row][col - 1] is not '#')
            {
                var current = (row, col);
                while (row > 0 && Input[row - 1][col] is not '#')
                {
                    row--;
                    if (!positions.Contains((row, col, 'u'))) continue;
                    obstructions.Add((current.row, current.col - 1));
                    break;
                }
                (row, col) = current;
                col--;
                positions.Add((row, col, 'l'));
            }

            if (!InBounds(row, col)) break;
        }

        Console.WriteLine($"Answer: There are {obstructions.Count}");
    }
}

//461 too low