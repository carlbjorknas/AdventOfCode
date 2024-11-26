using FluentAssertions;

internal class _2023_15
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Indata_2023\\15.txt");

        var commands = lines[0].Split(',');
        //var hashes = commands.Select(Hash).ToList();
        var map = Box(commands);

        var focusingPowers = map
            .Where(x => x.Value?.Any() == true)
            .Select(x => CalculateFocusingPowerForBox(x.Key, x.Value))
            .ToList();

        Console.WriteLine(focusingPowers.Sum());
    }

    private static int CalculateFocusingPowerForBox(int key, List<(string Label, int FocalLength)> lenses)
    {
        return lenses.Select((l, i) => CalculateFocusingPowerForLens(key, l, i+1)).Sum();
    }

    private static int CalculateFocusingPowerForLens(int key, (string Label, int FocalLength) lens, int positionInList)
    {
        return (key + 1) * positionInList * lens.FocalLength;
    }

    public static Dictionary<int, List<(string Label, int FocalLength)>> Box(string[] commands)
    {
        var boxes = new Dictionary<int, List<(string Label, int FocalLength)>>();

        foreach (var command in commands)
        {
            var add = command.Contains('=');

            var label = add
                ? command.Substring(0, command.Length - 2)
                : command.Substring(0, command.Length - 1);

            var focalLength = add
                ? int.Parse(command.Last().ToString())
                : -1;

            var key = Hash(label);
            if (!boxes.ContainsKey(key))
                boxes[key] = new List<(string Label, int FocalLength)>();

            var lenses = boxes[key];            

            var index = lenses.FindIndex(x => x.Label == label);
            if (add)
            {                
                if (index == -1)
                    lenses.Add((label, focalLength));
                else
                    lenses[index] = (label, focalLength);
            }
            else if (index != -1)
                lenses.RemoveAt(index);
        }

        return boxes;
    }

    public static int Hash(string str)
    {
        var hash = 0;

        foreach(var c in str)
        {
            hash += c;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }
}

