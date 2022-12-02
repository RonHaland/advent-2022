var strategy = await File.ReadAllLinesAsync("Input.txt");

var score = strategy.Sum(x => Score(x.Split(' ').First(), x.Split(' ').Last()));

Console.WriteLine("Score for this strategy is:");
Console.WriteLine(score);


Console.WriteLine();

var score2 = strategy.Sum(x => AltScore(x.Split(' ').First(), x.Split(' ').Last()));

Console.WriteLine("Score when XYZ is outcome will be:");
Console.WriteLine(score2);

Console.ReadLine();


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

static int AltScore(string opponent, string result)
{
    var valOppo = (int)opponent[0] - 64;
    var desiredOutcome = (int)result[0] - 89;

    var oppoAction = (Action)valOppo;
    var action = ReverseRPS(oppoAction, desiredOutcome);

    var matchScore = desiredOutcome * 3 + 3;
   
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

static Action ReverseRPS(Action oppo, int result)
{
    if (!new[] { -1, 0, 1 }.Contains(result))
        throw new IndexOutOfRangeException();
    if (result == 0)
        return oppo;
    if (result == 1)
    {
        if (oppo == Action.Scissors)
            return Action.Rock;
        return (Action)(int)oppo + 1;
    }
    if (oppo == Action.Rock)
        return Action.Scissors;
    return (Action)(int)oppo - 1;
}

internal enum Action
{
    Rock = 1,
    Paper = 2,
    Scissors = 3,
}