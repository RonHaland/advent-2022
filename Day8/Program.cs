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
        if (horizontalEdges.Contains(j) || verticalEdges.Contains(i))
        { 
            trees.Add(new Tree { Height = height, IsVisible = true });
            continue;
        }

        var verticalString = new string(input.Select(s => s[j]).ToArray());

        var left = input[i].Substring(0, j).Select(c => int.Parse(c.ToString()));
        var right = input[i].Substring(j + 1).Select(c => int.Parse(c.ToString()));
        var top = verticalString.Substring(0, i).Select(c => int.Parse(c.ToString()));
        var bottom = verticalString.Substring(i + 1).Select(c => int.Parse(c.ToString()));

        if (left.Max() < height || right.Max() < height || top.Max() < height || bottom.Max() < height)
            isVisible = true;
        trees.Add(new Tree { Height = height, IsVisible = isVisible });
    }
}

Console.WriteLine("Count of visible trees: {0}", trees.Count(t => t.IsVisible));
Console.ReadLine();


internal sealed class Tree
{
    public int Height { get; set; }
    public bool IsVisible { get; set; }
}   