
using System.Security.Cryptography.X509Certificates;

Console.WriteLine(ConvertToPrio('p')); //97
Console.WriteLine(ConvertToPrio('P')); //65
Console.WriteLine(ConvertToPrio('Z'));


var backpacksInput = await File.ReadAllLinesAsync("Input.txt");
var backpacksWithTwoCompartments = backpacksInput.Select(b => new { one = b[..(b.Length / 2)].Distinct(), two = b[(b.Length / 2)..].Distinct() });

List<char> duplicateItems = new();

foreach (var backpackWithTwoCompartments in backpacksWithTwoCompartments)
{
    foreach (var item in backpackWithTwoCompartments.one)
    {
        if (backpackWithTwoCompartments.two.Contains(item))
            duplicateItems.Add(item);
    }
}


Console.WriteLine("Sum of priorities for duplicates is:");
Console.WriteLine(duplicateItems.Sum(ConvertToPrio));

static int ConvertToPrio(char character)
{
    if (character >= 97)
        return character - 96;
    return character - 65 + 27;
}