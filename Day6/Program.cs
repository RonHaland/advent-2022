var input = await File.ReadAllTextAsync("Input.txt");
//var signal = "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg".ToQueue();

Console.WriteLine("Start-of-packet: {0}", ProcessSignal(input.ToQueue(), 4));
Console.WriteLine("Start-of-message: {0}", ProcessSignal(input.ToQueue(), 14));

static int ProcessSignal(Queue<char> signal, int uniqueCount)
{
    Queue<char> messageBuffer = new();
    int messageStart = uniqueCount;
    while (messageBuffer.Count < messageStart)
    {
        messageBuffer.Enqueue(signal.Dequeue());
    }

    while (signal.Count > 0)
    {
        if (messageBuffer.Count == messageBuffer.Distinct().Count())
        {
            return messageStart;
        }
        messageBuffer.Dequeue();
        messageBuffer.Enqueue(signal.Dequeue());
        messageStart++;
    }
    return -1;
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