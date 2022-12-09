namespace AdventOfCode2022;

public class Day7_NoSpaceLeftOnDevice
{
    private static readonly string[] TerminalOutput = File.ReadAllLines(@"..\..\..\Resources\Day7_Puzzle_Input.txt");

    private class Node
    {
        public string Name { get; }
        public bool IsDirectory { get; }
        public Node? Parent { get; }
        public List<Node>? Children { get;}
        public int Size { get; set; }

        public Node(string name, bool isDirectory, Node? parent)
        {
            Name = name;
            IsDirectory = isDirectory;
            Parent = parent;
            Children = IsDirectory ? new List<Node>() : null;
        }
    }

    public static void Part1And2()
    {
        var fileSystem = new List<Node>();

        Node? currentDirectory = null;

        foreach (var row in TerminalOutput)
        {
            var commands = row.Split(' ');

            switch (commands[0])
            {
                case "$":
                    if (commands[1] == "ls")
                    {
                        continue; // nothing to act on
                    }

                    switch (commands[2])
                    {
                        case "/":
                            var root = new Node(commands[2], true, null);

                            if (!fileSystem.Any())
                            {
                                fileSystem.Add(root);
                            }

                            currentDirectory = root;
                            break;

                        case "..":
                            if (currentDirectory is null)
                                throw new Exception("Root has not been initialized.");

                            currentDirectory = currentDirectory.Parent ?? throw new Exception("Already at root.");
                            break;

                        default: // enter a directory
                            if (currentDirectory is null) 
                                throw new Exception("Root has not been initialized.");

                            if (currentDirectory.Children is null)
                                throw new Exception($"'{currentDirectory.Name}' has no directories or files.");

                            var nextDirectory = currentDirectory.Children.FirstOrDefault(c => c.IsDirectory && c.Name == commands[2]);

                            currentDirectory = nextDirectory ?? throw new Exception($"Directory '{commands[2]}' could not be found.");
                            break;
                    }

                    break;

                case "dir": //directory exists in current directory
                    if (currentDirectory is null)
                        throw new Exception("Can't add child without parent.");
                    
                    if (currentDirectory.Children is null)
                        throw new Exception($"Not possible to add child to '{currentDirectory}'.");

                    if (currentDirectory.Children.FirstOrDefault(c => c.IsDirectory && c.Name == commands[1]) is not null)
                        throw new Exception($"{currentDirectory} already has a folder '{commands[1]}'.");

                    var directory = new Node(commands[1], true, currentDirectory);

                    currentDirectory.Children.Add(directory);
                    break;

                default: //file exists in current directory
                    if (currentDirectory is null)
                        throw new Exception("Can't add child without parent.");

                    if (currentDirectory.Children is null)
                        throw new Exception($"Not possible to add child to '{currentDirectory}'");

                    if (currentDirectory.Children.FirstOrDefault(c => !c.IsDirectory && c.Name == commands[1]) is not null)
                        throw new Exception($"{currentDirectory} already has a file '{commands[1]}'.");

                    if (!int.TryParse(commands[0], out var fileSize))
                        throw new Exception($"Can't parse file size of '{commands[1]}'.");

                    var file = new Node(commands[1], false, currentDirectory)
                    {
                        Size = fileSize
                    };

                    currentDirectory.Children.Add(file);
                    break;
            }
        }

        CalculateSizeOfDirectoriesInFileSystem(fileSystem[0]);

        var smallDirectories = new List<Node>();

        FindDirectoriesFromNodeSmallerOrEqualThan(100000, fileSystem[0], smallDirectories);

        Console.WriteLine($@"The answer to the first puzzle is {smallDirectories.Sum(d => d.Size)}");

        var directorySizeNecessaryToDelete = fileSystem[0].Size - 40000000;

        var deletionDirectories = new List<Node>();

        FindDirectoriesFromNodeSmallerOrEqualThan(fileSystem[0].Size, fileSystem[0], deletionDirectories);

        var deletionDirectory = deletionDirectories
            .OrderBy(d => d.Size)
            .FirstOrDefault(d => d.Size > directorySizeNecessaryToDelete) ?? fileSystem[0];

        Console.WriteLine($@"The answer to the second puzzle is {deletionDirectory.Size}");
    }

    private static void FindDirectoriesFromNodeSmallerOrEqualThan(int directorySize, Node node, ICollection<Node> directories)
    {
        if (node.Children is null)
            return;

        foreach (var child in node.Children)
        {
            if (child.IsDirectory && child.Size <= directorySize)
            {
                directories.Add(child);
            }
            
            FindDirectoriesFromNodeSmallerOrEqualThan(directorySize, child, directories);
        }
    }

    private static void CalculateSizeOfDirectoriesInFileSystem(Node node)
    {
        if (node.Children is null)
            return;

        foreach (var child in node.Children)
        {
            CalculateSizeOfDirectoriesInFileSystem(child);
            if (child.Parent is not null)
            {
                child.Parent.Size += child.Size;
            }
        }
    }
}