using Position = (int x, int y, int z);

namespace Advent2025.Days;

public static class Day08_Playground
{
    private static readonly string[] Input = SantasLittleHelpers.ReadFileRows("2025-12-08.txt");

    private static readonly Dictionary<(Position P1, Position P2), long> JunctionBoxDistances = [];

    private static readonly Dictionary<Position, HashSet<Position>> Circuits = [];

    private static readonly Dictionary<Position, Position> Location = [];

    public static void Puzzle1()
    {
        var junctionBoxPositions = ParseJunctionBoxPositions();

        FindDistances(junctionBoxPositions);

        var OrderedJunctionBoxDistances = JunctionBoxDistances.OrderBy(kvp => kvp.Value).ToDictionary();

        foreach (var position in junctionBoxPositions)
        {
            Circuits.Add(position, [position]);
        }

        for (int i = 0; i < junctionBoxPositions.Length; i++)
        {
            var shortestConnection = OrderedJunctionBoxDistances.First();
            OrderedJunctionBoxDistances.Remove(shortestConnection.Key);

            var junctionBox1 = shortestConnection.Key.P1;
            var junctionBox2 = shortestConnection.Key.P2;

            GroupJunctionBoxes(junctionBox1, junctionBox2);
        }

        var circuitSize = Circuits.Values
            .Select(s => s.Count)
            .OrderByDescending(s => s)
            .Take(3)
            .Aggregate(1, (current, count) => current * count);

        Console.WriteLine($"Answer: The three largest circuits produce {circuitSize}.");
    }

    public static void Puzzle2()
    {
        var junctionBoxPositions = ParseJunctionBoxPositions();

        FindDistances(junctionBoxPositions);

        var OrderedJunctionBoxDistances = JunctionBoxDistances.OrderBy(kvp => kvp.Value).ToDictionary();

        foreach (var position in junctionBoxPositions)
        {
            Circuits.Add(position, [position]);
        }

        KeyValuePair<(Position P1, Position P2), long> shortestConnection;
        do
        {
            shortestConnection = OrderedJunctionBoxDistances.First();
            OrderedJunctionBoxDistances.Remove(shortestConnection.Key);

            var junctionBox1 = shortestConnection.Key.P1;
            var junctionBox2 = shortestConnection.Key.P2;

            GroupJunctionBoxes(junctionBox1, junctionBox2);

        } while (Circuits.Count > 1);

        var distanceFromWall = (long)shortestConnection.Key.P1.x * shortestConnection.Key.P2.x;

        Console.WriteLine($"Answer: Distance from wall {distanceFromWall}.");
    }

    private static Position[] ParseJunctionBoxPositions()
    {
        var junctionBoxes = new Position[Input.Length];

        for (var i = 0; i < Input.Length; i++)
        {
            var coordinates = Input[i].Split(',');

            var x = int.Parse(coordinates[0]);
            var y = int.Parse(coordinates[1]);
            var z = int.Parse(coordinates[2]);

            junctionBoxes[i] = new Position(x, y, z);
        }

        return junctionBoxes;
    }

    private static void FindDistances(Position[] junctionBoxPositions)
    {
        for (var i = 0; i < junctionBoxPositions.Length; i++)
        {
            var p1 = junctionBoxPositions[i];

            for (var j = i + 1; j < junctionBoxPositions.Length; j++)
            {
                var p2 = junctionBoxPositions[j];

                JunctionBoxDistances.Add((p1, p2), SquaredEuclideanDistance(p1, p2)); // Mirror distance is never added
            }
        }
    }

    private static long SquaredEuclideanDistance(Position p1, Position p2) // Will work just as well without Math.Sqrt for our usage
    {
        return (long)(p1.x - p2.x) * (p1.x - p2.x) + (long)(p1.y - p2.y) * (p1.y - p2.y) + (long)(p1.z - p2.z) * (p1.z - p2.z);
    }

    private static void GroupJunctionBoxes(Position p1, Position p2)
    {
        if (Circuits.ContainsKey(p1) && Circuits.ContainsKey(p2)) // P1 & P2 are both circuit-key (both can contain multiple junction boxes)
        {
            MergeCircuits(p1, p2);
        }
        else if (Circuits.ContainsKey(p1) && Location.TryGetValue(p2, out var locationOfP2)) // P1 is circuit-key and P2 has location
        {
            if (p1 != locationOfP2)
            {
                MergeCircuits(p1, locationOfP2);
            }
        }
        else if (Location.TryGetValue(p1, out var locationOfP1) && Circuits.ContainsKey(p2)) // P1 has location and P2 is circuit-key
        {
            if (locationOfP1 != p2)
            {
                MergeCircuits(locationOfP1, p2);
            }
        }
        else if (Location.TryGetValue(p1, out locationOfP1) && Location.TryGetValue(p2, out locationOfP2)) // P1 has location and P2 has location
        {
            if (locationOfP1 != locationOfP2)
            {
                MergeCircuits(locationOfP1, locationOfP2);
            }
        }
    }

    private static void MergeCircuits(Position main, Position source)
    {
        foreach (var junctionBox in Circuits[source])
        {
            Circuits[main].Add(junctionBox);
            Location[junctionBox] = main;
        }
        Circuits.Remove(source);
    }
}