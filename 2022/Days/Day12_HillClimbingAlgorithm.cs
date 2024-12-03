namespace Days;

public class Day12_HillClimbingAlgorithm
{
    public class Node
    {
        public string Name { get; }
        public char Elevation { get; set; }
        public List<Node> Neighbours { get; set; } = new();
        public bool Visited { get; set; }
        public Node Previous { get; set; }

        public Node(string name, char elevation)
        {
            Name = name;
            Elevation = elevation;
        }
    }

    public static string[] HeightMap { get; set; } = File.ReadAllLines(@"..\..\..\Resources\Day12_Puzzle_Input.txt");
    public static int MapHeight = HeightMap.Length;
    public static int MapWidth = HeightMap[0].Length;
    public static List<Node> Nodes = new();

    public static void Part1()
    {
        CreateNodesFromHeightMap();
        ReadNodeNeighbours();

        var startNode = Nodes.Find(n => n.Elevation == (char)('a' - 1));
        var endNode = Nodes.Find(n => n.Elevation == (char)('z' + 1));

        var routeLength = BreadthFirstSearchReturnRouteLength(startNode, endNode);

        Console.WriteLine(@$"The answer to the first puzzle is {routeLength}");
    }

    public static void Part2()
    {
        CreateNodesFromHeightMap();
        ReadNodeNeighbours();

        var startNodes = Nodes.FindAll(n => n.Elevation is 'a' or (char)('a' - 1));
        var endNode = Nodes.Find(n => n.Elevation == (char)('z' + 1));

        var routeLengths = new List<int>();

        foreach (var startNode in startNodes)
        {
            routeLengths.Add(BreadthFirstSearchReturnRouteLength(startNode, endNode));

            foreach (var node in Nodes)
            {
                node.Visited = false;
                node.Previous = null;
            }
        }

        routeLengths = routeLengths.Where(n => n != 0).ToList();

        Console.WriteLine(@$"The answer to the second puzzle is {routeLengths.Min()}");
    }

    private static int BreadthFirstSearchReturnRouteLength(Node start, Node end)
    {
        var queue = new Queue<Node>();

        start.Visited = true;
        queue.Enqueue(start);

        while (queue.Any())
        {
            var currentNode = queue.Dequeue();

            foreach (var neighbour in currentNode.Neighbours.Where(neighbour => !neighbour.Visited))
            {
                neighbour.Visited = true;
                queue.Enqueue(neighbour);

                neighbour.Previous = currentNode;

                if (neighbour.Elevation == (char)('z' + 1))
                {
                    queue.Clear();
                    break;
                }
            }
        }
        return CalculateRouteLength(end);
    }

    private static int CalculateRouteLength(Node end)
    {
        var node = end;
        var route = new List<Node>();

        while (node is not null)
        {
            route.Add(node);
            node = node.Previous;
        }

        return route.Count - 1; // one less step than nodes
    }

    private static void CreateNodesFromHeightMap()
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                var elevation = HeightMap[i][j];

                elevation = elevation >= 'a'
                    ? elevation
                    : elevation == 'S'
                        ? (char)('a' - 1)
                        : (char)('z' + 1);

                Nodes.Add(new Node($"[{i}][{j}]", elevation));
            }
        }
    }

    private static void ReadNodeNeighbours()
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                if (j - 1 >= 0)
                    CheckWest(j, i);
                if (i - 1 >= 0)
                    CheckNorth(j, i);
                if (j + 1 <= MapWidth - 1)
                    CheckEast(j, i);
                if (i + 1 <= MapHeight - 1)
                    CheckSouth(j, i);
            }
        }
    }

    private static void CheckWest(int xPos, int yPos)
    {
        var currentNode = Nodes[yPos * MapWidth + xPos];
        var westNode = Nodes[yPos * MapWidth + (xPos - 1)];

        CheckDirection(currentNode, westNode);
    }

    private static void CheckEast(int xPos, int yPos)
    {
        var currentNode = Nodes[yPos * MapWidth + xPos];
        var eastNode = Nodes[yPos * MapWidth + xPos + 1];

        CheckDirection(currentNode, eastNode);
    }

    private static void CheckNorth(int xPos, int yPos)
    {
        var currentNode = Nodes[yPos * MapWidth + xPos];
        var northNode = Nodes[(yPos - 1) * MapWidth + xPos];

        CheckDirection(currentNode, northNode);
    }

    private static void CheckSouth(int xPos, int yPos)
    {
        var currentNode = Nodes[yPos * MapWidth + xPos];
        var southNode = Nodes[(yPos + 1) * MapWidth + xPos];

        CheckDirection(currentNode, southNode);
    }

    private static void CheckDirection(Node current, Node direction)
    {
        if (direction.Elevation - current.Elevation <= 1)
        {
            current.Neighbours.Add(direction);
        }
    }
}