using System.Text.RegularExpressions;

var file = await File.ReadAllTextAsync("Input.txt");

var rows = file.Split("\r\n\r\n")[0].Split("\r\n").Reverse().Skip(1).ToList();
var moves = file.Split("\r\n\r\n")[1].Split("\r\n").Select(s => new Move(s)).ToList();

List<Stack<char>> stacks = MakeStacks(rows);

moves.ForEach(m => m.ExecuteMove(stacks));

Console.WriteLine("The top crates are:");
Console.WriteLine(string.Join("",stacks.Select(s => s.Peek())));


static List<Stack<char>> MakeStacks(List<string> rows)
{
    List<Stack<char>> stacks = new();

    for (var r = 0; r < rows.Count; r++)
    {
        var row = rows[r];
        for (var i = 0; i < row.Length; i += 4)
        {
            var crate = row.Substring(i, 3);
            if (stacks.Count <= i / 4)
                stacks.Add(new());
            if (crate[1] != ' ')
                stacks[i / 4].Push(crate[1]);
        }
    }

    return stacks;
}

internal sealed class Move
{
    public Move(string line)
    {
        Regex numbers = new("[0-9]+");
        var values = numbers.Matches(line);

        Count = int.Parse(values[0].ToString());
        From = int.Parse(values[1].ToString()) - 1;
        To = int.Parse(values[2].ToString()) - 1;
    }
    public int From { get; set; }
    public int To { get; set; }
    public int Count { get; set; }

    public void ExecuteMove(List<Stack<char>> stacks)
    {
        for (var i = 0; i < Count; i++)
        {
            stacks[To].Push(stacks[From].Pop());
        }
    }
}
