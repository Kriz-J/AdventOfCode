namespace AdventOfCode2022;

public class Day8_TreetopTreeHouse
{
    private static readonly string[] TreeHeightMap = File.ReadAllLines(@"..\..\..\Resources\Day8_Puzzle_Input.txt");

    private static readonly int NumberOfTreeRows = TreeHeightMap.Length;
    private static readonly int NumberOfTreeColumns = TreeHeightMap.First().Length;
    public static int HiddenTrees { get; set; }

    public static void Part1And2()
    {
        var trees = new int[NumberOfTreeRows, NumberOfTreeColumns];
        var scenicScores = new int[NumberOfTreeRows, NumberOfTreeColumns];

        for (int i = 0; i < NumberOfTreeRows; i++)
        {
            for (int j = 0; j < NumberOfTreeColumns; j++)
            {
                trees[i, j] = TreeHeightMap[i][j] - '0';
            }
        }

        for (int i = 0; i < NumberOfTreeRows; i++)
        {
            for (int j = 0; j < NumberOfTreeColumns; j++)
            {
                if (CheckIfTreeIsHidden(j, i, trees, out scenicScores[i, j]))
                    HiddenTrees++;
            }
        }

        Console.WriteLine(@$"The answer to the first puzzle is: {NumberOfTreeRows * NumberOfTreeColumns - HiddenTrees}");

        var maxScenicScore = 0;

        for (int i = 0; i < NumberOfTreeRows; i++)
        {
            for (int j = 0; j < NumberOfTreeColumns; j++)
            {
                if (scenicScores[i, j] > maxScenicScore)
                    maxScenicScore = scenicScores[i, j];
            }
        }

        Console.WriteLine(@$"The answer to the second puzzle is: {maxScenicScore}");
    }

    private static bool CheckIfTreeIsHidden(int xCoord, int yCoord, int[,] map, out int scenicScore)
    {
        var treeHeight = map[yCoord, xCoord];

        var treeRow = Enumerable.Range(0, NumberOfTreeColumns).Select(x => map[yCoord, x]).ToArray();
        var treeColumn = Enumerable.Range(0, NumberOfTreeRows).Select(x => map[x, xCoord]).ToArray();

        var hiddenFromLeft = treeRow[..xCoord].Any(t => t >= treeHeight);
        var hiddenFromRight = treeRow[(xCoord + 1)..].Any(t => t >= treeHeight);
        var hiddenFromAbove = treeColumn[..yCoord].Any(t => t >= treeHeight);
        var hiddenFromBelow = treeColumn[(yCoord + 1)..].Any(t => t >= treeHeight);

        scenicScore = CalculateScenicScore(xCoord, yCoord, treeRow, treeColumn);

        return hiddenFromLeft && hiddenFromRight && hiddenFromAbove && hiddenFromBelow;
    }

    private static int CalculateScenicScore(int xCoord, int yCoord, int[] treeRow, int[] treeColumn)
    {
        var treeHeight = treeRow[xCoord];

        var left = CalculateScenicScoreInOneDirection(treeRow[..xCoord].Reverse(), treeHeight);
        var right = CalculateScenicScoreInOneDirection(treeRow[(xCoord + 1)..], treeHeight);
        var above = CalculateScenicScoreInOneDirection(treeColumn[..yCoord].Reverse(), treeHeight);
        var below = CalculateScenicScoreInOneDirection(treeColumn[(yCoord + 1)..], treeHeight);

        return left * right * above * below;
    }

    private static int CalculateScenicScoreInOneDirection(IEnumerable<int> treesInDirectionFromCurrentTree, int currentTreeHeight)
    {
        var score = 0;
        foreach (var treeHeight in treesInDirectionFromCurrentTree)
        {
            score++;
            if (treeHeight >= currentTreeHeight)
                break;
        }

        return score;
    }
}