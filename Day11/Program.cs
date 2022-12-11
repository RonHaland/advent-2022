using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync("Input.txt");

List<Monkey> monkeys = InitializeMonkeys(input);

for (int i = 0; i < 20; i++)
{
    foreach (var monkey in monkeys)
    {
        monkey.ExecuteTurn();
    }
}

var top2Monkeys = monkeys.OrderByDescending(m => m.InspectedItems).Take(2).ToList();
Console.WriteLine("Level of monkey business for part 1 is {0}", top2Monkeys.Aggregate(1L, (i, m) => i * m.InspectedItems));

monkeys = InitializeMonkeys(input);

for (int i = 0; i < 10000; i++)
{
    foreach (var monkey in monkeys)
    {
        monkey.ExecuteTurn(false);
    }

    if (i + 1 == 1 || i + 1 == 20 || (i + 1) % 1000 == 0)
    {
        Console.WriteLine("After {0}", i + 1);
        Console.WriteLine(string.Join(Environment.NewLine, monkeys.Select(m => $"{m.Name}: {m.InspectedItems}")));
        Console.WriteLine();
    }
}

top2Monkeys = monkeys.OrderByDescending(m => m.InspectedItems).Take(2).ToList();
Console.WriteLine("Level of monkey business for part 2 is {0}", top2Monkeys.Aggregate(1L, (i, m) => i * m.InspectedItems));

static List<Monkey> InitializeMonkeys(string input)
{
    var monkeys = input.Split(Environment.NewLine + Environment.NewLine).Select(x => new Monkey(x)).ToList();

    var controlNumber = monkeys.Select(x => x.Divisor).Aggregate(1L, (agr, val) => agr * val);

    foreach (var monkey in monkeys)
    {
        monkey.SetTargetMonkeys(monkeys);
        monkey.ControlNumber = controlNumber;
    }

    return monkeys;
}

internal sealed partial class Monkey
{
    private string[] _lines;
    public Monkey(string monkeyInput)
    {
        var infoLines = monkeyInput.Split(Environment.NewLine);
        _lines = infoLines;
        Name = infoLines[0].Split(':')[0];
        Items = new Queue<long>(infoLines[1].Split(':')[1].Split(",").Select(long.Parse));
        Operation = MakeOperation(infoLines[2].Split(':')[1]);
        Divisor = int.Parse(NumbersRegex().Match(infoLines[3]).Value);
        Test = MakeTest();
        InspectedItems = 0;
    }

    public string Name { get; set; }
    public Queue<long> Items { get; set; }
    public Func<long, long> Operation { get; set; }
    public long Divisor { get; set; }
    public Func<long, bool> Test { get; set; }
    private Monkey? TrueMonkey { get; set; }
    private Monkey? FalseMonkey { get; set; }
    public long InspectedItems { get; set; }
    public long ControlNumber { get; set; }

    public void ExecuteTurn(bool divide = true)
    {
        while (Items.Any())
        {
            var item = Items.Dequeue();
            InspectedItems++;
            var processedItem = (Operation(item) / (divide ? 3 : 1)) % (divide ? long.MaxValue : ControlNumber);
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

    private Func<long, long> MakeOperation(string opString)
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

    private Func<long, bool> MakeTest()
    {
        return (x) => x % Divisor == 0;
    }

    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumbersRegex();
}
