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

        foreach (var position in junctionBoxPositions)
        {
            Circuits.Add(position, [position]);
        }
        
        for (int i = 0; i < junctionBoxPositions.Length; i++)
        {
            var shortestConnection= JunctionBoxDistances.Where(kvp => kvp.Value > 0).MinBy(kvp => kvp.Value);

            var p1 = shortestConnection.Key.P1;
            var p2 = shortestConnection.Key.P2;

            if (Circuits.ContainsKey(p1) && Circuits.ContainsKey(p2)) // P1 & P2 are both circuit-key (both can contain multiple junction boxes)
            {
                foreach (var position in Circuits[p2])
                {
                    Circuits[p1].Add(position);
                    Location[position] = p1;
                }
                Circuits.Remove(p2);
            }
            else if (Circuits.ContainsKey(p1) && Location.TryGetValue(p2, out var location1)) // P1 is circuit-key and P2 has location
            {
                if (p1 == location1)
                {
                    JunctionBoxDistances.Remove((p1, p2));
                    continue;
                }

                foreach (var position in Circuits[location1])
                {
                    Circuits[p1].Add(position);
                    Location[position] = p1;
                }
                Circuits.Remove(location1);
            }
            else if (Location.TryGetValue(p1, out var location2) && Circuits.ContainsKey(p2)) // P1 has location and P2 is circuit-key
            {
                if (location2 == p2)
                {
                    JunctionBoxDistances.Remove((p1, p2));
                    continue;
                }

                foreach (var position in Circuits[p2])
                {
                    Circuits[location2].Add(position);
                    Location[position] = location2;
                }
                Circuits.Remove(p2);
            }
            else if (Location.TryGetValue(p1, out var location3) && Location.TryGetValue(p2, out var location4)) // P1 has location and P2 has location
            {
                if (location3 == location4)
                {
                    JunctionBoxDistances.Remove((p1, p2));
                    continue;
                }

                foreach (var position in Circuits[location4])
                {
                    Circuits[location3].Add(position);
                    Location[position] = location3;
                }
                Circuits.Remove(location4);
            }

            JunctionBoxDistances.Remove((p1, p2)); // Mirror is never added initially
        }

        var circuitSize = Circuits.Values.Select(s => s.Count).OrderByDescending(s => s).ToList().Take(3).Aggregate(1, (current, count) => current * count);
        
        Console.WriteLine($"Answer: The three largest circuits produce {circuitSize}.");
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

                JunctionBoxDistances.TryAdd((p1, p2), SquaredEuclideanDistance(p1, p2));
            }
        }
    }
    
    private static long SquaredEuclideanDistance(Position p1, Position p2) // Will work just as well without Math.Sqrt for our usage
    {
        return (long)(p1.x - p2.x) * (p1.x - p2.x) + (long)(p1.y - p2.y) * (p1.y - p2.y) + (long)(p1.z - p2.z) * (p1.z - p2.z);
    }

    public static void Puzzle2()
    {
    }
}