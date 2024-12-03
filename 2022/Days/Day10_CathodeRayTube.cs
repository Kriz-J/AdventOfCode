namespace Days;

public class Day10_CathodeRayTube
{
    public static readonly string[] Instructions = File.ReadAllLines(@"..\..\..\Resources\Day10_Puzzle_Input.txt");

    public static int Cycle { get; set; }
    public static int xRegister { get; set; } = 1;
    public static Dictionary<int, int> SignalStrength { get; set; } = new();
    public static char[] CRTScreen { get; set; } = new char[6 * 40];
    public static int DrawPosition { get; set; }
    public static int[] SpritePosition { get; set; } = new int[3];

    public static void Part1And2()
    {
        foreach (var instruction in Instructions)
        {
            var operation = instruction.Split(' ')[0];

            switch (operation)
            {
                case "noop":
                    PerformCycleLogic();
                    break;

                case "addx":
                    if (!int.TryParse(instruction.Split(' ')[1], out var value))
                        throw new Exception("Can't read instruction value.");

                    //Start of first cycle
                    PerformCycleLogic();
                    //Completion of first cycle

                    //Start of second cycle
                    PerformCycleLogic();
                    //Completion of second cycle

                    xRegister += value;
                    break;

                default:
                    throw new Exception($"Operation {operation} not supported.");
            }

        }

        var sumOfKeySignalStrengths = SignalStrength
            .Where(i => i.Key % 20 == 0 && i.Key / 20 % 2 != 0)
            .Sum(i => i.Value);

        Console.WriteLine(@$"The answer to the first puzzle is: {sumOfKeySignalStrengths}");

        Console.WriteLine(@$"The answer to the second puzzle is:");
        for (int i = 0; i < CRTScreen.Length; i++)
        {
            Console.Write(CRTScreen[i]);
            if ((i + 1) % 40 == 0)
                Console.WriteLine();
        }
    }

    private static void PerformCycleLogic()
    {
        Cycle++;
        SignalStrength.Add(Cycle, Cycle * xRegister);

        DrawPosition = (Cycle - 1) % 40;
        Array.Copy(new[] { xRegister - 1, xRegister, xRegister + 1 }, SpritePosition, 3);

        CRTScreen[Cycle - 1] = SpritePosition.Contains(DrawPosition) ? '#' : '.';
    }
}