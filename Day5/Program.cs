using System.Text.RegularExpressions;

var file = await File.ReadAllTextAsync("Input.txt");

var rows = file.Split("\r\n\r\n")[0]
               .Split("\r\n")
               .Reverse()
               .Skip(1)
               .ToList();
var moves = file.Split("\r\n\r\n")[1]
                .Split("\r\n")
                .Select(s => new Move(s))
                .ToList();

List<Stack<char>> stacks = MakeStacks(rows);
moves.ForEach(m => m.ExecuteMove(stacks));

Console.WriteLine("The top crates with CrateMover 9000 are:");
Console.WriteLine(string.Join("",stacks.Select(s => s.Peek())));

//part2
stacks = MakeStacks(rows);
moves.ForEach(m => m.ExecuteMoveV2(stacks));

Console.WriteLine("The top crates with CrateMover 9001 are:");
Console.WriteLine(string.Join("", stacks.Select(s => s.Peek())));


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

    public void ExecuteMoveV2(List<Stack<char>> stacks)
    {
        var cratesToMove = Enumerable.Range(0, Count)
            .Select(i => stacks[From].Pop())
            .Reverse()
            .ToList();
        cratesToMove.ForEach(c => stacks[To].Push(c));
    }
}
