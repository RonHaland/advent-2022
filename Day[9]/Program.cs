var input = await File.ReadAllLinesAsync("Input.txt");

var moves = input.Select(x => new Move(Enum.Parse<Direction>(x.Split(' ')[0]), int.Parse(x.Split(' ')[1])));

//RopeSegment headPos = new(0, 0);
List<RopeSegment> shortRope = Enumerable.Range(0, 2).Select(r => new RopeSegment(0, 0)).ToList();
List<RopeSegment> longRope = Enumerable.Range(0, 10).Select(r => new RopeSegment(0, 0)).ToList();


var shortVisited = SimulateRope(moves, shortRope);
var visited = SimulateRope(moves, longRope);

Console.WriteLine("Tail of short rope vistited {0} different positions!", shortVisited);
Console.WriteLine("Tail of long rope vistited {0} different positions!", visited);

double GetDistance(RopeSegment one, RopeSegment two)
{
    double hDist = double.Abs(one.X - two.X);
    double vDist = double.Abs(one.Y - two.Y);
    return double.Sqrt(double.Pow(hDist, 2) + double.Pow(vDist, 2)); 
}

int SimulateRope(IEnumerable<Move> moves, List<RopeSegment> rope)
{
    List<string> visited = new();

    foreach (var move in moves)
    {
        for (int i = 0; i < move.Count; i++)
        {
            var headPos = rope[0];
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
            for (int j = 1; j < rope.Count; j++)
            {
                var target = rope[j - 1];
                var current = rope[j];
                var dist = GetDistance(target, current);
                if (dist >= 2)
                {
                    current.MoveTowards(target, dist > 2);
                }
            }
            if (!visited.Contains(rope.Last().ToString()))
                visited.Add(rope.Last().ToString());

            //Console.WriteLine($"After move({move.Dir}, {i}) [{headPos}], [{tailPos}]");
        }
    }
    return visited.Count;
}

internal record Move(Direction Dir, int Count);
internal class RopeSegment
{
    public RopeSegment(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }

    internal void MoveTowards(RopeSegment target, bool diag)
    {
        var hDiff = (target.X - X)/2d;
        var hNegative = hDiff < 0;
        var vDiff = (target.Y - Y)/2d;
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