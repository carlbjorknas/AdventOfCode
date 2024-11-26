// See https://aka.ms/new-console-template for more information
using AdventOfCode;

//GridUsageExample.Run();
//return;

_2023_19.Run();
//_2023.Run();
return;

// Day 5 2022
//var lines = File.ReadAllLines("Indata_2022\\Day5.txt");
//Console.WriteLine("");

//Console.WriteLine("Hello, World!");

// Day 12 2022
//var lines = File.ReadAllLines("Indata_2022\\Day12.txt");

//var dest = (0, 0);
//var rows = lines.Select((x, row) => x.Select((c, col) => ConvertToMap(c, row, col)).ToList()).ToList();
//var startPoints = FindStartPoints().ToList();

//var shortest = startPoints.Select(FindShortestPath).Min();
//Console.WriteLine(shortest);

//IEnumerable <(int col, int row)> FindStartPoints()
//{
//    for (int row = 0; row < rows.Count; row++)
//    {
//        for (int col = 0; col < rows[0].Count; col++)
//        {
//            if (rows[row][col].Height == 'a')
//                yield return (col, row);
//        }
//    }
//}

//int FindShortestPath((int, int) start)
//{
//    rows = lines.Select((x, row) => x.Select((c, col) => ConvertToMap(c, row, col)).ToList()).ToList();
//    var toVisit = new Queue<(int col, int row, string path)>();
//    toVisit.Enqueue((start.Item1, start.Item2, ""));

//    while (rows[dest.Item2][dest.Item1] is (_, null))
//    {
//        if (!toVisit.Any())
//            return int.MaxValue;
//        var v = toVisit.Dequeue();
//        var cell = rows[v.row][v.col];
//        if (cell is (_, null))
//        {
//            rows[v.row][v.col] = (cell.Height, v.path);
//            AddIfVisitable(v.col, v.row - 1, v.path, "U", cell.Height, toVisit);
//            AddIfVisitable(v.col, v.row + 1, v.path, "D", cell.Height, toVisit);
//            AddIfVisitable(v.col + 1, v.row, v.path, "R", cell.Height, toVisit);
//            AddIfVisitable(v.col - 1, v.row, v.path, "L", cell.Height, toVisit);
//        }
//    }
//    // 472
//    Console.WriteLine(rows[dest.Item2][dest.Item1].Path.Length + " " + rows[dest.Item2][dest.Item1].Path);

//    return rows[dest.Item2][dest.Item1].Path.Length;
//}
//void AddIfVisitable(int col, int row, string path, string dir, int height, Queue<(int col, int row, string path)> toVisit)
//{
//    if (col < 0 || col >= rows[0].Count || row < 0 || row >= rows.Count)
//        return;

//    var cell = rows[row][col];
//    if (cell is (_, null) &&
//        height >= cell.Height - 1)
//        toVisit.Enqueue((col, row, path + dir));
//}

//(int Height, string? Path) ConvertToMap(char c, int row, int col)
//{
//    if (c == 'S')
//    {
//        return ('a', null);
//    }
//    else if (c == 'E')
//    {
//        dest = (col, row);
//        return ('z', null);
//    }
//    return (c, null);
//}

// Day 11 2022
//var lines = File.ReadAllLines("Indata_2022\\Day11.txt");
//var monkeyTexts = lines.SplitAtEmptyLines();
//var monkeys = new List<Monkey>();
//monkeyTexts.ToList().ForEach(txt =>
//{
//    var startingItems = txt[1][18..].Split(", ").Select(x => int.Parse(x)).ToList();
//    var operationArr = txt[2][23..].Split(" ");
//    var op = operationArr[0];
//    var opValue = operationArr[1];
//    var divider = int.Parse(txt[3][21..]);

//    Func<long, long> operation = (oldValue) =>
//    {
//        var value = opValue == "old" ? oldValue : int.Parse(opValue);
//        checked
//        {
//            return op == "+" ? oldValue + value : oldValue * value;
//        }
//    };

//    var trueId = int.Parse(txt[4][29..]);
//    var falseId = int.Parse(txt[5][30..]);

//    var m = new Monkey(operation, divider, trueId, falseId, monkeys);
//    startingItems.ForEach(x => m.Items.Enqueue(x));
//});

//Enumerable.Range(0, 10_000).ToList().ForEach(x =>
//    {
//        if (x % 1000 == 0)
//        {
//            Console.WriteLine();
//            Console.WriteLine(x + "---");
//            monkeys.ForEach(x => Console.WriteLine(x));
//        }
//        monkeys.ForEach(m => m.Act());
//    });

//var sortedMonkeys = monkeys
//    .OrderByDescending(x => x.NumberInspections)
//    //.Select(x => $"{x.Id} {x.NumberInspections}")
//    .ToList();

////Console.WriteLine(string.Join(", ", sortedMonkeys));

//// 321339104 Too low
//Console.WriteLine((long)sortedMonkeys[0].NumberInspections * sortedMonkeys[1].NumberInspections);

//class Monkey
//{
//    public Monkey(Func<long, long> operation, int divider, int trueId, int falseId, List<Monkey> monkeys)
//    {
//        Id = monkeys.Count;
//        _operation = operation;
//        _divider = divider;
//        _trueId = trueId;
//        _falseId = falseId;
//        _monkeys = monkeys;
//        monkeys.Add(this);
//    }

//    public Queue<long> Items = new Queue<long>();
//    public int NumberInspections { get; private set; }
//    public int Id { get; }

//    private readonly Func<long, long> _operation;
//    private readonly int _divider;
//    private readonly int _trueId;
//    private readonly int _falseId;
//    private readonly List<Monkey> _monkeys;

//    public void Act()
//    {
//        while (Items.Any())
//        {
//            var worryLevel = Items.Dequeue();
//            worryLevel = _operation(worryLevel);
//            var divisible = worryLevel % _divider == 0;
//            var index = divisible ? _trueId : _falseId;

//            //if (divisible)
//            //    worryLevel /= _divider;

//            var product = _monkeys.Select(x => x._divider).Aggregate(1, (x, y) => x * y);
//            if (worryLevel > product)
//            {
//                var times = worryLevel / product;
//                worryLevel = worryLevel - (times * product);
//            }

//            _monkeys[index].Items.Enqueue(worryLevel);

//            NumberInspections++;
//        }
//    }

//    public override string ToString()
//    {
//        return $"{Id} {NumberInspections} {string.Join(", ", Items)}";
//    }
//}

//Day 10 2022
//var lines = File.ReadAllLines("Indata_2022\\Day10.txt");

//// Lägger till ett dummyvärde för att få 1-indexerad lista med de riktiga värdena.
//var registerValueHistory = new List<int> { 0 };
//var x = 1;

//foreach (var line in lines)
//{
//    if (line == "noop")
//        registerValueHistory.Add(x);
//    else
//    {
//        registerValueHistory.Add(x);
//        registerValueHistory.Add(x);
//        var value = int.Parse(line[5..]);
//        x += value;        
//    }
//}

//var signalStrengthSum = registerValueHistory
//    .Select((x, i) => x * i)
//    .Where((x, i) => i == 20 || (i - 20) % 40 == 0)
//    .Sum();

//var chunks = registerValueHistory.Skip(1).Chunk(40).ToList();
//chunks.ForEach(c => Console.WriteLine(string.Join(" ", c.Select(x => $"{x,3}"))));
//// 14320 Too high
//Console.WriteLine(signalStrengthSum);

//int cycle = 1;
//for (int i = 0; i < 6; i++)
//{
//    for (int j = 0; j < 40; j++)
//    {
//        if (j - 1 <= registerValueHistory[cycle] && registerValueHistory[cycle] <= j + 1)
//            Console.Write("#");
//        else
//            Console.Write(".");

//        cycle++;
//    }
//    Console.WriteLine();    
//}

/* PZGPKPEB
 ###..####..##..###..#..#.###..####.###..
#..#....#.#..#.#..#.#.#..#..#.#....#..#.
#..#...#..#....#..#.##...#..#.###..###..
###...#...#.##.###..#.#..###..#....#..#.
#....#....#..#.#....#.#..#....#....#..#.
#....####..###.#....#..#.#....####.###..
*/

// Day 9 2022
// Part 2, 1996 is too low.
//var lines = File.ReadAllLines("Indata_2022\\Day9.txt");

//var visitedPositions = new HashSet<(int, int)> { (0, 0) };
//var headPos = (x:0, y:0);
//var tails = new List<(int x, int y)>(Enumerable.Repeat((0, 0), 9));

//Print();

//foreach (var line in lines)
//{
//    Console.WriteLine("--> " + line);

//    var parts = line.Split(' ');
//    var dir = parts[0];
//    var steps = int.Parse(parts[1]);
//    if (dir == "U")
//        MoveKnots(() => headPos = (headPos.x, headPos.y + 1), steps);
//    else if (dir == "D")
//        MoveKnots(() => headPos = (headPos.x, headPos.y - 1), steps);
//    else if (dir == "R")
//        MoveKnots(() => headPos = (headPos.x + 1, headPos.y), steps);
//    else if (dir == "L")
//        MoveKnots(() => headPos = (headPos.x - 1, headPos.y), steps);    
//}

//Console.WriteLine(visitedPositions.Count);

//void MoveKnots(Action moveHead, int steps)
//{
//    for (int i = 0; i < steps; i++)
//    {
//        moveHead();
//        for (var tailIndex = 0; tailIndex < tails.Count; tailIndex++)
//        {
//            var lead = tailIndex == 0 ? headPos : tails[tailIndex-1]; 
//            tails[tailIndex] = MoveTail(tails[tailIndex], lead);
//        }
//        visitedPositions.Add(tails.Last());

//        //Print();
//    }    
//}

//(int x, int y) MoveTail((int x, int y) tailPos, (int x, int y) leadPos)
//{
//    var diff = (x: leadPos.x - tailPos.x, y: leadPos.y - tailPos.y);
//    if (Math.Abs(diff.x) == 2 && Math.Abs(diff.y) == 2)
//        return (tailPos.x + diff.x / 2, tailPos.y + diff.y / 2);
//    else if (Math.Abs(diff.x) == 2)
//        return (tailPos.x + diff.x / 2, tailPos.y + diff.y);
//    else if (Math.Abs(diff.y) == 2)
//        return (tailPos.x + diff.x, tailPos.y + diff.y / 2);

//    return tailPos;
//}

//void Print()
//{
//    var knots = new List<(int x, int y)> { headPos };
//    knots.AddRange(tails);
//    var minX = knots.Select(t => t.x).Min();
//    var maxX = knots.Select(t => t.x).Max();
//    var minY = knots.Select(t => t.y).Min();
//    var maxY = knots.Select(t => t.y).Max();

//    var width = maxX - minX + 1;
//    var height = maxY - minY + 1;

//    var transX = 0 - minX;
//    var transY = 0 - minY;

//    var mappedKnots = knots.Select(k => (x: k.x + transX, y: k.y + transY)).ToList();

//    for (var y = height-1;  y >= 0; y--)
//    {
//        for (var x = 0; x < width; x++)
//        {
//            var knotIndex = mappedKnots.IndexOf((x, y));
//            Console.Write(knotIndex != -1 ? knotIndex : ".");
//        }
//        Console.WriteLine();
//    }

//    Console.WriteLine();
//}

//visitedPositions.ForEach(pos => Console.WriteLine(pos));

//Console.WriteLine(visitedPositions.Count);

//int x, y, xMin, xMax, yMin, yMax;
//x = y = xMin = xMax = yMin = yMax = 0;


//foreach (var line in lines)
//{
//    var parts = line.Split(' ');
//    var dir = parts[0];
//    var steps = int.Parse(parts[1]);
//    if (dir == "U")
//    {
//        y += steps;
//        yMax = Math.Max(y, yMax);
//    }
//    else if (dir == "D")
//    {
//        y -= steps;
//        yMin = Math.Min(y, yMin);
//    }
//    else if (dir == "R")
//    {
//        x += steps;
//        xMax = Math.Max(x, xMax);
//    }
//    else if (dir == "L")
//    {
//        x -= steps;
//        xMin = Math.Min(x, xMin);
//    }
//}

////var grid = new Grid<Cell>(xMax - xMin + 1, yMax - yMin + 1);

//Console.WriteLine($"{xMin} {xMax} {yMin} {yMax}");
// -52 238 -131 52

//class Cell
//{
//    public Cell(int row, int col)
//    {
//        Row = row;
//        Col = col;
//    }

//    public bool HasHead;
//    public bool HasTail;
//    public bool IsStart;

//    public int Row { get; }
//    public int Col { get; }
//}

// Day 8 2022
//var lines = File.ReadAllLines("Indata_2022\\Day8.txt");

//var numberRows = lines.Length;
//var numberColumns = lines.First().Length;

//var trees = new List<Tree>();
//lines.ToList().ForEach(line => line.Select(x => int.Parse(x.ToString())).ToList().ForEach(x => trees.Add(new Tree(x))));

//var fromLeft = trees.Chunk(numberColumns).ToList();
//var fromRight = fromLeft.Select(x => x.Reverse().ToArray()).ToList();
//var fromTop = Enumerable.Range(0, numberColumns)
//    .Select(index => fromLeft.Select(treeLine => treeLine[index]).ToArray())
//    .ToList();
//var fromBottom = fromTop.Select(x => x.Reverse().ToArray()).ToList();

//var markVisibleTrees = (Tree[] treeLine) =>
//{
//    var lastHeight = -1;
//    foreach (var tree in treeLine)
//    {
//        if (tree.Height > lastHeight)
//        {
//            lastHeight = tree.Height;
//            tree.Visible = true;
//        }
//    }
//};

//fromLeft.ForEach(treeLine => markVisibleTrees(treeLine));
//fromRight.ForEach(treeLine => markVisibleTrees(treeLine));
//fromTop.ForEach(treeLine => markVisibleTrees(treeLine));
//fromBottom.ForEach(treeLine => markVisibleTrees(treeLine));

//var count = trees.Count(tree => tree.Visible);
////560 too low
//Console.WriteLine(count);

//List<int> scores = new();

//for (var rowIndex = 0; rowIndex < numberColumns; rowIndex++)
//{
//    for (var colIndex = 0; colIndex < numberRows; colIndex++)
//    {
//        var tree = trees[rowIndex * numberColumns + colIndex];
//        var l = fromLeft[rowIndex].Skip(colIndex+1).TakeUntil(x => x.Height >= tree.Height).Count();
//        var r = fromRight[rowIndex].Skip(numberColumns-colIndex).TakeUntil(x => x.Height >= tree.Height).Count();
//        var t = fromTop[colIndex].Skip(rowIndex+1).TakeUntil(x => x.Height >= tree.Height).Count();
//        var b = fromBottom[colIndex].Skip(numberRows-rowIndex).TakeUntil(x => x.Height >= tree.Height).Count();

//        scores.Add(l*r*t*b);
//    }
//}

//Console.WriteLine(scores.Max());

//class Tree
//{
//    public Tree(int height)
//    {
//        Height = height;
//    }

//    public int Height { get; }
//    public bool Visible { get; set; }

//    public override string ToString()
//    {
//        return $"({Height}, {Visible})";
//    }
//}


// Day 7 2022

//var lines = File.ReadAllLines("Indata_2022\\Day7.txt");

//Dir topDir = new Dir("/", null);
//Dir? currentDir = null;

//var index = 0;

//while (index < lines.Length)
//{
//    var line = lines[index];

//    if (line == "$ cd ..")
//    {
//        currentDir = currentDir.Parent;
//    }
//    else if (line.StartsWith("$ cd "))
//    {
//        var dirName = line[5..];
//        currentDir = dirName == "/" 
//            ? topDir 
//            : currentDir!.Dirs.First(d => d.Name == dirName);
//    }
//    else if (line.StartsWith("$ ls"))
//    {
//        index++;        
//        while(index < lines.Length && !lines[index].StartsWith("$"))
//        {
//            line = lines[index];
//            if (line.StartsWith("dir"))
//            {
//                var dirName = line.Substring(4);
//                currentDir!.AddDir(dirName);
//            }
//            else
//            {
//                var fileInfo = line.Split(" ");
//                currentDir!.AddFile(fileInfo[1], int.Parse(fileInfo[0]));
//            }
//            index++;
//        }
//        index--;
//    }
//    index++;
//}

////List<Dir> dirs = new();
////topDir.Visit((dir) => { if (dir.Size <= 100_000) dirs.Add(dir); });

//const int Capacity = 70_000_000;
//const int Need = 30_000_000;

//var currentlyFree = Capacity - topDir.Size;
//var amountToFree = Need - currentlyFree;

//Console.WriteLine($"Amount to free: {amountToFree}");

//List<Dir> dirs = new();
//topDir.Visit((dir) => { if (dir.Size >= amountToFree) dirs.Add(dir); });

//var smallestDir = dirs.OrderBy(x => x.Size).First();

//Console.WriteLine(smallestDir.Size);

//class Dir
//{
//    public Dir(string name, Dir? parent)
//    {
//        Name = name;
//        Parent = parent;
//    }

//    public void Visit(Action<Dir> visitor)
//    {
//        visitor(this);
//        Dirs.ForEach(d => d.Visit(visitor));
//    }

//    public int Size 
//    {
//        get
//        {
//            var size = 0;
//            Visit((dir) => size += dir.Files.Sum(x => x.Size));
//            return size;
//        }
//    }

//    public void AddDir(string name)
//    {
//        var dir = Dirs.FirstOrDefault(x => x.Name == name);
//        if (dir == null)
//            Dirs.Add(new Dir(name, this));
//    }

//    public void AddFile(string name, int size)
//    {
//        var file = Files.FirstOrDefault(x => x.Name == name);
//        if (file == null)
//            Files.Add(new AocFile(name, size));
//    }

//    public List<AocFile> Files = new();
//    public List<Dir> Dirs = new();
//    public string Name { get; }
//    public Dir? Parent { get; }

//    public override string ToString()
//    {
//        return base.ToString() + $" {Name}, dir count {Dirs.Count}, file count {Files.Count}";
//    }
//}

//record AocFile(string Name, int Size)
//{
//    public override string ToString()
//    {
//        return base.ToString() + $"{Name} - {Size}";
//    }
//}

// Day 6 2022
//var line = File.ReadAllLines("Indata_2022\\Day6.txt").First();
//var queue = new Queue<char>();

//int index = 0;

//while (queue.Distinct().Count() != 14)
//{
//    queue.Enqueue(line[index]);
//    if (queue.Count > 14)
//        queue.Dequeue();
//    index++;
//}

//Console.WriteLine(index);


// Day 5 2022
//var lines = File.ReadAllLines("Indata_2022\\Day5.txt");
//var stacksAndMovements = lines.SplitAtEmptyLines();

//var stackStrings = stacksAndMovements.First().SkipLast(1);
//var movementStrings = stacksAndMovements.Last();

//var stacks = new List<Stack<char>>();
//var crates = Enumerable.Range(0, 9)
//    .Select(i => stackStrings.Select(x => x[i * 4 + 1]).Where(x => x != ' ').ToList())    
//    .ToList();

//crates.ForEach(x => 
//{
//    var stack = new Stack<char>();
//    stacks.Add(stack);
//    ((IEnumerable<char>)x).Reverse().ToList().ForEach(c => stack.Push(c));
//});

//var movements = movementStrings
//    .Select(x => x.Split(" "))
//    .Select(x => (x[1], x[3], x[5]))
//    .Select(x => (int.Parse(x.Item1), int.Parse(x.Item2) - 1, int.Parse(x.Item3) - 1))
//    .ToList();

//movements.ForEach(x =>
//{
//    Enumerable.Range(0, x.Item1)
//        .ToList()
//        .ForEach(_ => stacks[x.Item3].Push(stacks[x.Item2].Pop()));
//});

//movements.ForEach(x =>
//{
//    crates[x.Item3].InsertRange(0, crates[x.Item2].Take(x.Item1));
//    crates[x.Item2].RemoveRange(0, x.Item1);
//});

////var tops = stacks.Select(x => x.Pop()).ToList();
//var tops = crates.Select(x => x.First()).ToList();

//Console.WriteLine(string.Join("", tops));

// Day 4 2022
//var lines = File.ReadAllLines("Indata_2022\\Day4.txt");
//var ranges = lines
//    .Select(x => x.Split(new string[] { ",", "-" }, StringSplitOptions.None))
//    .Select(x => (int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2]), int.Parse(x[3])))
//    //.Where(x =>
//    //    (x.Item1 <= x.Item3 && x.Item2 >= x.Item4) ||
//    //    (x.Item3 <= x.Item1 && x.Item4 >= x.Item2))
//    .Select(x => (Enumerable.Range(x.Item1, x.Item2 - x.Item1 + 1).ToList(), Enumerable.Range(x.Item3, x.Item4 - x.Item3 + 1).ToList()))
//    .Where(x => x.Item1.Intersect(x.Item2).Any())
//    .ToList();


//Console.WriteLine(ranges.Count);

// Day 3 2022
//var lines = File.ReadAllLines("Indata_2022\\Day3.txt");

//var chunks = lines.Chunk(3).ToList();
//var badges = chunks
//    .Select(x => x[0].Intersect(x[1]).Intersect(x[2]).First())
//    .ToList();
//var sum = badges
//   .Select(x => (int)x) // A = 65, a = 97
//   .Select(x => x >= 97 ? (x - 96) : (x - 65 + 27))
//   .Sum();

//Console.WriteLine(sum);

//var chars = lines
//    .Select(x => (x, x.Length))
//    .Select(x => (x.x[..(x.Length / 2)], x.x[(x.Length / 2)..]))
//    .Select(x => x.Item1.Intersect(x.Item2).First())
//    .ToList();
// var sum = chars
//    .Select(x => (int)x) // A = 65, a = 97
//    .Select(x => x >= 97 ? (x - 96) : (x - 65 + 27))
//    .Sum();

//Console.WriteLine(sum);


// Day 2 2022
//var lines = File.ReadAllLines("Indata_2022\\Day2.txt");
//var results = lines.Select(x =>
//{
//    var pair = x.Split(" ");
//    var p1 = pair[0] switch
//    {
//        "A" => 1,
//        "B" => 2,
//        "C" => 3
//    };

//    var zp1 = p1 - 1;
//    var p2 = pair[1].First() switch
//    {
//        'X' => (zp1 + 2) % 3,
//        'Y' => zp1,
//        'Z' => (zp1 + 1) % 3
//    };
//    p2++;

//    var outcome = 0;
//    if (p1 == p2)
//        outcome = 3;
//    else if ((p1, p2) is (1, 2) or (2, 3) or (3, 1))
//        outcome = 6;

//    return p2 + outcome;
//}).ToList();
//var sum = results.Sum();

//Console.WriteLine(sum);



// Day 1 2022
//var lines = File.ReadAllLines("Indata_2022\\Day1.txt");
//var groups = lines.SplitAtEmptyLines();

//var calorieGroups = groups.Select(foods => foods.Select(food => int.Parse(food)).ToList());
//var groupsOfSums = calorieGroups.Select(x => x.Sum());
//var max = groupsOfSums.Max();

//var sumOfTop3 = groupsOfSums.OrderDescending().Take(3).Sum();

//Console.WriteLine(lines.Length);
