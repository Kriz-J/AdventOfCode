using Position = (int x, int y);

namespace Advent2025.Days;

public static class Day09_MovieTheater
{
    private static readonly string[] Input = SantasLittleHelpers.ReadFileRows("2025-12-09.txt");

    public static void Puzzle1()
    {
        var vertices = ParseVertices(Input);

        var largestArea = 0L;

        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                var origin = vertices[i];
                var oppositeCorner = vertices[j];

                var area = (1L + Math.Abs(origin.x - oppositeCorner.x)) * (1L + Math.Abs(origin.y - oppositeCorner.y));

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
        var vertices = ParseVertices(Input);
        var compressedVertices = CompressVertices(vertices);
        var compressedBorder = ConstructCompressedBorder(compressedVertices);
        var compressedFill = FillInterior(compressedBorder);
        var compressedPolygon = new HashSet<Position>(compressedBorder);
        compressedPolygon.UnionWith(compressedFill);

        var largestArea = 0L;
        for (var i = 0; i < vertices.Length; i++)
        {
            for (var j = i + 1; j < vertices.Length; j++)
            {
                var origin = vertices[i];
                var oppositeCorner = vertices[j];

                var area = (1L + Math.Abs(origin.x - oppositeCorner.x)) * (1L + Math.Abs(origin.y - oppositeCorner.y));

                var (ne, nw, sw, se) = FindCornersOfRectangle(compressedVertices[i], compressedVertices[j]);

                if (area > largestArea && IsValidRectangle(ne, nw, sw, se, compressedPolygon))
                {
                    largestArea = area;
                }
            }
        }

        Console.WriteLine($"Answer: The largest area is {largestArea}.");
    }

    private static Position[] ParseVertices(string[] input)
    {
        var vertices = new Position[input.Length];

        for (var i = 0; i < input.Length; i++)
        {
            var parts = input[i].Split(',');

            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);

            vertices[i] = new Position(x, y);
        }

        return vertices;
    }

    private static Position[] CompressVertices(Position[] vertices)
    {
        var uniqueX = vertices.Select(p => p.x).Distinct().Order().ToArray();
        var uniqueY = vertices.Select(p => p.y).Distinct().Order().ToArray();

        var compressedX = new Dictionary<int, int>();
        for (var i = 0; i < uniqueX.Length; i++)
        {
            compressedX[uniqueX[i]] = i;
        }

        var compressedY = new Dictionary<int, int>();
        for (var i = 0; i < uniqueY.Length; i++)
        {
            compressedY[uniqueY[i]] = i;
        }

        var compressedVertices = new Position[vertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            compressedVertices[i] = new Position(compressedX[vertices[i].x], compressedY[vertices[i].y]);
        }

        return compressedVertices;
    }

    private static HashSet<Position> ConstructCompressedBorder(Position[] compressedVertices)
    {
        var compressedBorder = new HashSet<Position>();

        var inputLength = compressedVertices.Length;
        for (var i = 0; i < inputLength; i++)
        {
            var current = compressedVertices[i];
            var next = compressedVertices[(i + 1) % inputLength];

            var dx = next.x - current.x;
            var dy = next.y - current.y;

            var xCoordinates = dx > 0 ? Enumerable.Range(current.x, 1 + dx) : Enumerable.Range(next.x, 1 + Math.Abs(dx)).Reverse(); // right:left
            var yCoordinates = dy > 0 ? Enumerable.Range(current.y, 1 + dy) : Enumerable.Range(next.y, 1 + Math.Abs(dy)).Reverse(); // down:up

            var borderPositionsX = xCoordinates.Select(x => new Position(x, current.y)).ToHashSet();
            var borderPositionsY = yCoordinates.Select(y => new Position(current.x, y)).ToHashSet();

            compressedBorder.UnionWith(borderPositionsX);
            compressedBorder.UnionWith(borderPositionsY);
        }

        return compressedBorder;
    }

    private static HashSet<Position> FillInterior(HashSet<Position> border) // Alternative to flood fill which doesn't require a known point inside - Shoots rays to center
    {
        var fill = new HashSet<Position>();

        var borderList = border.ToArray();
        var borderListLength = borderList.Length;

        for (var i = 0; i < borderListLength; i++)
        {
            var current = borderList[i];
            var next = borderList[(i + 1) % borderListLength];

            var dx = next.x - current.x;
            var dy = next.y - current.y;

            if (dx == 0)
            {
                var direction = dy > 0 ? -1 : 1; // right:left

                while (true)
                {
                    current.x += direction;
                    if (border.Contains(current))
                    {
                        break;
                    }
                    fill.Add(current);
                }
            }
            else if (dy == 0)
            {
                var direction = dx > 0 ? 1 : -1; // down:up

                while (true)
                {
                    current.y += direction;
                    if (border.Contains(current))
                    {
                        break;
                    }
                    fill.Add(current);
                }
            }
        }

        return fill;
    }

    private static (Position NorthEast, Position NorthWest, Position SouthWest, Position SouthEast) FindCornersOfRectangle(Position origin, Position oppositeCorner)
    {
        Position ne = (0, 0);
        Position nw = (0, 0);
        Position sw = (0, 0);
        Position se = (0, 0);

        if (IsFirstQuadrant(origin, oppositeCorner))
        {
            ne = oppositeCorner;
            nw = new Position(origin.x, oppositeCorner.y);
            sw = origin;
            se = new Position(oppositeCorner.x, origin.y);
        }
        else if (IsSecondQuadrant(origin, oppositeCorner))
        {
            ne = new Position(origin.x, oppositeCorner.y);
            nw = oppositeCorner;
            sw = new Position(oppositeCorner.x, origin.y);
            se = origin;
        }
        else if (IsThirdQuadrant(origin, oppositeCorner))
        {
            ne = origin;
            nw = new Position(oppositeCorner.x, origin.y);
            sw = oppositeCorner;
            se = new Position(origin.x, oppositeCorner.y);
        }
        else if (IsFourthQuadrant(origin, oppositeCorner))
        {
            ne = new Position(oppositeCorner.x, origin.y);
            nw = origin;
            sw = new Position(origin.x, oppositeCorner.y);
            se = oppositeCorner;
        }

        return (NorthEast: ne, NorthWest: nw, SouthWest: sw, SouthEast: se);
    }

    private static bool IsFirstQuadrant(Position origin, Position position) => origin.x <= position.x && origin.y >= position.y;

    private static bool IsSecondQuadrant(Position origin, Position position) => origin.x >= position.x && origin.y >= position.y;

    private static bool IsThirdQuadrant(Position origin, Position position) => origin.x >= position.x && origin.y <= position.y;

    private static bool IsFourthQuadrant(Position origin, Position position) => origin.x <= position.x && origin.y <= position.y;

    private static bool IsValidRectangle(Position ne, Position nw, Position sw, Position se, HashSet<Position> polygon)
    {
        return IsAllPointsOnEdgeInPolygon(ne, nw, -1, 0, polygon) &&
               IsAllPointsOnEdgeInPolygon(nw, sw, 0, 1, polygon) &&
               IsAllPointsOnEdgeInPolygon(sw, se, 1, 0, polygon) &&
               IsAllPointsOnEdgeInPolygon(se, ne, 0, -1, polygon);
    }

    private static bool IsAllPointsOnEdgeInPolygon(Position start, Position end, int dx, int dy, HashSet<Position> polygon)
    {
        var current = start;

        while (current != end)
        {
            if (!polygon.Contains(current))
            {
                return false;
            }

            current = new Position(current.x + dx, current.y + dy);
        }

        return true;
    }
}