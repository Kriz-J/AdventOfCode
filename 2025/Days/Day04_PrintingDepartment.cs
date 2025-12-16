namespace Advent2025.Days;

public class Day04_PrintingDepartment
{
    public static string[] Input { get; set; } = SantasLittleHelpers.ReadFileRows("2025-12-04.txt");

    private static int Rows => Input.Length;
    private static int Columns => Input[0].Length;

    public static void Puzzle1()
    {
        var movableRolls = 0;

        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                if (Input[i][j] == '.')
                {
                    continue;
                }

                if (IsRollMovable(i, j))
                {
                    movableRolls++;
                }
            }
        }

        Console.WriteLine($"Answer: The number of movable rolls is: {movableRolls}");
    }

    public static void Puzzle2()
    {
        var removedRolls = 0;
        var startColumn = 0;

        for (var i = 0; i < Rows; i++)
        {
            for (var j = startColumn; j < Columns; j++)
            {
                if (j == Columns - 1)
                {
                    startColumn = 0;
                }

                if (Input[i][j] == '.')
                {
                    continue;
                }

                if (IsRollMovable(i, j))
                {
                    Input[i] = Input[i][..j] + '.' + Input[i][(j + 1)..];
                    removedRolls++;

                    (i, startColumn) = StepBack(i, j);
                    i--; // Offset auto increment
                    
                    break;
                }
            }
        }

        Console.WriteLine($"Answer: The number of removed rolls is: {removedRolls}");
    }

    private static bool IsRollMovable(int row, int column)
    {
        List<(int Y, int X)> directions = [(-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1)];

        var adjacentRolls = 0;

        foreach (var (y, x) in directions)
        {
            if (row + y < 0 || row + y >= Columns || column + x < 0 || column + x >= Rows)
            {
                continue;
            }

            if (Input[row + y][column + x] == '@')
            {
                adjacentRolls++;
            }

            if (adjacentRolls > 3)
            {
                return false;
            }
        }

        return true;
    }


    private static (int Row, int Column) StepBack(int row, int column)
    {
        if (row - 1 >= 0 && column - 1 >= 0) //N-W
        {
            return (row - 1, column - 1);
        }

        if (row - 1 >= 0) //North
        {
            return (row - 1, column);
        }

        if (column - 1 >= 0) //West
        {
            return (row, column - 1);
        }

        return (row, column);
    }
}