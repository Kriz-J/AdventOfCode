using Advent2024;

var days = SantasLittleHelpers.GetAvailableDays();
if (!days.Any())
{
    Console.WriteLine("No days available to run.");
    return;
}

SantasLittleHelpers.DisplayOptions("Select day:", days.Select(d => d.Name), out var cursorPosition);
Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
var selectedDay = SantasLittleHelpers.ParseAndValidateSelection(upperBound: days.Count);
if (selectedDay == -1) return;

Console.SetCursorPosition(0, cursorPosition.Top + days.Count + 2);

var day = days[selectedDay];
var puzzles = SantasLittleHelpers.GetAvailablePuzzles(day);
if (!puzzles.Any())
{
    Console.WriteLine("No puzzles available for this day.");
    return;
}

SantasLittleHelpers.DisplayOptions($"Select puzzle from {day.Name}:", puzzles.Select(p => p.Name), out cursorPosition);
Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
var selectedPuzzle = SantasLittleHelpers.ParseAndValidateSelection(upperBound: puzzles.Count);
if (selectedPuzzle == -1) return;

Console.SetCursorPosition(0, cursorPosition.Top + puzzles.Count + 2);

puzzles[selectedPuzzle].Invoke(null, null);