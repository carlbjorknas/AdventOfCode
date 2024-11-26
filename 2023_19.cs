using AdventOfCode;
using System.Data;

class Rule(string? propName, bool lessThan, int value, string destination)
{
    public Rule(string destination) : this(null, false, 0, destination) { }

    public string? PropName { get; } = propName;
    public bool LessThan { get; } = lessThan;
    public int Value { get; } = value;
    public string Destination { get; } = destination;

    public static Rule Parse(string value)
    {
        // m>2090:A
        // rfg
        if (value.Contains(':'))
        {
            var parts = value.Split(':');
            var mathParts = parts[0].Split(["<", ">"], StringSplitOptions.None);
            var lessThan = parts[0].Contains('<');
            return new Rule(mathParts[0], lessThan, int.Parse(mathParts[1]), parts[1]);
        }

        return new Rule(value);
    }

    public override string ToString()
    {
        return $"{PropName} {(LessThan ? '<' : '>')} {Value}";
    }
}

record PartRatings(int x, int m, int a, int s);

internal class _2023_19
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Indata_2023\\19.txt");
        var parts = lines.SplitAtEmptyLines().ToList();
       
        var workflows = ParseWorkflows(parts[0]);

        var partRatings = parts[1].Select(x =>
            {
                var parts = x.Split(["=", ",", "{", "}"], StringSplitOptions.RemoveEmptyEntries);
                return new PartRatings(int.Parse(parts[1]), int.Parse(parts[3]), int.Parse(parts[5]), int.Parse(parts[7]));
            })
            .ToList();

        var A = new List<PartRatings>();
        var R = new List<PartRatings>();

        foreach(var partRating in partRatings)
        {
            Console.WriteLine($"Sorting {partRating}");

            var rules = workflows["in"];
            while (true)
            {
                string? workflowName = null;
                foreach (var rule in rules)
                {
                    Console.Write($"Evaluating {rule} => ");

                    if (rule.PropName is null)
                    {
                        Console.WriteLine($"No rule matched.");
                        workflowName = rule.Destination;
                        break;
                    }

                    var value = rule.PropName switch
                    {
                        "x" => partRating.x,
                        "m" => partRating.m,
                        "s" => partRating.s,
                        "a" => partRating.a
                    };

                    if ((rule.LessThan && value < rule.Value) || (!rule.LessThan && value > rule.Value))
                    {
                        workflowName = rule.Destination;
                        Console.WriteLine($"TRUE");
                        break;
                    }
                    else
                        Console.WriteLine($"FALSE");
                }

                if (workflowName is "A")
                {
                    Console.WriteLine($"ACCEPTED");
                    A.Add(partRating);
                    break;
                }
                else if (workflowName is "R")
                {
                    Console.WriteLine($"REJECTED");
                    R.Add(partRating);
                    break;
                }
                else
                {
                    Console.WriteLine($"New destination is '{workflowName}'");
                    rules = workflows[workflowName];
                }

            }
        }

        Console.WriteLine(A.Select(x => (long)x.a + x.s + x.x + x.m).Sum());
    }

    private static Dictionary<string, List<Rule>> ParseWorkflows(List<string> lines)
    {
        Dictionary<string, List<Rule>> rulesDict = [];
        // px{a<2006:qkq,m>2090:A,rfg}
        foreach (var line in lines)
        {
            var parts = line.Split(new[] { "{", "}", "," }, StringSplitOptions.RemoveEmptyEntries);
            var name = parts[0];
            var rules = parts.Skip(1).Select(Rule.Parse).ToList();
            rulesDict.Add(name, rules);
        }

        return rulesDict;
    }
}