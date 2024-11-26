namespace AdventOfCode;

static internal class StringArrayExtensions
{
    public static IEnumerable<List<string>> SplitAtEmptyLines(this string[] lines)
    {
        var group = new List<string>();
        foreach(var line in lines)
        {
            if (line.Length == 0)
            {
                yield return group;
                group = new List<string>();
            }
            else
            {
                group.Add(line);
            }
        }

        yield return group;
    }
}
