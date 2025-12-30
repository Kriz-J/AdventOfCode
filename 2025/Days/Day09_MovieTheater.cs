using Position = (int x, int y);

namespace Advent2025.Days;

public static class Day09_MovieTheater
{
    private static readonly string[] Input = SantasLittleHelpers.ReadFileRows("2025-12-09.txt");

    public static void Puzzle1()
    {
        var coordinates = new Position[Input.Length];

        for (int i = 0; i < Input.Length; i++)
        {
            var parts = Input[i].Split(',');

            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);

            coordinates[i] = new Position(x, y);
        }

        var largestArea = 0L;
        var area = 0L;

        for (int i = 0; i < coordinates.Length; i++)
        {
            for (int j = i + 1; j < coordinates.Length; j++)
            {
                if (IsFirstQuadrant(coordinates[i], coordinates[j]))
                {
                    area = (1L + coordinates[j].x - coordinates[i].x) * (1L + coordinates[j].y - coordinates[i].y);
                }

                if (IsSecondQuadrant(coordinates[i], coordinates[j]))
                {
                    area = (1L + coordinates[i].x - coordinates[j].x) * (1L + coordinates[j].y - coordinates[i].y);
                }

                if (IsThirdQuadrant(coordinates[i], coordinates[j]))
                {
                    area = (1L + coordinates[i].x - coordinates[j].x) * (1L + coordinates[i].y - coordinates[j].y);
                }

                if (IsFourthQuadrant(coordinates[i], coordinates[j]))
                {
                    area = (1L + coordinates[j].x - coordinates[i].x) * (1L + coordinates[i].y - coordinates[j].y);
                }

                if (area > largestArea)
                {
                    largestArea = area;
                }
            }
        }

        Console.WriteLine($"Answer: The largest area is {largestArea}.");
    }

    public static void Puzzle2()
    {
        var coordinates = new Position[Input.Length];

        for (int i = 0; i < Input.Length; i++)
        {
            var parts = Input[i].Split(',');

            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);

            coordinates[i] = new Position(x, y);
        }

        var listCoordinates = coordinates.ToList();

        var largestArea = 0L;
        var area = 0L;
        Position ne = (0, 0);
        Position nw = (0, 0);
        Position sw = (0, 0);
        Position se = (0, 0);

        for (int i = 0; i < coordinates.Length; i++)
        {
            for (int j = i + 1; j < coordinates.Length; j++)
            {
                if (IsFirstQuadrant(coordinates[i], coordinates[j]))
                {
                    area = (1L + coordinates[j].x - coordinates[i].x) * (1L + coordinates[i].y - coordinates[j].y);

                    ne = coordinates[j];
                    nw = new Position(coordinates[i].x, coordinates[j].y);
                    sw = coordinates[i];
                    se = new Position(coordinates[j].x, coordinates[i].y);
                }

                else if (IsSecondQuadrant(coordinates[i], coordinates[j]))
                {
                    area = (1L + coordinates[i].x - coordinates[j].x) * (1L + coordinates[i].y - coordinates[j].y);

                    ne = new Position(coordinates[i].x, coordinates[j].y);
                    nw = coordinates[j];
                    sw = new Position(coordinates[j].x, coordinates[i].y);
                    se = coordinates[i];
                }

                else if (IsThirdQuadrant(coordinates[i], coordinates[j]))
                {
                    area = (1L + coordinates[i].x - coordinates[j].x) * (1L + coordinates[j].y - coordinates[i].y);

                    ne = coordinates[i];
                    nw = new Position(coordinates[j].x, coordinates[i].y);
                    sw = coordinates[j];
                    se = new Position(coordinates[i].x, coordinates[j].y);
                }

                else if (IsFourthQuadrant(coordinates[i], coordinates[j]))
                {
                    area = (1L + coordinates[j].x - coordinates[i].x) * (1L + coordinates[j].y - coordinates[i].y);

                    ne = new Position(coordinates[j].x, coordinates[i].y);
                    nw = coordinates[i];
                    sw = new Position(coordinates[i].x, coordinates[j].y);
                    se = coordinates[j];
                }

                if (listCoordinates.Any(c => c.x >= ne.x && c.y <= ne.y) &&
                    listCoordinates.Any(c => c.x <= nw.x && c.y <= nw.y) &&
                    listCoordinates.Any(c => c.x <= sw.x && c.y >= sw.y) &&
                    listCoordinates.Any(c => c.x >= se.x && c.y >= se.y))
                {
                    if (area > largestArea)
                    {
                        largestArea = area;
                    }
                }
            }
        }

        Console.WriteLine($"Answer: The largest area is {largestArea}.");
    }

    private static bool IsFirstQuadrant(Position origin, Position position) => origin.x <= position.x && origin.y >= position.y;

    private static bool IsSecondQuadrant(Position origin, Position position) => origin.x >= position.x && origin.y >= position.y;

    private static bool IsThirdQuadrant(Position origin, Position position) => origin.x >= position.x && origin.y <= position.y;

    private static bool IsFourthQuadrant(Position origin, Position position) => origin.x <= position.x && origin.y <= position.y;
}

//4 584 201 744 HIGH
//4 629 504 600 HIGH