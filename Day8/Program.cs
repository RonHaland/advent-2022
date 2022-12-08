var input = await File.ReadAllLinesAsync("Input.txt");

int[] horizontalEdges = new[] { 0, input.Length - 1 };
int[] verticalEdges = new[] { 0, input[0].Length - 1 };

List<Tree> trees = new();
for (int i = 0; i < input.Length; i++)
{
    for (int j = 0; j < input[i].Length; j++)
    {
        bool isVisible = false;
        var height = int.Parse(input[i][j].ToString());
        var verticalString = new string(input.Select(s => s[j]).ToArray());

        var left = input[i].Substring(0, j).Select(c => int.Parse(c.ToString())).Reverse();
        var right = input[i].Substring(j + 1).Select(c => int.Parse(c.ToString()));
        var top = verticalString.Substring(0, i).Select(c => int.Parse(c.ToString())).Reverse();
        var bottom = verticalString.Substring(i + 1).Select(c => int.Parse(c.ToString()));

        if (horizontalEdges.Contains(j)
            || verticalEdges.Contains(i)
            || left.Max() < height
            || right.Max() < height
            || top.Max() < height
            || bottom.Max() < height)
            isVisible = true;

        var visibility = CalculateVisibility(height, left, right, top, bottom);

        trees.Add(new Tree { Height = height, IsVisible = isVisible, VisibilityScore = visibility });
    }
}

Console.WriteLine("Count of visible trees: {0}", trees.Count(t => t.IsVisible));

Console.WriteLine("Highest visibility score: {0}", trees.OrderBy(t => t.VisibilityScore).Last().VisibilityScore);
Console.ReadLine();

int CalculateVisibility(int height, params IEnumerable<int>[] directions)
{
    var visibilities = directions.Select(d => GetVisibilityInDirection(height, d));
    return visibilities.Aggregate(1, (a, b) => a * b);
}

int GetVisibilityInDirection(int height, IEnumerable<int> direction)
{
    int result = 0;
    foreach (var tree in direction)
    {
        result++;
        if (tree >= height)
            break;
    }
    return result;
}

internal sealed class Tree
{
    public int Height { get; set; }
    public bool IsVisible { get; set; }
    public int VisibilityScore { get; set; }
}
