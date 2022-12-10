var input = await File.ReadAllLinesAsync("Input.txt");

var cycle = 1;
var X = 1;
List<int> signals = new();

for (int i = 0; i < input.Length; i++)
{
    cycle++;
    SignalStrength(X, cycle, signals);
    if (input[i] == "noop")
        continue;

    cycle++;
    X += int.Parse(input[i].Split(' ')[1]);
    SignalStrength(X, cycle, signals);
}

Console.WriteLine("{0}",signals.Sum());

void SignalStrength(int reg, int cycle, List<int> signals)
{
    if ((cycle + 20) % 40 == 0)
    {
        signals.Add(reg * cycle);
        Console.WriteLine("{0}: {1}", cycle, reg * cycle);
    }
}