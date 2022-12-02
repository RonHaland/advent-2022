var strategy = await File.ReadAllLinesAsync("Input.txt");

var score = strategy.Sum(x => Score(x.Split(' ').First(), x.Split(' ').Last()));

Console.WriteLine("Score for this strategy is:");
Console.WriteLine(score);
Console.ReadLine();
//A rock X
//B paper Y
//C scissors Z

static int Score(string opponent, string you)
{
    var valOppo = (int)opponent[0] - 64;
    var valYou = (int)you[0] - 87;

    var oppoAction = (Action)valOppo;
    var action = (Action)valYou;
    var match = RPS(oppoAction, action);
    var matchScore = match * 3 + 3;

    return matchScore + (int)action;
}

static int RPS(Action oppo, Action you)
{
    var arr = new[] { oppo, you }; 
    if (oppo == you)
        return 0;
    if (arr.Contains(Action.Scissors) && arr.Contains(Action.Rock))
        return (you < oppo) ? 1 : -1;
    return (you < oppo) ? -1 : 1;
}

internal enum Action
{
    Rock = 1,
    Paper = 2,
    Scissors = 3,
}