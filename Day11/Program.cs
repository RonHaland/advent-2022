using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync("Input.txt");
var monkeys = input.Split(Environment.NewLine + Environment.NewLine).Select(x => new Monkey(x)).ToList();

foreach (var monkey in monkeys)
{
	monkey.SetTargetMonkeys(monkeys);
}

for (int i = 0; i < 20; i++)
{
	foreach (var monkey in monkeys)
	{
		monkey.ExecuteTurn();
	}
}

var top2Monkeys = monkeys.OrderByDescending(m => m.InspectedItems).Take(2).ToList();

Console.WriteLine("Level of monkey business is {0}", top2Monkeys.Aggregate(1, (i, m) => i * m.InspectedItems));

internal sealed partial class Monkey
{
	private string[] _lines;
	public Monkey(string monkeyInput)
	{
		var infoLines = monkeyInput.Split(Environment.NewLine);
		_lines = infoLines;
		Name = infoLines[0].Split(':')[0];
		Items = new Queue<int>(infoLines[1].Split(':')[1].Split(",").Select(int.Parse));
		Operation = MakeOperation(infoLines[2].Split(':')[1]);
		Test = MakeTest(infoLines[3]);
		InspectedItems = 0;
    }

	public string Name { get; set; }
	public Queue<int> Items { get; set; }
	public Func<int, int> Operation { get; set; }
	public Func<int, bool> Test { get; set; }
	private Monkey? TrueMonkey { get; set; }
	private Monkey? FalseMonkey { get; set; }
	public int InspectedItems { get; set; }

	public void ExecuteTurn()
	{
		while (Items.Any())
		{
			var item = Items.Dequeue();
			InspectedItems++;
            var processedItem = Operation(item) / 3;
			if (Test(processedItem))
				TrueMonkey?.Items.Enqueue(processedItem);
			else
				FalseMonkey?.Items.Enqueue(processedItem);
		}
	}

	public void SetTargetMonkeys(IEnumerable<Monkey> allMonkeys) 
	{
		var trueMonkeyTarget = NumbersRegex().Match(_lines[4]).Value ?? "0";
        var falseMonkeyTarget = NumbersRegex().Match(_lines[5]).Value ?? "0";
		TrueMonkey = allMonkeys.Single(x => x.Name == $"Monkey {trueMonkeyTarget}");
        FalseMonkey = allMonkeys.Single(x => x.Name == $"Monkey {falseMonkeyTarget}");
    }

	private Func<int, int> MakeOperation(string opString) 
    {
		var parts = opString.Split(' ');

        return (input) =>
		{
			var first = parts[3] == "old" ? input : int.Parse(parts[3]);
			var second = parts[5] == "old" ? input : int.Parse(parts[5]);
			switch (parts[4])
			{
				case "*":
					return first * second;
				case "/":
					return first / second;
				case "-":
					return first - second;
				case "+":
				default:
					return first + second;
			}
		};
    }

	private Func<int, bool> MakeTest(string testString)
	{
		var divisibleNumber = int.Parse(NumbersRegex().Match(testString).Value);
		return (x) => x % divisibleNumber == 0;
	}

    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumbersRegex();
}
