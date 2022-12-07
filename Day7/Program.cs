var input = await File.ReadAllLinesAsync("Input.txt");
Queue<string> lines = input.ToQueue();

Stack<IFile> files = new();
List<MyDir> dirs = new();
lines.ProcessCommands(files, dirs);

var fileTree = files.Pop(); //Root folder populated with all children
var sum = dirs.Where(d => d.Size < 100_000).Sum(d => d.Size);

Console.WriteLine("Sum of dirs under 100k: {0}", sum);


var freeSpace = 70_000_000 - fileTree.Size;
var missingSpace = 30_000_000 - freeSpace;

var smallestFolderToDelete = dirs.Where(d => d.Size > missingSpace).OrderBy(d => d.Size).First();

Console.WriteLine("The smallest folder that can be deleted to achieve required free space is: {0}, {1}", smallestFolderToDelete.Name, smallestFolderToDelete.Size);

Console.ReadLine();


internal interface IFile 
{
    int Size { get; }
    string Name { get; }
}

internal class MyFile : IFile
{
    public MyFile(string name, int size)
    {
        Name = name;
        Size = size;
    }

    public int Size { get; private set; }
    public string Name { get; private set; }

    public override string ToString()
    {
        return $"{Name}: {Size}";
    }
}

internal sealed class MyDir : IFile
{
    public MyDir(string name)
    {
        Name = name;
        Children = new List<IFile>();
    }

    public IList<IFile> Children { get; }
    public int Size => Children?.Sum(x => x.Size) ?? 0;
    public string Name { get; private set; }

    public override string ToString()
    {
        return $"{Name}: {Size}";
    }
}

internal static class StringEnumerableExt
{
    internal static Queue<string> ToQueue(this IEnumerable<string> input)
    {
        Queue<string> queue = new();
        foreach (string c in input)
        {
            queue.Enqueue(c);
        }
        return queue;
    }

    internal static bool NextIsCommand(this Queue<string> q) => q.Any() && q.Peek().StartsWith('$');
    internal static IFile GetFile(this string l) => l.StartsWith("dir") ? new MyDir(l.Split(' ')[1]) : new MyFile(l.Split(' ')[1], int.Parse(l.Split(' ')[0]));

    internal static void ProcessCommands(this Queue<string> q, Stack<IFile> dirs, IList<MyDir> directories)
    {
        var line = q.Dequeue();
        if (!line.StartsWith('$'))
            throw new ArgumentException("Not a command");
        var fullCommand = line.Split(" ");
        var command = fullCommand[1];
        var parameter = fullCommand.Length > 2 ? fullCommand[2] : null;

        if (command == "cd")
        {
            switch (parameter)
            {
                case "/":
                    MyDir root = new("/");
                    dirs.Push(root);
                    break;
                case "..":
                    dirs.Pop();
                    break;
                default:
                    dirs.Push(((MyDir)dirs.Peek()).Children.First(c => c.Name == parameter));
                    break;
            }
        }
        else if (command == "ls")
        {
            while (!q.NextIsCommand() && q.Any())
            {
                var file = GetFile(q.Dequeue());
                ((MyDir)dirs.Peek()).Children.Add(file);
                if (file is MyDir dir)
                    directories.Add(dir);
            }
        }
        if (q.Any())
            ProcessCommands(q, dirs, directories);
        else
            while (dirs.Count > 1) dirs.Pop();
    }
}