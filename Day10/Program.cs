var input = await File.ReadAllLinesAsync("Input.txt");

var cycle = 1;
var X = 1;
List<int> signals = new();
var lines = Enumerable.Range(0, 6).Select(l => "").ToList();

for (int i = 0; i < input.Length; i++)
{
    IncrementCycle(X);
    SignalStrength(X, cycle, signals);
    if (input[i] == "noop")
        continue;

    IncrementCycle(X);
    X += int.Parse(input[i].Split(' ')[1]);
    SignalStrength(X, cycle, signals);
}

Console.WriteLine("{0}",signals.Sum());
Console.WriteLine(string.Join(Environment.NewLine, lines));

void SignalStrength(int reg, int cycle, List<int> signals)
{
    if ((cycle + 20) % 40 == 0)
    {
        signals.Add(reg * cycle);
        Console.WriteLine("{0}: {1}", cycle, reg * cycle);
    }
}

void IncrementCycle(int reg)
{
    if (cycle % 40 >= reg && cycle % 40 <= reg + 2)
        lines[(cycle-1) / 40] += "#";
    else
        lines[(cycle-1) / 40] += ".";

    cycle++;
}