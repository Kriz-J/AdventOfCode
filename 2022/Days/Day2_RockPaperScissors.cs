namespace Days;

public class Day2_RockPaperScissors
{
    public static void Part1()
    {

        var strategyGuide = File.ReadAllLines(@"..\..\..\Resources\Day2_Puzzle_Input.txt");

        var totalScore = 0;

        foreach (var round in strategyGuide)
        {
            var instructions = round.Split(' ');

            var opponentsHand = MapLetterToHand(instructions[0]);
            var yourHand = MapLetterToHand(instructions[1]);

            totalScore += (int)yourHand + CalculateRoundOutcome(opponentsHand, yourHand);
        }

        Console.WriteLine(@$"The answer to the first puzzle is: {totalScore}");
    }

    public static void Part2()
    {
        var strategyGuide = File.ReadAllLines(@"..\..\..\Resources\Day2_Puzzle_Input.txt");

        var totalScore = 0;

        foreach (var round in strategyGuide)
        {
            var instructions = round.Split(' ');

            var opponentsHand = MapLetterToHand(instructions[0]);
            var yourHand = MapYourHandToLoseDrawWin(opponentsHand, instructions[1]);

            totalScore += (int)yourHand + CalculateRoundOutcome(instructions[1]);
        }

        Console.WriteLine(@$"The answer to the second puzzle is: {totalScore}");
    }

    private enum GameHand
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private static GameHand MapLetterToHand(string letter) => letter switch
    {
        "A" or "X" => GameHand.Rock,
        "B" or "Y" => GameHand.Paper,
        "C" or "Z" => GameHand.Scissors,

        _ => throw new ArgumentOutOfRangeException(nameof(letter), @"Not included in the strategy guide.")
    };

    private static int CalculateRoundOutcome(GameHand opponent, GameHand you)
    {
        if (opponent == you)
            return 3;

        return opponent switch
        {
            GameHand.Rock => you == GameHand.Paper ? 6 : 0,
            GameHand.Paper => you == GameHand.Scissors ? 6 : 0,
            GameHand.Scissors => you == GameHand.Rock ? 6 : 0,
            _ => throw new InvalidOperationException($"Hand '{opponent}' not supported.")
        };
    }

    private static GameHand MapYourHandToLoseDrawWin(GameHand opponent, string letter) => letter switch
    {
        "X" => opponent switch
        {
            GameHand.Rock => GameHand.Scissors,
            GameHand.Paper => GameHand.Rock,
            GameHand.Scissors => GameHand.Paper,
            _ => throw new InvalidOperationException($"Hand '{opponent}' not supported.")
        },

        "Y" => opponent,

        "Z" => opponent switch
        {
            GameHand.Rock => GameHand.Paper,
            GameHand.Paper => GameHand.Scissors,
            GameHand.Scissors => GameHand.Rock,
            _ => throw new InvalidOperationException($"Hand '{opponent}' not supported.")

        },

        _ => throw new ArgumentOutOfRangeException(nameof(letter), @"Not included in the strategy guide.")

    };

    private static int CalculateRoundOutcome(string letter) => letter switch
    {
        "X" => 0,
        "Y" => 3,
        "Z" => 6,
        _ => throw new ArgumentOutOfRangeException(nameof(letter), @"Not included in the strategy guide.")
    };
}