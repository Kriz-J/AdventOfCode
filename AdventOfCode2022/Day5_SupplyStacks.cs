using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day5_SupplyStacks
{
    private static readonly string[] instructions = File.ReadAllLines(@"..\..\..\Resources\Day5_Puzzle_Input.txt");

    public static void Part1()
    {
        var idxBreak = FindOperationsStartIndex(instructions);
        
        var startingPositions = instructions[..(idxBreak - 1)];
        var craneOperations = instructions[idxBreak..];

        if (!int.TryParse(startingPositions[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[^1], out var numberOfStacks))
        {
            throw new Exception("Can't find number of stacks.");
        }

        var stacks = CreateStacks(numberOfStacks);

        InitializeStacks(startingPositions, stacks);

        OperateCrateMover9000(craneOperations, stacks);

        Console.Write(@$"The answer to the first puzzle is: {ReadTopCrates(stacks)}");
    }
    public static void Part2()
    {
        var idxBreak = FindOperationsStartIndex(instructions);

        var startingPositions = instructions[..(idxBreak - 1)];
        var craneOperations = instructions[idxBreak..];

        if (!int.TryParse(startingPositions[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[^1], out var numberOfStacks))
        {
            throw new Exception("Can't find number of stacks.");
        }

        var stacks = CreateStacks(numberOfStacks);

        InitializeStacks(startingPositions, stacks);

        OperateCrateMover9001(craneOperations, stacks);

        Console.Write(@$"The answer to the second puzzle is: {ReadTopCrates(stacks)}");
    }

    private static int FindOperationsStartIndex(IReadOnlyList<string> instructions)
    {
        var index = 0;

        // assume empty line delimits starting positions from operations
        while (!string.IsNullOrWhiteSpace(instructions[index++]))
        {
        }

        return index;
    }

    private static Stack<char>[] CreateStacks(int numberOfStacks)
    {
        var stacks = new Stack<char>[numberOfStacks];

        for (int j = 0; j < numberOfStacks; j++)
        {
            stacks[j] = new Stack<char>();
        }

        return stacks;
    }

    private static void InitializeStacks(string[] startingPositions, IReadOnlyList<Stack<char>> stacks)
    {
        foreach (var crateRow in startingPositions[..^1].Reverse())
        {
            for (var j = 0; j < stacks.Count; j++)
            {
                // assume crate positions follows: 1 + n * 4
                if (!string.IsNullOrWhiteSpace($"{crateRow[1 + 4 * j]}"))
                    stacks[j].Push(crateRow[1 + 4 * j]);
            }
        }
    }

    private static void OperateCrateMover9000(IEnumerable<string> craneOperations, IReadOnlyList<Stack<char>> stacks)
    {
        foreach (var craneOperation in craneOperations)
        {
            var (howManyCrates, fromStack, toStack) = ParseOperation(craneOperation);

            while (howManyCrates-- > 0)
            {
                var crate = stacks[fromStack - 1].Pop();
                stacks[toStack - 1].Push(crate);
            }
        }
    }
    private static void OperateCrateMover9001(IEnumerable<string> craneOperations, Stack<char>[] stacks)
    {
        var cratesToMove = new Stack<char>();

        foreach (var craneOperation in craneOperations)
        {
            var (howManyCrates, fromStack, toStack) = ParseOperation(craneOperation);

            while (howManyCrates-- > 0)
            {
                cratesToMove.Push(stacks[fromStack - 1].Pop());
            }

            while (cratesToMove.Count > 0)
            {
                var crate = cratesToMove.Pop();
                stacks[toStack - 1].Push(crate);
            }
        }
    }

    private static (int howManyCrates, int fromStack, int toStack) ParseOperation(string craneOperation)
    {
        var operations = Regex.Matches(craneOperation, @"\d+");

        if (!int.TryParse(operations[0].Value, out var howManyCrates))
            throw new Exception("Can't find how many crates to move.");
        if (!int.TryParse(operations[1].Value, out var fromStack))
            throw new Exception("Can't find where to move crates from.");
        if (!int.TryParse(operations[2].Value, out var toStack))
            throw new Exception("Can't find where to move crates to.");

        return (howManyCrates, fromStack, toStack);
    }

    private static string ReadTopCrates(IEnumerable<Stack<char>> stacks)
    {
        var sb = new StringBuilder();

        foreach (var stack in stacks)
        {
            sb.Append(stack.Peek());
        }

        return sb.ToString();
    }
}