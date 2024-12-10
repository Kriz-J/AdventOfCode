namespace Advent2024.Days;

public class Day04_CeresSearch
{
    public static string[] Input { get; set; } = SantasLittleHelpers.ReadFileRows("2024-12-04.txt");

    public delegate int Searches(int i, int j);

    public static void Puzzle1()
    {
        var numberOfXmas = 0;

        for (var i = 0; i < Input.Length; i++)
        {
            for (var j = 0; j < Input[i].Length; j++)
            {
                var letter = Input[i][j];
                if (letter != 'X') continue;

                Searches? searchDelegate = null;
                searchDelegate += SearchRight;
                searchDelegate += SearchLeft;
                searchDelegate += SearchUp;
                searchDelegate += SearchDown;
                searchDelegate += SearchUpRight;
                searchDelegate += SearchUpLeft;
                searchDelegate += SearchDownRight;
                searchDelegate += SearchDownLeft;

                if (TooCloseToTopBorder(i))
                {
                    searchDelegate = FilterMethodsFromDelegate(searchDelegate, methodNameContains: "Up");
                }

                if (TooCloseToBottomBorder(i))
                {
                    searchDelegate = FilterMethodsFromDelegate(searchDelegate, methodNameContains: "Down");
                }

                if (TooCloseToLeftBorder(j))
                {
                    searchDelegate = FilterMethodsFromDelegate(searchDelegate, methodNameContains: "Left");
                }

                if (TooCloseToRightBorder(j, i))
                {
                    searchDelegate = FilterMethodsFromDelegate(searchDelegate, methodNameContains: "Right");
                }

                numberOfXmas += searchDelegate.GetInvocationList()
                    .Cast<Searches>()
                    .Sum(method => method.Invoke(i, j));
            }
        }

        Console.WriteLine($"Answer: In the word search 'XMAS' occurs {numberOfXmas} times.");
    }

    private static Searches FilterMethodsFromDelegate(Searches searchDelegate, string methodNameContains)
    {
        return searchDelegate.GetInvocationList()
            .Where(d => !d.Method.Name.Contains(methodNameContains))
            .Select(d => (Searches)d)
            .Aggregate((Searches)null!, (current, next) => current + next);
    }

    private static bool TooCloseToTopBorder(int i) => i < 3;
    private static bool TooCloseToBottomBorder(int i) => i > Input.Length - 4;
    private static bool TooCloseToLeftBorder(int j) => j < 3;
    private static bool TooCloseToRightBorder(int j, int i) => j > Input[i].Length - 4;

    private static int SearchRight(int i, int j) => new string([Input[i][j + 1], Input[i][j + 2], Input[i][j + 3]]) == "MAS" ? 1 : 0;
    private static int SearchLeft(int i, int j) => new string([Input[i][j - 1], Input[i][j - 2], Input[i][j - 3]]) == "MAS" ? 1 : 0;
    private static int SearchUp(int i, int j) => new string([Input[i - 1][j], Input[i - 2][j], Input[i - 3][j]]) == "MAS" ? 1 : 0;
    private static int SearchDown(int i, int j) => new string([Input[i + 1][j], Input[i + 2][j], Input[i + 3][j]]) == "MAS" ? 1 : 0;
    private static int SearchDownRight(int i, int j) => new string([Input[i + 1][j + 1], Input[i + 2][j + 2], Input[i + 3][j + 3]]) == "MAS" ? 1 : 0;
    private static int SearchUpRight(int i, int j) => new string([Input[i - 1][j + 1], Input[i - 2][j + 2], Input[i - 3][j + 3]]) == "MAS" ? 1 : 0;
    private static int SearchDownLeft(int i, int j) => new string([Input[i + 1][j - 1], Input[i + 2][j - 2], Input[i + 3][j - 3]]) == "MAS" ? 1 : 0;
    private static int SearchUpLeft(int i, int j) => new string([Input[i - 1][j - 1], Input[i - 2][j - 2], Input[i - 3][j - 3]]) == "MAS" ? 1 : 0;

    public static void Puzzle2()
    {
        var numberOfXmas = 0;

        for (var i = 1; i < Input.Length - 1; i++)
        {
            for (var j = 1; j < Input[i].Length - 1; j++)
            {
                var letter = Input[i][j];
                if (letter != 'A') continue;

                if ((UpRightIsM(i, j) && DownLeftIsS(i, j) && UpLeftIsM(i, j) && DownRightIsS(i, j)) ||
                    (UpRightIsM(i, j) && DownLeftIsS(i, j) && UpLeftIsS(i, j) && DownRightIsM(i, j)) ||
                    (UpRightIsS(i, j) && DownLeftIsM(i, j) && UpLeftIsM(i, j) && DownRightIsS(i, j)) ||
                    (UpRightIsS(i, j) && DownLeftIsM(i, j) && UpLeftIsS(i, j) && DownRightIsM(i, j)))
                {
                    numberOfXmas++;
                }
            }
        }
        Console.WriteLine($"Answer: In the word search 'X-MAS' occurs {numberOfXmas} times.");
    }

    private static bool UpRightIsM(int i, int j) => Input[i - 1][j + 1] == 'M';
    private static bool UpRightIsS(int i, int j) => Input[i - 1][j + 1] == 'S';
    private static bool UpLeftIsM(int i, int j) => Input[i - 1][j - 1] == 'M';
    private static bool UpLeftIsS(int i, int j) => Input[i - 1][j - 1] == 'S';
    private static bool DownRightIsM(int i, int j) => Input[i + 1][j + 1] == 'M';
    private static bool DownRightIsS(int i, int j) => Input[i + 1][j + 1] == 'S';
    private static bool DownLeftIsM(int i, int j) => Input[i + 1][j - 1] == 'M';
    private static bool DownLeftIsS(int i, int j) => Input[i + 1][j - 1] == 'S';
}