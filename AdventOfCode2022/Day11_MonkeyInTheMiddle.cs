using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day11_MonkeyInTheMiddle
{
    public static readonly string[] Notes = File.ReadAllLines(@"..\..\..\Resources\Day11_Puzzle_Input.txt");
    private class Monkey
    {
        public int Id { get; set; }
        public Queue<long> Items { get; set; } = new();
        public string Operator { get; set; }
        public int OperatorValue { get; set; }
        public int TestValue { get; set; }
        public readonly int[] Targets = new int[2];
        public int NumberOfItemInspections { get; set; }
    }

    private static readonly List<Monkey> Monkeys = new();

    public static void Part1()
    {
        ParseNotes(Notes);

        const int numberOfRounds = 20;
        const int worryRelief = 3;

        PlayKeepAway(Monkeys, numberOfRounds, worryRelief);
        
        var twoMostActiveMonkeys = Monkeys
            .OrderByDescending(i => i.NumberOfItemInspections)
            .Select(i => i.NumberOfItemInspections)
            .Take(2)
            .ToArray();

        var levelOfMonkeyBusiness = (long)twoMostActiveMonkeys[0] * twoMostActiveMonkeys[1];

        Console.WriteLine(@$"The answer to the first puzzle is: {levelOfMonkeyBusiness}");
    }

    public static void Part2()
    {
        ParseNotes(Notes);

        const int numberOfRounds = 10000;
        const int worryRelief = 0;

        PlayKeepAway(Monkeys, numberOfRounds, worryRelief);

        var twoMostActiveMonkeys = Monkeys
            .OrderByDescending(i => i.NumberOfItemInspections)
            .Select(i => i.NumberOfItemInspections)
            .Take(2)
            .ToArray();

        var levelOfMonkeyBusiness = (long)twoMostActiveMonkeys[0] * twoMostActiveMonkeys[1];

        Console.WriteLine(@$"The answer to the second puzzle is: {levelOfMonkeyBusiness}");
    }

    private static void ParseNotes(string[] notes)
    {
        var currentMonkey = 0;

        foreach (var line in Notes)
        {
            var lineWithoutPadding = line.TrimStart();

            if (lineWithoutPadding.StartsWith("Monkey"))
            {
                currentMonkey = ParseLineToInt(lineWithoutPadding, @"\d+", "Monkey is missing id.");

                Monkeys.Add(new Monkey { Id = currentMonkey });
            }
            else if (lineWithoutPadding.StartsWith("Starting items:"))
            {
                var itemsWorryLevel = Regex.Matches(lineWithoutPadding, @"\d+");

                for (int i = 0; i < itemsWorryLevel.Count; i++)
                {
                    if (!int.TryParse(itemsWorryLevel[i].Value, out var worrylevel))
                        throw new Exception("Can't parse item worry level.");

                    Monkeys[currentMonkey].Items.Enqueue(worrylevel);
                }
            }
            else if (lineWithoutPadding.StartsWith("Operation:"))
            {
                var operatorSign = Regex.Matches(lineWithoutPadding, @"[+|*]");

                if (!operatorSign.Any())
                    throw new Exception("Monkey is missing operator value.");

                Monkeys[currentMonkey].Operator = operatorSign[0].Value;

                var operatorValueMatch = lineWithoutPadding.Split(' ').Last();

                int operatorValue = 0;

                if (operatorValueMatch == "old")
                {
                    operatorValue = 0;
                }
                else
                {
                    if (!int.TryParse(operatorValueMatch, out operatorValue))
                        throw new Exception("Can't parse operator value.");
                }

                Monkeys[currentMonkey].OperatorValue = operatorValue;
            }
            else if (lineWithoutPadding.StartsWith("Test:"))
            {
                Monkeys[currentMonkey].TestValue = ParseLineToInt(lineWithoutPadding, @"\d+", "Monkey is missing test value.");
            }
            else if (lineWithoutPadding.StartsWith("If true:"))
            {
                Monkeys[currentMonkey].Targets[0] = ParseLineToInt(lineWithoutPadding, @"\d+", "Monkey is missing target if test is true.");
            }
            else if (lineWithoutPadding.StartsWith("If false:"))
            {
                Monkeys[currentMonkey].Targets[1] = ParseLineToInt(lineWithoutPadding, @"\d+", "Monkey is missing target if test is false.");
            }
        }
    }

    private static int ParseLineToInt(string line, string regex, string exceptionMessage)
    {
        if (!int.TryParse(Regex.Matches(line, regex)[0].Value, out var returnValue))
            throw new Exception(exceptionMessage);
        
        return returnValue;
    }

    private static void PlayKeepAway(List<Monkey> monkeys, int numberOfRounds, int worryRelief)
    {
        var modulus = monkeys.Aggregate(1, (current, monkey) => current * monkey.TestValue);

        while (numberOfRounds-- > 0)
        {
            foreach (var monkey in Monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    checked
                    {
                        var itemWorryLevel = monkey.Items.Dequeue();

                        if (monkey.Operator == "+")
                            itemWorryLevel += monkey.OperatorValue == 0 ? itemWorryLevel : monkey.OperatorValue;
                        else if (monkey.Operator == "*")
                            itemWorryLevel *= monkey.OperatorValue == 0 ? itemWorryLevel : monkey.OperatorValue;
                        
                        if(worryRelief > 1)
                            itemWorryLevel /= worryRelief;
                        else
                            itemWorryLevel %= modulus; // = 11 * 19 * 5 * 2 * 13 * 7 * 3 * 17

                        if (itemWorryLevel % monkey.TestValue == 0)
                            Monkeys[monkey.Targets[0]].Items.Enqueue(itemWorryLevel);
                        else
                            Monkeys[monkey.Targets[1]].Items.Enqueue(itemWorryLevel);

                        monkey.NumberOfItemInspections++;
                    }
                }
            }
        }
    }
}