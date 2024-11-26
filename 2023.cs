namespace AdventOfCode;

internal static class _2023
{
    class SpringRow
    {
        public SpringRow(string str)
        {
            var parts = str.Split(" ");
            Template = string.Join("?", Enumerable.Repeat(parts[0], 5));
            Arrangements = string.Join(",", Enumerable.Repeat(parts[1], 5)).Split(",").Select(int.Parse).ToList();
        }

        public string Template { get; }
        public List<int> Arrangements { get; }
        //public List<string> Variations { get; private set; }

        public List<string> CreateVariations()
        {
            var variations = new List<string> { "" };
            foreach(var c in Template)
            {
                if (c is '.' or '#')
                    variations = variations.Select(x => x + c).ToList();
                else
                {
                    var temp = variations.Select(x => x + '.').ToList();
                    temp.AddRange(variations.Select(x => x + '#'));
                    variations = temp.ToList();
                }
                variations = variations.Where(IsValidSoFar).ToList();
            }

            return variations;
        }

        //public int NumberValidVariations()
        //{
        //    return Variations.Where(IsValidVariation).Count();
        //}

        private bool IsValidSoFar(string variation)
        {
            if (variation.EndsWith('#'))
                return true;

            var consecutives = variation
                .Split(".", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Length)
                .ToList();

            if (consecutives.Count > Arrangements.Count)
                return false;            

            for (int i = 0; i < consecutives.Count; i++)
            {
                if (consecutives[i] != Arrangements[i])
                    return false;
            }

            var rest = Arrangements.Skip(consecutives.Count).ToList();
            var minRequiredLength = rest.Sum() + rest.Count - 1;
            if (Template.Length - variation.Length < minRequiredLength)
                return false;

            return true;
        }

        public bool IsValidVariation(string variation)
        {
            var consecutives = variation
                .Split(".", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Length)
                .ToList();

            if (consecutives.Count == Arrangements.Count)
            {
                for (int i = 0; i < consecutives.Count; i++)
                {
                    if (consecutives[i] != Arrangements[i])
                        return false;
                }
                return true;
            }
            return false;            
        }
    }

    public static void Run()
    {
        // Day 5 ****************************************************
        //var lines = File.ReadAllLines("Indata_2023\\5.txt");
        //Console.WriteLine("");

        // Day 12 ****************************************************

        // https://www.youtube.com/watch?v=g3Ms5e7Jdqo

        var lines = File.ReadAllLines("Indata_2023\\12.txt");

        //var springRows = lines.Select(x => new SpringRow(x)).ToList();
        //springRows.ForEach(x => x.CreateVariations());

        //var validVariations = springRows.Select(x => x.NumberValidVariations());

        Dictionary<(string, string), long> cache = new();
        List<long> variations = new();
        for (int i = 0; i<lines.Length; i++)
        {
            //var springRow = new SpringRow(lines[i]);
            //var variations = springRow.CreateVariations();
            //validCounts.Add(variations.Count(springRow.IsValidVariation));

            var parts = lines[i].Split(" ");

            //var springs = parts[0];
            //var groups = parts[1].Split(",").Select(int.Parse).ToArray();

            var springs = string.Join("?", Enumerable.Repeat(parts[0], 5));
            var groups = string.Join(",", Enumerable.Repeat(parts[1], 5)).Split(",").Select(int.Parse).ToArray();

            variations.Add(Count(springs, groups));

            Console.WriteLine($"Line {i+1} has {variations.Last()} variations.");
        }

        Console.WriteLine(variations.Sum());

        long Count(string springs, int[] groups)
        {            
            if (springs == string.Empty)
                return groups.Any() ? 0 : 1;

            if (!groups.Any())
                return springs.Contains('#') ? 0 : 1;

            var key = (springs, string.Join(",", groups));
            if (cache.ContainsKey(key))
                return cache[key];

            long variations = 0;

            if (springs[0] is '.' or '?')
                variations += Count(springs[1..], groups);

            if (springs[0] is '#' or '?')
            {
                if (
                    groups[0] <= springs.Length &&
                    !springs[..groups[0]].Contains('.') &&
                    (springs.Length == groups[0] || springs[groups[0]] != '#'))
                {
                    var skipCount = Math.Min(springs.Length, groups[0] + 1);
                    variations += Count(springs[skipCount..], groups.Skip(1).ToArray());
                }
            }

            cache[key] = variations;
            return variations;
        }

        // 5500044 Too high

        //Line 1 has 504684 variations.
        //Line 2 has 759375 variations.
        //Line 3 has 5139 variations.
        //Line 4 has 32 variations.
        //Line 5 has 330160 variations.
        //Line 6 has 103680 variations.
        //Line 7 has 40000 variations.
        //Line 8 has 15799 variations.

        Console.WriteLine(variations.Sum());

        // Day 11 ****************************************************

        //class GalaxyPair
        //{
        //    public GalaxyPair(Cell<char> p1, Cell<char> p2)
        //    {
        //        P1 = p1;
        //        P2 = p2;

        //        Distance = Math.Abs(p1.Col - p2.Col) + Math.Abs(p1.Row - p2.Row);
        //    }

        //    public int Distance { get; }
        //    public Cell<char> P1 { get; }
        //    public Cell<char> P2 { get; }
        //}

        //var lines = File.ReadAllLines("Indata_2023\\11.txt").ToList();

        //// Expand
        ////for (int i = 0; i < lines.Count; i++)
        ////{
        ////    if (lines[i].All(c => c == '.'))
        ////    {
        ////        lines.Insert(i, lines[i]);
        ////        i++;
        ////    }
        ////}

        ////for (int i=0; i < lines[0].Length; i++)
        ////{
        ////    if (lines.All(l => l[i] == '.'))
        ////    {
        ////        lines = lines.Select(l => l.Insert(i, ".")).ToList();
        ////        i++;
        ////    }
        ////}

        //var grid = new Grid<char>(lines, row => row.Select(x => x));

        //var galaxies = grid.AllCells.Where(c => c.Data == '#').ToList();

        //// Expand
        //const int expansion = 999_999;
        //var expandingRowIndices = grid.Rows.Where(x => x.All(c => c.Data == '.')).Select(x => x.First().Row).ToList();
        //expandingRowIndices.Reversed().ToList()
        //    .ForEach(x => galaxies.Where(g => g.Row > x).ToList().ForEach(g => g.Row = g.Row + expansion));

        //var expandingColIndices = grid.Columns.Where(x => x.All(c => c.Data == '.')).Select(x => x.First().Col).ToList();
        //expandingColIndices.Reversed().ToList()
        //    .ForEach(x => galaxies.Where(g => g.Col > x).ToList().ForEach(g => g.Col = g.Col + expansion));

        //var pairs = galaxies.SelectMany((g, i) => galaxies.Skip(i + 1).Select(g2 => new GalaxyPair(g, g2))).ToList();

        //// 9790852 Too low
        //// 10313550 correct
        //// Part 2: 611998701561 too high
        //// Part 2: 82000210 too low. Fel data, använde testdatat.
        //// Part 2: 611998089572
        //Console.WriteLine(pairs.Select(x => (long)x.Distance).Sum());


        // Day 10 ****************************************************

        //class Pipe
        //{
        //    public Pipe(char type)
        //    {
        //        Type = type;
        //    }

        //    public char Type { get; set; }
        //    public bool Visited { get; set; }
        //    public int Distance { get; set; }
        //    public Pipe From { get; set; }
        //    public Pipe To { get; set; }
        //    public bool Inside { get; set; }
        //}

        //var lines = File.ReadAllLines("Indata_2023\\10.txt");

        //var grid = new Grid<Pipe>(lines, row => row.Select(c => new Pipe(c)));

        //var startCell = grid.AllCells.First(x => x.Data.Type == 'S');
        //startCell.Data.Visited = true;
        //startCell.Data.Distance = 0;

        //var currentPipeCell = startCell;
        //Cell<Pipe> prev;
        //while (currentPipeCell != null)
        //{                        
        //    prev = currentPipeCell;
        //    currentPipeCell = WalkToNextPipeCell(currentPipeCell);  

        //    if (currentPipeCell != null)
        //    {
        //        currentPipeCell.Data.Visited = true;
        //        currentPipeCell.Data.From = prev.Data;
        //        prev.Data.To = currentPipeCell.Data;
        //    }
        //}

        //var numberVisited = grid.AllData.Where(x => x.Visited).Count();
        //var farthest = numberVisited / 2;

        //Console.WriteLine(farthest);

        //SetStartCellType(startCell);

        //foreach(var row in grid.Rows)
        //{
        //    bool inside = false;
        //    bool? lastBendWasUpwards = null;
        //    foreach (var cell in row)
        //    {
        //        if (!cell.Data.Visited)
        //            cell.Data.Inside = inside;
        //        else {
        //            if (cell.Data.Type == '|')
        //                inside = !inside;
        //            else if (cell.Data.Type == '-')
        //            {
        //                // Do nothing
        //            }
        //            else if (IsBend(cell))
        //            {
        //                if (lastBendWasUpwards == null)
        //                    lastBendWasUpwards = IsBentUpward(cell);
        //                else
        //                {
        //                    var goesBack = lastBendWasUpwards == IsBentUpward(cell);
        //                    if (!goesBack)
        //                        inside = !inside;
        //                    lastBendWasUpwards = null;
        //                }
        //            }
        //        }
        //    }
        //}

        //bool IsBend(Cell<Pipe> cell) 
        //    => cell.Data.Type is 'J' or 'F' or 'L' or '7';
        //bool IsBentUpward(Cell<Pipe> cell)
        //    => cell.Data.Type is 'J' or 'L';


        //var enclosed = grid.AllData.Count(x => x.Inside);

        //// 1712 Too high
        //// 399 Too low
        //Console.WriteLine(enclosed);

        //Cell<Pipe>? WalkToNextPipeCell(Cell<Pipe> current)
        //{
        //    var (top, right, bottom, left) = grid.GetCellsConnectedTo(current);
        //    if (CanConnectUpwards(current.Data.Type) && top != null && !top.Data.Visited && CanConnectDownwards(top.Data.Type))
        //        return top;
        //    if (CanConnectToRight(current.Data.Type) && right != null && !right.Data.Visited && CanConnectToLeft(right.Data.Type))
        //        return right;
        //    if (CanConnectDownwards(current.Data.Type) && bottom != null && !bottom.Data.Visited && CanConnectUpwards(bottom.Data.Type))
        //        return bottom;
        //    if (CanConnectToLeft(current.Data.Type) && left != null && !left.Data.Visited && CanConnectToRight(left.Data.Type))
        //        return left;

        //    return null;
        //}

        //bool CanConnectUpwards(char c)
        //    => c is '|' or 'J' or 'L' or 'S';

        //bool CanConnectDownwards(char c)
        //    => c is '|' or '7' or 'F' or 'S';

        //bool CanConnectToLeft(char c)
        //    => c is '-' or '7' or 'J' or 'S';

        //bool CanConnectToRight(char c)
        //    => c is '-' or 'L' or 'F' or 'S';

        //void SetStartCellType(Cell<Pipe> start)
        //{
        //    var (top, right, bottom, left) = grid.GetCellsConnectedTo(start);

        //    if (top != null && CanConnectDownwards(top.Data.Type))
        //    {
        //        if (right != null && CanConnectToLeft(right.Data.Type))
        //            start.Data.Type = 'L';
        //        if (left != null && CanConnectToRight(left.Data.Type))
        //            start.Data.Type = 'J';
        //        else
        //            start.Data.Type = '|';
        //    }
        //    else if (bottom != null && CanConnectUpwards(bottom.Data.Type))
        //    {
        //        if (right != null && CanConnectToLeft(right.Data.Type))
        //            start.Data.Type = 'F';
        //        if (left != null && CanConnectToRight(left.Data.Type))
        //            start.Data.Type = '7';
        //    }
        //    else
        //        start.Data.Type = '-';

        //    Console.WriteLine($"Start har ersatts med '{start.Data.Type}'.");
        //}


        // Day 9 ****************************************************

        //class History
        //{
        //    List<List<long>> series = new();

        //    public History(string line)
        //    {
        //        var firstSerie = line.Split(' ').Select(long.Parse).ToList();
        //        series.Add(firstSerie);
        //        CalculateSeries();
        //    }

        //    private void CalculateSeries()
        //    {
        //        while(series.Last().Any(x => x != 0))
        //        {
        //            var nextSerie = series.Last()
        //                .Zip(series.Last().Skip(1))
        //                .Select(pair => pair.Second - pair.First)
        //                .ToList();
        //            series.Add (nextSerie);
        //        }
        //    }

        //    public long Extrapolate()
        //    {
        //        series.Last().Add(0);
        //        for (int i = series.Count - 2; i >= 0; i--)
        //        {
        //            var s = series[i];
        //            var next = series[i + 1];
        //            s.Add(s.Last() + next.Last());
        //        }
        //        return series.First().Last();
        //    }

        //    public long ExtrapolateBackwards()
        //    {
        //        series.Last().Add(0);
        //        for (int i = series.Count - 2; i >= 0; i--)
        //        {
        //            var s = series[i];
        //            var next = series[i + 1];
        //            s.Insert(0, s.First() - next.First());
        //        }
        //        return series.First().First();
        //    }

        //    public void Print()
        //    {
        //        series.ForEach(s =>
        //        {
        //            Console.WriteLine(string.Join(" ", s));
        //        });
        //    }
        //}

        //var lines = File.ReadAllLines("Indata_2023\\9.txt");

        //var histories = lines.Select(x => new History(x)).ToList();
        //histories.ForEach(x => { x.Print(); Console.WriteLine(); });

        ////var extras = histories.Select(h => h.Extrapolate()).ToList();
        //var backwards = histories.Select(x => x.ExtrapolateBackwards()).ToList();

        //Console.WriteLine(backwards.Sum());

        // Day 8 ****************************************************

        //class Node
        //{
        //    public Node(string name) 
        //    {
        //        Name = name;
        //    }

        //    public string Name { get; }
        //    public Node Left { get; private set; }
        //    public Node Right { get; private set; }
        //    public Node Parent { get; private set; }

        //    public void SetLeft(Node left)
        //    {
        //        Left = left;
        //        Left.Parent = this;
        //    }

        //    public void SetRight(Node right)
        //    {
        //        Right = right;
        //        right.Parent = this;
        //    }

        //    public override string ToString()
        //    {
        //        return $"{Name} {Left?.Name} {Right?.Name} {Parent?.Name}";
        //    }
        //}

        //        var lines = File.ReadAllLines("Indata_2023\\8.txt");
        //        //var lines = File.ReadAllLines("Indata_2023\\8_test.txt");

        //        var path = lines[0];

        //        Dictionary<string, Node> nodes = new();
        //        InitNodes();
        //        var headNode = FindHeadNode();
        //        var steps = Traverse(path);

        //        // 2: 1333600941 Too low (int)
        //        // 1408514752349 Too low 
        //        // 16449085039026865463698093 Too high (BigInteger)
        //        // 378890468381881
        //        // 11678319315857 https://www.calculatorsoup.com/calculators/math/lcm.php

        //        /*
        //15871, 15871, 15871, 15871, 15871, 15871

        //21251, 21251, 21251, 21251

        //16409, 16409, 16409, 16409, 16409, 16409

        //11567, 11567, 11567, 11567, 11567, 11567, 11567, 11567, 11567

        //18023, 18023, 18023, 18023, 18023

        //14257, 14257, 14257, 14257, 14257, 14257, 14257
        //         */
        //        Console.WriteLine(steps);



        //        BigInteger Traverse(string path)
        //        {
        //            int i = 0;
        //            var currentNodes = nodes.Where(x => x.Key.EndsWith('A')).Select(x => x.Value).ToList();

        //            var trips = Enumerable.Range(0, 6).Select(_ => new List<long>()).ToList();

        //            while (trips.All(x => x.Count < 10))
        //            {
        //                if (currentNodes.Any(x => x.Name.EndsWith('Z')))
        //                {
        //                    for (var k=0; k < currentNodes.Count; k++)
        //                    {
        //                        if (currentNodes[k].Name.EndsWith('Z'))
        //                            trips[k].Add(i);
        //                    }
        //                }
        //                var c = path[(int)(i % path.Length)];
        //                i++;
        //                currentNodes = currentNodes.Select(n => c == 'L' ? n.Left : n.Right).ToList();                
        //            }

        //            for (var k = 0; k < trips.Count; k++)
        //            {
        //                var pairs = trips[k].Zip(trips[k].Skip(1)).ToList();
        //                Console.WriteLine(string.Join(", ", pairs.Select(x => x.Second - x.First)));
        //                Console.WriteLine();
        //            }

        //            //var pairs = trips.Zip(trips.Skip(1)).Select(x => BigInteger.GreatestCommonDivisor(x.First, x.Second)).ToList();
        //            //var test = pairs.Aggregate(new BigInteger(1), (a, b) => a * b);

        //            //var t = BigInteger.GreatestCommonDivisor(1, 10);
        //            //var bcd = BigInteger.GreatestCommonDivisor(trips[0], trips[1]);
        //            //var test = trips.Aggregate(new BigInteger(1), (a, b) => a * BigInteger.GreatestCommonDivisor(a, b));
        //            return new BigInteger(0);
        //        }

        //        void InitNodes()
        //        {
        //            foreach (var line in lines.Skip(2))
        //            {
        //                var node = new Node(line[..3]);
        //                var left = new Node(line[7..10]);
        //                var right = new Node(line[12..15]);

        //                if (left.Name == right.Name)
        //                    left = right;

        //                if (nodes.ContainsKey(node.Name))
        //                    node = nodes[node.Name];

        //                if (nodes.ContainsKey(left.Name))
        //                    left = nodes[left.Name];

        //                if (nodes.ContainsKey(right.Name))
        //                    right = nodes[right.Name];

        //                node.SetLeft(left);
        //                node.SetRight(right);

        //                nodes.TryAdd(node.Name, node);
        //                nodes.TryAdd(left.Name, left);
        //                nodes.TryAdd(right.Name, right);
        //            }
        //        }

        //        Node FindHeadNode()
        //        {
        //            var currentNode = nodes.Values.First();
        //            while (currentNode.Parent != null)
        //            {
        //                currentNode = currentNode.Parent;
        //            }
        //            return currentNode;
        //        }

        // Day 7 ****************************************************

        //public class Hand : IComparable<Hand>
        //{
        //    private static Dictionary<char, int> _values = new Dictionary<char, int>
        //    {
        //        { '2', 2 },
        //        { '3', 3 },
        //        { '4', 4 },
        //        { '5', 5 },
        //        { '6', 6 },
        //        { '7', 7 },
        //        { '8', 8 },
        //        { '9', 9 },
        //        { 'T', 10 },
        //        { 'J', 1 },
        //        { 'Q', 12 },
        //        { 'K', 13 },
        //        { 'A', 14 },
        //    };

        //    public Hand(string handStr)
        //    {
        //        var parts = handStr.Split(" ");
        //        Cards = parts[0].Select(c => _values[c]).ToList();

        //        var jokerCount = Cards.Count(x => x == 1);
        //        var groups = Cards.Where(x => x > 1).GroupBy(x => x);
        //        if (jokerCount == 5 || groups.Any(x => x.Count() + jokerCount == 5))
        //            Type = 7;
        //        else if (groups.Any(x => x.Count() + jokerCount == 4))
        //            Type = 6;
        //        else if (
        //            (jokerCount == 0 && groups.Any(x => x.Count() == 3) && groups.Any(x => x.Count() == 2)) ||
        //            (jokerCount == 1 && (groups.Any(x => x.Count() == 3) || groups.Count(x => x.Count() == 2) == 2)) ||
        //            (jokerCount == 2 && groups.Any(x => x.Count() == 2)) || 
        //            jokerCount == 3)
        //            Type = 5;
        //        else if (groups.Any(x => x.Count() + jokerCount == 3))
        //            Type = 4;
        //        else if (
        //            groups.Count(x => x.Count() == 2) == 2 || 
        //            jokerCount == 2 || 
        //            (groups.Count(x => x.Count() == 2) == 1 && jokerCount == 1))
        //            Type = 3;
        //        else if (groups.Any(x => x.Count() == 2) || jokerCount == 1)
        //            Type = 2;
        //        else
        //            Type = 1;

        //        Bid = int.Parse(parts[1]);
        //    }

        //    public List<int> Cards { get; }
        //    public int Type { get; }
        //    public int Bid { get; }

        //    int IComparable<Hand>.CompareTo(Hand? other)
        //    {
        //        if (other == null || Type > other.Type) return 1;
        //        if (Type < other.Type) return -1;

        //        foreach(var i in Enumerable.Range(0, 5))
        //        {
        //            if (Cards[i] > other.Cards[i]) return 1;
        //            if (Cards[i] < other.Cards[i]) return -1;
        //        }

        //        return 0;
        //    }

        //    public override string ToString()
        //    {
        //        var cardStr = string.Join(",", Cards.Select(c => $"{c,2}"));
        //        return $"{cardStr} {Type} {Bid}";
        //    }
        //}

        //var lines = File.ReadAllLines("Indata_2023\\7.txt");

        //var orderedCards = lines.Select(x => new Hand(x)).OrderBy(x => x).ToList();

        //var winnings = orderedCards.Select((card, i) => (long)card.Bid * (i + 1)).ToList();

        //// Part 1 250602641 - correct
        //// Part 2 250563472 - too low
        //// 250563472
        //// 251037509
        //Console.WriteLine(winnings.Sum());

        // Day 6 ****************************************************
        //var lines = File.ReadAllLines("Indata_2023\\6.txt");

        ////var time = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
        ////var distance = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();

        ////var pairs = time.Zip(distance).ToList();

        ////var results = pairs.Select(x => FindPossibilites(x.First, x.Second)).ToList();

        ////var product = results.Select(x => x.Count()).Aggregate(1, (a, b) => a * b);

        //var time = int.Parse(lines[0].Replace(" ", "").Remove(0, "Time:".Length));
        //var distance = long.Parse(lines[1].Replace(" ", "").Remove(0, "Distance:".Length));

        //var xMin = (long)Math.Ceiling(time / 2.0 - Math.Sqrt(Math.Pow(time / 2.0, 2) - distance));
        //var xMax = (long)Math.Floor(time / 2.0 + Math.Sqrt(Math.Pow(time / 2.0, 2) - distance));

        ////var products = results.ForEach(x => x.Select(x => x.Time).Aggregate(1, (a, b) => a * b))

        //// 26592951 Too high
        //// 1155175 Correct
        ////Console.WriteLine(product);
        //Console.WriteLine(xMax - xMin + 1);

        //List<(int Time, int Distance)> FindPossibilites(int time, int recordDistance)
        //{
        //    return Enumerable.Range(0, time)
        //        .Select(x => (Time: x, Distance: (time - x) * x))
        //        .Where(x => x.Distance > recordDistance)
        //        .ToList();
        //}

        // Day 5 ****************************************************

        //public class Map
        //{
        //    public Map(string name, List<RangeMap> rangeMaps)
        //    {
        //        Name = name;
        //        RangeMaps = rangeMaps;
        //    }

        //    public string Name { get; }
        //    public List<RangeMap> RangeMaps { get; }

        //    public long Convert(long value)
        //    {
        //        var rangeMap = RangeMaps.FirstOrDefault(x => x.ValidFor(value));

        //        if (rangeMap == null)
        //        {
        //            //Console.WriteLine($"Found no matching conversion in {Name}. Returning {value}.");
        //            return value;
        //        }

        //        var convertedValue = rangeMap.Convert(value);
        //        //Console.WriteLine($"Found matching conversion in {Name}. Returning {convertedValue}.");
        //        return convertedValue;
        //    }

        //    public IEnumerable<Range> MapRange(Range range)
        //    {
        //        var unmappedRanges = new List<Range> { range };
        //        var mappedRanges = new List<Range>();
        //        foreach (var rangeMap in RangeMaps)
        //        {
        //            unmappedRanges = rangeMap.Map(unmappedRanges, mappedRanges);
        //        }

        //        return unmappedRanges.Concat(mappedRanges);
        //    }
        //}

        //public class RangeMap
        //{
        //    private long _diff;

        //    public RangeMap(long fromStart, long toStart, long count)
        //    {
        //        FromStart = fromStart;
        //        FromEnd = fromStart + count - 1;
        //        _diff = toStart - fromStart;
        //    }

        //    public long FromStart { get; }
        //    public long FromEnd { get; }

        //    public bool ValidFor(long value)
        //        => FromStart <= value && value <= FromEnd;

        //    public long Convert(long value)
        //        => value + _diff;

        //    public List<Range> Map(List<Range> unmappedRanges, List<Range> mappedRanges)
        //    {
        //        var newUnmappedRanges = new List<Range>();

        //        foreach (var unmappedRange in unmappedRanges)
        //        {
        //            // Finns ett överlapp?
        //            if (FromEnd >= unmappedRange.Start && FromStart <= unmappedRange.End)
        //            {
        //                // Överlappar mappningen helt?
        //                if (FromStart <= unmappedRange.Start && FromEnd >= unmappedRange.End)
        //                    mappedRanges.Add(new Range(unmappedRange.Start + _diff, unmappedRange.Count));
        //                // Överlappar mappningen undre delen?
        //                else if (FromStart <= unmappedRange.Start)
        //                {
        //                    mappedRanges.Add(new Range(unmappedRange.Start + _diff, FromEnd - unmappedRange.Start + 1));
        //                    newUnmappedRanges.Add(new Range(FromEnd + 1, unmappedRange.End - FromEnd));
        //                }
        //                // Överlappar mappningen övre delen?
        //                else if (FromEnd >= unmappedRange.End)
        //                {
        //                    mappedRanges.Add(new Range(FromStart + _diff, unmappedRange.End - FromStart + 1));
        //                    newUnmappedRanges.Add(new Range(unmappedRange.Start, FromStart - unmappedRange.Start));
        //                }
        //                // Mappningen överlappas helt.
        //                else
        //                {
        //                    newUnmappedRanges.Add(new Range(unmappedRange.Start, FromStart - unmappedRange.Start));
        //                    mappedRanges.Add(new Range(FromStart, FromEnd - FromStart + 1));
        //                    newUnmappedRanges.Add(new Range(FromEnd + 1, unmappedRange.End - FromEnd));
        //                }
        //            }
        //            else
        //            {
        //                newUnmappedRanges.Add(unmappedRange);
        //            }
        //        }

        //        return newUnmappedRanges;
        //    }

        //}

        //public class Range
        //{
        //    public Range(long start, long count)
        //    {
        //        Start = start;
        //        Count = count;
        //        End = Start + Count - 1;
        //    }

        //    public long Start { get; }
        //    public long End { get; }
        //    public long Count { get; }

        //    public override string ToString() => $"{Start} - {End}, {Count}";
        //}

        //var lines = File.ReadAllLines("Indata_2023\\5.txt");

        //    var groups = lines.SplitAtEmptyLines().ToList();

        //    var seeds = groups[0][0].Remove(0, "seeds: ".Length).Split(" ").Select(long.Parse).ToList();
        //    var seedRanges = seeds.Chunk(2).Select(chunk => new Range(chunk[0], chunk[1])).ToList();  

        //    var maps = groups.Skip(1).Select(ConvertToMap).ToList();

        //    var locations = seeds.Select(MapIt).ToList();
        //    var mappedRanges = seedRanges.SelectMany(MapRange).ToList();

        //    var min = mappedRanges.Min(x => x.Start);

        //    // 6950361 too low
        //    // 29599454 too low
        //    Console.WriteLine(locations.Min());

        //    // 20283860 Correct for part 2.
        //    //Console.WriteLine(newMin);

        //    static Map ConvertToMap(List<string> list)
        //    {
        //        var rangeMaps = list
        //            .Skip(1)
        //            .Select(x => x.Split(" ").Select(long.Parse).ToList())
        //            .Select(x => new RangeMap(x[1], x[0], x[2]))
        //            .ToList();

        //        return new Map(list.First(), rangeMaps);
        //    }

        //    long MapIt(long value)
        //    {
        //        var newValue = value;
        //        maps.ForEach(map => newValue = map.Convert(newValue));
        //        return newValue;
        //    }

        //    IEnumerable<Range> MapRange(Range range)
        //    {
        //        Console.WriteLine($"Starting mapping of range {range}");

        //        var ranges = new List<Range> { range };
        //        List<Range> mappedRanges = new List<Range>();
        //        var i = 1;
        //        foreach(var map in maps)
        //        {
        //            foreach(var r in ranges)
        //            {
        //                mappedRanges.AddRange(map.MapRange(r));
        //            }

        //            ranges.Clear();
        //            ranges.AddRange(mappedRanges);
        //            mappedRanges.Clear();

        //            Console.WriteLine($"After {i} round of mapping:");
        //            ranges.ForEach(Console.WriteLine);

        //            i++;
        //        }

        //        return ranges;
        //    }

        //// Day 4 ****************************************************
        //var lines = File.ReadAllLines("Indata_2023\\4.txt");

        //int id = 1;

        //var cards = lines
        //    .Select(x =>
        //    {
        //        var numbers = x.Remove(0, "Card   1: ".Length).Split(" | ");

        //        var winners = numbers[0]
        //        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        //        .Select(x => int.Parse(x))
        //        .ToList();

        //        var myNumbers = numbers[1]
        //        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        //        .Select(x => int.Parse(x))
        //        .ToList();

        //        var myWinningNumbers = myNumbers.Intersect(winners).ToList();

        //        return (Id: id++, NumberWon:myWinningNumbers.Count);
        //    })
        //    .ToList();

        //var wonCards = new List<int>();

        //for (int i = 0; i < cards.Count; i++)
        //{
        //    var card = cards[i];
        //    var cardCount = 1 + wonCards.Count(x => x == card.Id);
        //    Enumerable.Range(card.Id+1, card.NumberWon)
        //        .ToList()
        //        .ForEach(x => wonCards.AddRange(Enumerable.Repeat(x, cardCount)));
        //}

        //// 1139 too low
        //// 5744979 correct!
        //Console.WriteLine(cards.Count + wonCards.Count);

        // Day 3 ****************************************************

        //class CellData
        //{
        //    public CellData(char c)
        //    {
        //        C = c;
        //        IsNumber = C >= '0' && C <= '9';
        //        IsSymbol = C != '.' && !IsNumber;            
        //    }

        //    public char C { get; }        
        //    public bool AdjacentToSymbol { get; set; }
        //    public bool IsNumber { get; }
        //    public bool IsSymbol { get; }
        //    public int PartNumber { get; set; }

        //    public override string ToString()
        //    {
        //        return $"{IsNumber} {IsSymbol} {(AdjacentToSymbol ? "JA" : "NÄ")}";
        //    }
        //}

        //var lines = File.ReadAllLines("Indata_2023\\3.txt");

        //var grid = new Grid<CellData>(lines, RowToCellData);

        //var symbolCells = grid.AllCells.Where(x => x.Data.IsSymbol).ToList();
        //symbolCells.ForEach(cell => grid.CellsAddjacentTo(cell).ToList().ForEach(x => x.Data.AdjacentToSymbol = true));

        //var numberCells = grid.Rows.SelectMany(FindPartNumbers);

        //// 525489 Too low
        //// 528819 Correct!
        //Console.WriteLine(numberCells.Sum());

        //var starCells = grid.AllCells.Where(x => x.Data.C == '*').ToList();
        //var gearRatios = starCells
        //    .Select(GetAdjacentPartNumbers)
        //    .Where(x => x.Count == 2)
        //    .Select(x => x.First() * x.Last())
        //    .ToList();

        //// 80403602
        //Console.WriteLine(gearRatios.Sum());

        //List<int> GetAdjacentPartNumbers(Cell<CellData> cell)
        //{
        //    return grid!.CellsAddjacentTo(cell)
        //        .Select(c => c.Data.PartNumber)
        //        .Where(x => x != 0)
        //        .Distinct()
        //        .ToList();
        //}

        //IEnumerable<CellData> RowToCellData(string row)
        //{
        //    return row.Select(c => new CellData(c));
        //}

        //IEnumerable<int> FindPartNumbers(IEnumerable<Cell<CellData>> row)
        //{
        //    var i = 0;
        //    var rowList = row.ToList();
        //    var numberCells = new List<Cell<CellData>>();
        //    while(i < rowList.Count)
        //    {
        //        var cell = rowList[i];
        //        if (cell.Data.IsNumber)
        //            numberCells.Add(cell);
        //        else if (numberCells.Any())
        //        {
        //            if (TryConvertToPartNumber(numberCells, out var partNumber))
        //                yield return partNumber;
        //            numberCells.Clear();
        //        }

        //        i++;
        //    }

        //    if (TryConvertToPartNumber(numberCells, out var lastPartNumber))
        //        yield return lastPartNumber;

        //    bool TryConvertToPartNumber(List<Cell<CellData>> cells, out int partNumber)
        //    {
        //        partNumber = -1;
        //        if (cells.Any(x => x.Data.AdjacentToSymbol))
        //        {
        //            var pn = int.Parse(new string(cells.Select(cell => cell.Data.C).ToArray()));
        //            partNumber = pn;
        //            cells.ForEach(c => c.Data.PartNumber = pn);
        //        }

        //        return partNumber != -1;

        //    }
        //}

        // Day 2 ****************************************************

        //// Game 1: 1 green, 1 blue, 1 red; 3 green, 1 blue, 1 red; 4 green, 3 blue, 1 red; 4 green, 2 blue, 1 red; 3 blue, 3 green
        //var lines = File.ReadAllLines("Indata_2023\\2.txt");

        //var cubeAmount = (Red: 12, Green: 13, Blue: 14);

        //var games = lines.Select(ConvertLine).ToList();

        //var possibleGames = games
        //    .Select(g => (g.Id, Red: g.Reveals.Max(x => x.Red), Green: g.Reveals.Max(x => x.Green), Blue: g.Reveals.Max(x => x.Blue)))
        //    .Where(g => g.Red <= cubeAmount.Red && g.Green <= cubeAmount.Green && g.Blue <= cubeAmount.Blue)
        //    .Select(g => g.Id)
        //    .ToList();

        //Console.WriteLine(possibleGames.Sum());

        //var fewest = games
        //    .Select(g => (g.Id, Red: g.Reveals.Max(x => x.Red), Green: g.Reveals.Max(x => x.Green), Blue: g.Reveals.Max(x => x.Blue)))
        //    .Select(g => g.Red * g.Green * g.Blue)
        //    .ToList();

        //Console.WriteLine(fewest.Sum());

        //(int Id, List<(int Red, int Green, int Blue)> Reveals) ConvertLine(string line)
        //{
        //    var parts = line.Split(new[] { ": ", "; " }, StringSplitOptions.None);
        //    var id = int.Parse(parts[0][5..]);

        //    // 1 green, 1 blue, 1 red
        //    var colorList = parts.Skip(1).Select(x =>
        //    {
        //        var cols = x.Split(", ");
        //        int r, g, b;
        //        r = g = b = 0;

        //        var redStr = cols.FirstOrDefault(x => x.Contains("red"));                
        //        if (redStr != null)
        //        {
        //            r = int.Parse(redStr.Remove(redStr.IndexOf(' ')));
        //        }

        //        var greenStr = cols.FirstOrDefault(x => x.Contains("green"));
        //        if (greenStr != null)
        //        {
        //            g = int.Parse(greenStr.Remove(greenStr.IndexOf(' ')));
        //        }

        //        var blueStr = cols.FirstOrDefault(x => x.Contains("blue"));
        //        if (blueStr != null)
        //        {
        //            b = int.Parse(blueStr.Remove(blueStr.IndexOf(' ')));
        //        }

        //        return (r, g, b);
        //    })
        //    .ToList();

        //    return (id, colorList);
        //}

        // Day 1 ***********************************************************************
        //var lines = File.ReadAllLines("Indata_2023\\1.txt");

        //var sum = lines.Select(x => (first: GetFirstDigit(x), last: GetLastDigit(x)))
        //    .Select(x => x.first * 10 + x.last)
        //    .Sum();

        //Console.WriteLine(sum);

        //int GetFirstDigit(string line)
        //{
        //    var list = new List<(int index, int value)>
        //    {
        //        FindIndex(line, "0", 0),
        //        FindIndex(line, "1", 1),
        //        FindIndex(line, "2", 2),
        //        FindIndex(line, "3", 3),
        //        FindIndex(line, "4", 4),
        //        FindIndex(line, "5", 5),
        //        FindIndex(line, "6", 6),
        //        FindIndex(line, "7", 7),
        //        FindIndex(line, "8", 8),
        //        FindIndex(line, "9", 9),
        //        FindIndex(line, "zero", 0),
        //        FindIndex(line, "one", 1),
        //        FindIndex(line, "two", 2),
        //        FindIndex(line, "three", 3),
        //        FindIndex(line, "four", 4),
        //        FindIndex(line, "five", 5),
        //        FindIndex(line, "six", 6),
        //        FindIndex(line, "seven", 7),
        //        FindIndex(line, "eight", 8),
        //        FindIndex(line, "nine", 9)
        //    };

        //    return list.Where(x => x.index != -1).OrderBy(x => x.index).First().value;
        //}

        //int GetLastDigit(string line)
        //{
        //    var list = new List<(int index, int value)>
        //    {
        //        FindLastIndex(line, "0", 0),
        //        FindLastIndex(line, "1", 1),
        //        FindLastIndex(line, "2", 2),
        //        FindLastIndex(line, "3", 3),
        //        FindLastIndex(line, "4", 4),
        //        FindLastIndex(line, "5", 5),
        //        FindLastIndex(line, "6", 6),
        //        FindLastIndex(line, "7", 7),
        //        FindLastIndex(line, "8", 8),
        //        FindLastIndex(line, "9", 9),
        //        FindLastIndex(line, "zero", 0),
        //        FindLastIndex(line, "one", 1),
        //        FindLastIndex(line, "two", 2),
        //        FindLastIndex(line, "three", 3),
        //        FindLastIndex(line, "four", 4),
        //        FindLastIndex(line, "five", 5),
        //        FindLastIndex(line, "six", 6),
        //        FindLastIndex(line, "seven", 7),
        //        FindLastIndex(line, "eight", 8),
        //        FindLastIndex(line, "nine", 9)
        //    };

        //    return list.Where(x => x.index != -1).OrderByDescending(x => x.index).First().value;
        //}



        //(int index, int value) FindIndex(string line, string strValue, int value)
        //{
        //    return (line.IndexOf(strValue), value);
        //}

        //(int index, int value) FindLastIndex(string line, string strValue, int value)
        //{
        //    return (line.LastIndexOf(strValue), value);
        //}
    }
}
