var pairs = (await File.ReadAllLinesAsync("Input.txt")).Select(p => new Pair(p)).ToList();

int count = 0;
foreach (var pair in pairs)
{
    if (pair.Range1.Item1 <= pair.Range2.Item1 && pair.Range1.Item2 >= pair.Range2.Item2)
    {
        count++;
        continue;
    }
    if (pair.Range2.Item1 <= pair.Range1.Item1 && pair.Range2.Item2 >= pair.Range1.Item2)
    {
        count++;
    }
}

Console.WriteLine("The count of fully contained ranges within the pairs is:");
Console.WriteLine(count);

internal sealed class Pair
{
	public Pair(string input)
	{
		var r1 = input.Split(",").First();
        var r2 = input.Split(",").Last();
		Range1 = new(int.Parse(r1.Split('-').First()), int.Parse(r1.Split('-').Last()));
        Range2 = new(int.Parse(r2.Split('-').First()), int.Parse(r2.Split('-').Last()));
    }

	public Tuple<int, int> Range1 { get; }
    public Tuple<int, int> Range2 { get; }
}