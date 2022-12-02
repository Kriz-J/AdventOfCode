using System.Reflection;

// get available classes (days in december)
var days = Assembly.GetExecutingAssembly().GetTypes().Where(c => c.Namespace is not null && c.IsClass && !c.IsNotPublic && !c.IsNested).ToList();

Console.WriteLine(@$"Select which day to run (1-{days.Count}):");

foreach (var (index, @class) in days.Select((value, i) => (i, value)))
{
    Console.WriteLine(@$"{index + 1}: {@class.Name}");
}

var daySelection = ParseAndValidateSelection(days.Count);
if (daySelection == -1) return;

Console.WriteLine();

var classFullName = days[daySelection].FullName;
if (classFullName == null) return;

var classType = Type.GetType(classFullName);
if (classType == null) return;

// get available methods (puzzles) of that class (day)
var puzzles = classType.GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();

Console.WriteLine(@$"Select which puzzle (1-{puzzles.Count}):");

foreach (var (index, method) in puzzles.Select((value, i) => (i, value)))
{
    Console.WriteLine(@$"{index + 1}: {method.Name}");
}

var puzzleSelection = ParseAndValidateSelection(puzzles.Count);
if (puzzleSelection == -1) return;

Console.WriteLine();

puzzles[puzzleSelection].Invoke(null, null);

Console.WriteLine();


int ParseAndValidateSelection(int upperBound)
{
    if (int.TryParse(Console.ReadLine(), out var selection))
    {
        if (selection > 0 && selection <= upperBound) return (selection - 1);
    }

    Console.WriteLine(@"Not valid input. Exiting.");
    return -1;
}