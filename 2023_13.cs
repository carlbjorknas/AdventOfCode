namespace AdventOfCode;

internal class _2023_13
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Indata_2023\\13.txt");
        var patterns = lines.SplitAtEmptyLines();

        var sum = 0;

        foreach (var pattern in patterns)
        {
            var grid = new Grid<char>(pattern, row => row.Select(c => c));

            var verticalIndex = FindVerticalLine(grid);
            if (verticalIndex != null)
                sum += verticalIndex.Value;
            else
                sum += FindHorizontalLine(grid) * 100;
        }

        Console.WriteLine(sum);
    }

    private static int? FindVerticalLine(Grid<char> grid)
    {        
        for (var i = 1; i < grid.Columns.Count; i++)
        {
            var left = grid.Columns.Select(c => c).Take(i).Reverse().ToList();
            var right = grid.Columns.Skip(i).ToList();

            var columnPairs = left.Zip(right).ToList();
            //if (columnPairs.All(pair => pair.First.Select(c => c.Data).SequenceEqual(pair.Second.Select(c => c.Data))))
            if (HasExactlyOneDifference(columnPairs))
                return i;
        }

        return null;
    }

    private static int FindHorizontalLine(Grid<char> grid)
    {
        for (var i = 1; i < grid.Rows.Count; i++)
        {
            var left = grid.Rows.Take(i).Reverse().ToList();
            var right = grid.Rows.Skip(i).ToList();

            var rowPairs = left.Zip(right).ToList();
            //if (columnPairs.All(pair => pair.First.Select(c => c.Data).SequenceEqual(pair.Second.Select(c => c.Data))))
            if (HasExactlyOneDifference(rowPairs))
                return i;

        }

        throw new Exception();
    }

    private static bool HasExactlyOneDifference(List<(List<Cell<char>>, List<Cell<char>>)> pairs)
    {
        bool foundOne = false;
        foreach(var pair in pairs)
        {
            var chars1 = pair.Item1.Select(c => c.Data).ToList();
            var chars2 = pair.Item2.Select(c => c.Data).ToList();

            for (var i = 0; i < chars1.Count; i++)
            {
                if (chars1[i] != chars2[i])
                {
                    if (!foundOne)
                        foundOne = true;
                    else
                        return false;
                }
                    
            }
        }

        return foundOne;
    }
}

