var input = await File.ReadAllLinesAsync("Input.txt");

var moves = input.Select(x => new Move(Enum.Parse<Direction>(x.Split(' ')[0]), int.Parse(x.Split(' ')[1])));

RopeEnd headPos = new(0, 0);
RopeEnd tailPos = new(0, 0);

List<string> visited = new();


foreach (var move in moves)
{
    for (int i = 0; i < move.Count; i++)
    {

        switch (move.Dir)
        {
            case Direction.U:
                headPos.Y--;
                break;
            case Direction.R:
                headPos.X++;
                break;
            case Direction.D:
                headPos.Y++;
                break;
            case Direction.L:
                headPos.X--;
                break;
            default:
                break;
        }
        var dist = GetDistance(headPos, tailPos);
        if (dist >= 2)
        {
            tailPos.MoveTowards(headPos, dist > 2);
        }
        if (!visited.Contains(tailPos.ToString()))
            visited.Add(tailPos.ToString());

        //Console.WriteLine($"After move({move.Dir}, {i}) [{headPos}], [{tailPos}]");
    }
}

Console.WriteLine("Tail vistited {0} different positions!", visited.Count);

double GetDistance(RopeEnd one, RopeEnd two)
{
    double hDist = double.Abs(one.X - two.X);
    double vDist = double.Abs(one.Y - two.Y);
    return double.Sqrt(double.Pow(hDist, 2) + double.Pow(vDist, 2)); 
}
internal record Move(Direction Dir, int Count);
internal class RopeEnd
{
    public RopeEnd(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }

    internal void MoveTowards(RopeEnd headPos, bool diag)
    {
        var hDiff = (headPos.X - X)/2d;
        var hNegative = hDiff < 0;
        var vDiff = (headPos.Y - Y)/2d;
        var vNegative = vDiff < 0;
        X += !diag || hNegative ? (int)Math.Floor(hDiff) : (int)Math.Ceiling(hDiff);
        Y += !diag || vNegative ? (int)Math.Floor(vDiff) : (int)Math.Ceiling(vDiff);
    }
    public override string ToString()
    {
        return $"{X}:{Y}";
    }
}
internal enum Direction { U,R,D,L }