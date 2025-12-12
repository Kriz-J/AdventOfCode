using System.Reflection;

namespace Advent2025;

public static class SantasLittleHelpers
{
    public static string ReadFile(string file)
    {
        var path = $@"..\..\..\Input\{file}";

        try
        {
            using StreamReader reader = new(path);

            return reader.ReadToEnd();
        }
        catch (IOException e)
        {
            Console.WriteLine($"The file {path} could not be read.");
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public static string[] ReadFileRows(string file)
    {
        var path = $@"..\..\..\Input\{file}";

        try
        {
            return File.ReadAllLines(path);
        }
        catch (IOException e)
        {
            Console.WriteLine($"The file {path} could not be read.");
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public static List<Type> GetAvailableDays()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(c => c.Name.Contains("Day"))
            .ToList();
    }

    public static void DisplayOptions(string message, IEnumerable<string> options, out (int Left, int Top) cursorPosition)
    {
        Console.Write ($"{message} ");
        cursorPosition = Console.GetCursorPosition();
        Console.WriteLine();

        foreach (var (index, name) in options.Select((value, i) => (i, value)))
        {
            Console.WriteLine($"{index + 1}: {name}");
        }
    }

    public static int ParseAndValidateSelection(int upperBound)
    {
        if (int.TryParse(Console.ReadLine(), out var selection))
        {
            if (selection > 0 && selection <= upperBound)
            {
                return selection - 1;
            }
        }

        return -1;
    }

    public static List<MethodInfo> GetAvailablePuzzles(Type type)
    {
        var methodInfos = type
            .GetMethods()
            .Where(m => m.Name.Contains("Puzzle"))
            .ToList();
        return methodInfos;
    }
}