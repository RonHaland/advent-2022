var input = await File.ReadAllLinesAsync("Input.txt");
var nodes = input.SelectMany((l, y) => l.Select((c, x) => new Node(c, x, y))).ToList();

//populate all neighbours
foreach (var node in nodes)
{
    node.ConnectedNodes = nodes.Where(n => (((n.X == node.X - 1 || n.X == node.X + 1) && n.Y == node.Y)
                                           || ((n.Y == node.Y - 1 || n.Y == node.Y + 1) && n.X == node.X)) && int.Abs(node.Level - n.Level) <= 1).ToList();
}

List<Node> openNodes = new()
{
    nodes.OrderBy(n => n.Level).First()
};

var target = nodes.OrderBy(n => n.Level).Last();
openNodes.First().Score = 0;
openNodes.First().TargetScore = - Distance(openNodes.First(), target);

while (openNodes.Any())
{
    var current = openNodes.OrderBy(n => n.TargetScore).First();
    if (current.Id == target.Id)
    {
        Console.WriteLine("target reached");
        break;
    }

    openNodes.RemoveAll(n => n.Id == current.Id);
    foreach (var neighbour in current.ConnectedNodes)
    {
        var tempScore = current.Score + 1;
        if (tempScore < neighbour.Score)
        {
            neighbour.Score = tempScore;
            neighbour.TargetScore =  (tempScore - Distance(neighbour, target));
            neighbour.CameFrom = current;
            if (!openNodes.Contains(neighbour))
                openNodes.Add(neighbour);
        }
    }
    if (!openNodes.Any())
        Console.WriteLine(current.Level);
}

var cameFrom = target.CameFrom;
int steps = 0;
while(cameFrom != null)
{
    Console.WriteLine((char)int.Abs(cameFrom.Level));
    steps++;
    cameFrom = cameFrom.CameFrom;
}

Console.WriteLine("Steps required: {0}", steps);

static double Distance(Node one, Node two) => Math.Sqrt(double.Pow(one.X - two.X, 2) + double.Pow(one.Y - two.Y, 2));

internal sealed class Node
{
    public Node(char level, int x, int y)
    {
        Level = 1 * (level == 'E' ? 123 : level == 'S' ? 96 : (int)level);
        ConnectedNodes = new List<Node>();
        Score = int.MaxValue;
        TargetScore = double.MaxValue;
        X = x;
        Y = y;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public int X { get; set; }
    public int Y { get; set; }
    public int Level { get; set; }
    public IList<Node> ConnectedNodes { get; set; }
    public Node? CameFrom { get; set; }
    public int Score { get; set; }
    public double TargetScore { get; set; }
}