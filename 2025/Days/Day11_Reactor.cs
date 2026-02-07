namespace Advent2025.Days;

public class Day11_Reactor
{
    private static readonly string[] Input = SantasLittleHelpers.ReadFileRows("2025-12-11.txt");

    private static readonly Dictionary<(string, bool, bool), long> DevicePathCache = [];

    public static void Puzzle1()
    {
        var devices = ParseInput();

        var current = "you";
        var toVisit = new Stack<string>();

        foreach (var connection in devices[current])
        {
            toVisit.Push(connection);
        }

        var total = 0;
        while (toVisit.Count > 0)
        {
            current = toVisit.Pop();

            if (current == "out")
            {
                total++;
                continue;
            }

            foreach (var connection in devices[current])
            {
                toVisit.Push(connection);
            }
        }

        Console.WriteLine($"Answer: The total number of paths from 'you' to 'out': {total}");
    }

    public static void Puzzle2()
    {
        var devices = ParseInput();

        var total = PathsFromDeviceThroughFftAndDac("svr", devices);

        Console.WriteLine($"Answer: The total number of paths from 'svr' to 'out' through 'fft' and 'dac': {total}");
    }

    private static long PathsFromDeviceThroughFftAndDac(string device, Dictionary<string, List<string>> devices, bool passedFft = false, bool passedDac = false)
    {
        passedFft = passedFft || device == "fft";
        passedDac = passedDac || device == "dac";

        if (device == "out")
        {
            return (passedFft && passedDac) ? 1 : 0;
        }

        if (DevicePathCache.TryGetValue((device, passedFft, passedDac), out var cachedValue))
        {
            return cachedValue;
        }

        var paths = devices[device].Sum(connectedDevice => PathsFromDeviceThroughFftAndDac(connectedDevice, devices, passedFft, passedDac));

        DevicePathCache[(device, passedFft, passedDac)] = paths;

        return paths;
    }

    private static Dictionary<string, List<string>> ParseInput()
    {
        var devices = new Dictionary<string, List<string>>();

        foreach (var row in Input)
        {
            var deviceNameEnd = row.IndexOf(':');

            devices[row[..deviceNameEnd]] = [];

            var connections = row[(deviceNameEnd + 2)..].Split(' ');
            foreach (var connection in connections)
            {
                devices[row[..deviceNameEnd]].Add(connection);
            }
        }

        return devices;
    }
}