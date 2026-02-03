using Device = (string Name, int Depth);

namespace Advent2025.Days;

public class Day11_Reactor
{
    private static readonly string[] Input = SantasLittleHelpers.ReadFileRows("2025-12-11.txt");

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
        } // 172 728 388 596 569
          // 380 961 604 031 372
        Console.WriteLine($"Answer: The total number of paths from 'you' to 'out': {total}");
    }

    public static void Puzzle2()
    {
        var devices = ParseInput();

        var depthsOfDevices = FindDepthOfDevicesDacAndFft(devices);

        var pathsToDevice = new Dictionary<string, long>();

        var firstDeviceDepth = Math.Min(depthsOfDevices["dac"], depthsOfDevices["fft"]);
        var secondDeviceDepth = Math.Max(depthsOfDevices["dac"], depthsOfDevices["fft"]);
        var firstDeviceName = firstDeviceDepth == depthsOfDevices["dac"] ? "dac" : "fft";
        var secondDeviceName = secondDeviceDepth == depthsOfDevices["dac"] ? "dac" : "fft";

        var currentDevice = "svr";
        var currentDepth = 0;

        pathsToDevice[currentDevice] = 1;
        
        var toVisit = new Stack<Device>();
        var currentPath = new List<Device> { (currentDevice, currentDepth) };

        foreach (var connection in devices[currentDevice])
        {
            toVisit.Push((connection, currentDepth + 1));

            if (pathsToDevice.ContainsKey(connection))
            {
                pathsToDevice[connection] += pathsToDevice[currentDevice];
            }
            else
            {
                pathsToDevice[connection] = pathsToDevice[currentDevice];
            }
        }

        var total = 0;
        while (toVisit.Count > 0)
        {
            (currentDevice, currentDepth) = toVisit.Pop();

            if (currentDepth == firstDeviceDepth && currentDevice != firstDeviceName)
            {
                //currentPath.RemoveAll(d => d.Depth >= toVisit.Peek().Depth);
                //continue;

                if (toVisit.TryPeek(out var nextDepth))
                {
                    currentPath.RemoveAll(d => d.Depth >= nextDepth.Depth);
                    continue;
                }

                break;
            }

            if (currentDepth == secondDeviceDepth && currentDevice != secondDeviceName)
            {
                if (toVisit.TryPeek(out var nextDepth))
                {
                    currentPath.RemoveAll(d => d.Depth >= nextDepth.Depth);
                    continue;
                }

                break;
            }
            
            if (currentPath.Any(d => d.Depth >= currentDepth))
            {
                currentPath.RemoveAll(d => d.Depth >= currentDepth);
            }
            currentPath.Add((currentDevice, currentDepth));

            foreach (var device in currentPath)
            {
                Console.Write($"{device.Name}:{device.Depth},");
            }
            Console.WriteLine();

            if (currentDevice == "fft")
            {
                if (currentPath.Any(d => d.Name == "fft") && currentPath.Any(d => d.Name == "dac"))
                {
                    total++;
                }

                currentPath.Remove((currentDevice, currentDepth));
                continue;
            }

            if (devices[currentDevice].Contains(firstDeviceName))
            {
                toVisit.Push((firstDeviceName, currentDepth + 1));

                if (pathsToDevice.ContainsKey(firstDeviceName))
                {
                    pathsToDevice[firstDeviceName] += pathsToDevice[currentDevice];
                }
                else
                {
                    pathsToDevice[firstDeviceName] = pathsToDevice[currentDevice];
                }
            }
            else if (devices[currentDevice].Contains(secondDeviceName))
            {
                toVisit.Push((secondDeviceName, currentDepth + 1));

                if (pathsToDevice.ContainsKey(secondDeviceName))
                {
                    pathsToDevice[secondDeviceName] += pathsToDevice[currentDevice];
                }
                else
                {
                    pathsToDevice[secondDeviceName] = pathsToDevice[currentDevice];
                }
            }
            else
            {
                foreach (var connection in devices[currentDevice])
                {
                    toVisit.Push((connection, currentDepth + 1));

                    if (pathsToDevice.ContainsKey(connection))
                    {
                        pathsToDevice[connection] += pathsToDevice[currentDevice];
                    }
                    else
                    {
                        pathsToDevice[connection] = pathsToDevice[currentDevice];
                    }
                }
            }
        }

        Console.WriteLine($"Answer: The total number of paths from 'svr' to 'out': {total}");
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

    private static Dictionary<string, int> FindDepthOfDevicesDacAndFft(Dictionary<string, List<string>> devices)
    {
        var depths = new Dictionary<string, int>();

        var currentDevice = "svr";
        var currentDepth = 0;

        depths[currentDevice] = currentDepth;

        var toVisit = new Queue<Device>();

        foreach (var connection in devices[currentDevice])
        {
            toVisit.Enqueue((connection, currentDepth + 1));
        }

        while (toVisit.Count > 0)
        {
            (currentDevice, currentDepth) = toVisit.Dequeue();

            if (!depths.TryAdd(currentDevice, currentDepth))
            {
                continue;
            }

            //Console.WriteLine($"{currentDevice}:{currentDepth}");

            if (depths.ContainsKey("dac") && depths.ContainsKey("fft"))
            {
                break;
            }

            if (currentDevice == "out")
            {
                continue;
            }

            foreach (var connection in devices[currentDevice])
            {
                toVisit.Enqueue((connection, currentDepth + 1));
            }
        }

        return depths;
    }
}