using AdventOfCode;

internal class _2023_18
{

    enum State { Outside, Inside, OnBorder }

    public static void Run()
    {
        var lines = File.ReadAllLines("Indata_2023\\18.txt");

        var instructions = lines.Select(line => Convert(line)).ToList();

        var trench = GetTrench(instructions);

        var rowMin = trench.Keys.Min();
        var rowMax = trench.Keys.Max();

        long sum = 0;
        for (var rowIndex = rowMin; rowIndex <= rowMax; rowIndex++)
        {
            var digs = trench[rowIndex].OrderBy(x => x.Col).ToList();

            if (rowIndex == rowMax)
            {
                Console.WriteLine("Sista?");
            }

            //Console.WriteLine(string.Join(" ", digs.Select(x => x.Type)));

            var state = State.Outside;
            var prevState = State.Outside;

            for (int i = 0; i < digs.Count; i++)
            {
                var currentDig = digs[i];
                if (state is State.Outside)
                {
                    state = currentDig.Type switch
                    {
                        '|' => State.Inside,
                        'F' or 'L' => State.OnBorder
                    };
                }
                else
                {
                    var prevDig = digs[i - 1];

                    if (state == State.Inside)
                    {
                        sum += currentDig.Col - prevDig.Col;

                        state = currentDig.Type switch
                        {
                            '|' => State.Outside,
                            'F' or 'L' => State.OnBorder
                        };
                    }
                    else if (state == State.OnBorder)
                    {
                        (int diff, state) = (prevDig.Type, currentDig.Type) switch
                        {
                            ('F', '7') or ('L', 'J') => (currentDig.Col - prevDig.Col, prevState),
                            (_, _) => (currentDig.Col - prevDig.Col, prevState is State.Inside ? State.Outside : State.Inside)
                        };
                        sum += diff;
                    }
                }

                if (state == State.Outside)
                    sum++;

                if (state != State.OnBorder)
                    prevState = state;
            }
        }

        Console.WriteLine(sum);

        // Test correct 952408144115
        // Test result  952406268976
        //              952405561911
        //              952407216920
        //              952408144115
    }

    private static Dictionary<int, List<(int Col, char Type)>>
        GetTrench(List<(Direction Dir, int Steps)> instructions)
    {
        Dictionary<int, List<(int, char)>> trench = [];

        var startPos = (Row: 0, Col: 0);
        var currentPos = startPos;
        // Start is to the right and last is up.
        trench.Add(currentPos.Row, [(currentPos.Col, 'F')]);

        foreach (var (instruction, next) in instructions.Zip(instructions.Skip(1)))
        {
            if (instruction.Dir is Direction.Up or Direction.Down)
            {
                for (int i = 0; i < instruction.Steps; i++)
                {
                    currentPos = currentPos.Move(instruction.Dir);

                    char c = '|';
                    if (i == instruction.Steps - 1)
                        c = TranslateFromVertical(instruction.Dir, next.Dir);

                    if (trench.TryGetValue(currentPos.Row, out var list))
                        list.Add((currentPos.Col, c));
                    else
                        trench.Add(currentPos.Row, [(currentPos.Col, c)]);
                }
            }
            else
            {
                currentPos = currentPos.Move(instruction.Dir, instruction.Steps);
                char c = TranslateFromHorizontal(instruction.Dir, next.Dir);
                trench[currentPos.Row].Add((currentPos.Col, c));
            }
        }

        // Last is up
        var lastInstr = instructions.Last();

        // WARNING An extra -1 here, because else it became wrong on test data. But why? do they dig the first hole twice? 
        for (int i = 0; i < lastInstr.Steps-1; i++)
        {
            currentPos = currentPos.Move(lastInstr.Dir);
            trench[currentPos.Row].Add((currentPos.Col, '|'));
        }

        return trench;
    }

    private static char TranslateFromHorizontal(Direction prev, Direction next)
        => prev is Direction.Left
        ? next switch { Direction.Up => 'L', Direction.Down => 'F' }
        : next switch { Direction.Up => 'J', Direction.Down => '7' };

    private static char TranslateFromVertical(Direction prev, Direction next)
    {
        if (prev == Direction.Up)
            return next switch { Direction.Left => '7', Direction.Right => 'F' };
        return next switch { Direction.Left => 'J', Direction.Right => 'L' };
    }

    private static (Direction Direction, int Steps) Convert(string line)
    {
        // R 6 (#70c710)
        var parts = line.Split(' ');
        var hex = parts[2];
        var stepsPart = hex.Substring(2, 5);
        var steps = int.Parse(stepsPart, System.Globalization.NumberStyles.HexNumber);
        var dir = hex[7] switch
        {
            '0' => Direction.Right,
            '1' => Direction.Down,
            '2' => Direction.Left,
            '3' => Direction.Up
        };

        return (dir, steps);
    }

    // Part 1
    //public static void Run()
    //{
    //    var lines = File.ReadAllLines("Indata_2023\\18.txt");

    //    var instructions = lines.Select(line => Convert(line)).ToList();

    //    var trench = DigTrench(instructions);

    //    var (grid, translatedTrench) = Grid<string>.CreateGrid(trench);

    //    grid.AllCells.ForEach(c => c.Data = ".");

    //    foreach( var pos in translatedTrench)
    //    {
    //        grid.GetCell(pos).Data = "#";
    //    }

    //    grid.Print();

    //    DigOutInterior(grid);

    //    grid.Print();

    //    Console.WriteLine(grid.AllData.Count(d => d == "#"));
    //}

    //private static void DigOutInterior(Grid<string> grid)
    //{
    //    var startPos = GetStartOfDigOut(grid);

    //    var posToDig = new HashSet<(int Row, int Col)>
    //    {
    //        startPos
    //    };

    //    while (posToDig.Any())
    //    {
    //        var pos = posToDig.First();
    //        var cell = grid.GetCell(pos);
    //        cell.Data = "#";

    //        var (c1, c2, c3, c4) = grid.GetCellsConnectedTo(cell);
    //        List<Cell<string>> neighbours = [c1, c2, c3, c4];
    //        neighbours.Where(n => n.Data != "#").ToList().ForEach(x => posToDig.Add((x.Row, x.Col)));

    //        posToDig.Remove(pos);
    //    }
    //}

    //private static (int, int) GetStartOfDigOut(Grid<string> grid)
    //{
    //    foreach(var row in grid.Rows)
    //    {
    //        var trenchCells = row.Where(c => c.Data == "#").ToList();
    //        if (trenchCells.Count > 2)
    //            continue;

    //        // Finns en ogrävd cell som har en och endast en grävd cell på vardera sida (vänster, höger).
    //        if (trenchCells[0].Col + 1 < trenchCells[1].Col)
    //            return (trenchCells[0].Row, trenchCells[0].Col + 1);
    //    }

    //    throw new Exception("Hittade ingen startcell");
    //}

    //private static List<(int Row, int Col)> DigTrench(List<(Direction Direction, int Steps)> instructions)
    //{
    //    List<(int Row, int Col)> trench = [];
    //    var currentPos = (0, 0);
    //    trench.Add(currentPos);
    //    foreach(var instr in instructions)
    //    {
    //        for (int i = 0; i < instr.Steps; i++)
    //        {
    //            currentPos = currentPos.Move(instr.Direction);
    //            trench.Add(currentPos);
    //        }
    //    }

    //    return trench;
    //}

    //public static (Direction Direction, int Steps) Convert(string line)
    //{
    //    var parts = line.Split(' ');

    //    var direction = parts[0] switch
    //    {
    //        "R" => Direction.Right,
    //        "L" => Direction.Left,
    //        "U" => Direction.Up,
    //        "D" => Direction.Down,
    //    };

    //    var steps = int.Parse(parts[1]);

    //    return (direction, steps);
    //}
}