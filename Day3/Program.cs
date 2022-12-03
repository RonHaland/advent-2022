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

List<char> commonItems = new();
for (int i = 0; i < backpacksInput.Length; i += 3)
{
    commonItems.Add(FindCommonItem(backpacksInput.Skip(i).Take(3)));
}

Console.WriteLine("Sum of common items for groups of 3 is:");
Console.WriteLine(commonItems.Sum(ConvertToPrio));

static int ConvertToPrio(char character)
{
    if (character >= 97)
        return character - 96;
    return character - 65 + 27;
}

static char FindCommonItem(IEnumerable<string> backpacks)
{
    var cc = backpacks.Select(c => c.Distinct());
    foreach (var item in cc.First())
    {
        if (cc.Skip(1).First().Contains(item) && cc.Skip(2).First().Contains(item))
            return item;
    }
    throw new Exception("Please dont get here :O");
}