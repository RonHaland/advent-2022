var signal = (await File.ReadAllTextAsync("Input.txt")).ToQueue();
//var signal = "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg".ToQueue();

Queue<char> buffer = new();
while (buffer.Count < 4)
{
    buffer.Enqueue(signal.Dequeue());
}

int signalStart = 4;
while(signal.Count > 0)
{
    if (buffer.Count == buffer.Distinct().Count())
    {
        Console.WriteLine("Done with loop {0}", signalStart);
        break;
    }
    buffer.Dequeue();
    buffer.Enqueue(signal.Dequeue());
    signalStart++;
}

internal static class StringExt
{
    internal static Queue<char> ToQueue(this string input)
    {
        Queue<char> queue = new();
        foreach (char c in input)
        {
            queue.Enqueue(c);
        }
        return queue;
    }
}