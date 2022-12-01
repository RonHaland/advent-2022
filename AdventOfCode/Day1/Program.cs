﻿var foods = await File.ReadAllLinesAsync("Input.txt");
var elves = new List<Elf>();
Elf currentElf = new(0);

foreach (var food in foods)
{
    if (!string.IsNullOrEmpty(food) && int.TryParse(food, out var cals))
    {
        currentElf.Calories += cals;
        continue;
    }

    elves.Add(new(currentElf.Calories));
    currentElf = new(0);
        
}

var mostCalorieRichElf = elves.OrderBy(e => e.Calories).Last();
Console.WriteLine(mostCalorieRichElf.Calories);

internal sealed record Elf(int Calories) 
{
    public int Calories { get; set; } = Calories;
};
