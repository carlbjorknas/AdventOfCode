namespace AdventOfCode;

internal class _2023_14
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Indata_2023\\14.txt");

        var grid = new Grid<char>(lines, line => line.Select(x => x));

        //var numberCycles = 1_000_000_000;
        var numberCycles = 1_000;
        for (int i = 0; i < numberCycles; i++)
        {
            //Print(grid);
            TiltNorth(grid);
            //Print(grid);
            TiltWest(grid);
            //Print(grid);
            TiltSouth(grid);
            //Print(grid);
            TiltEast(grid);
            //Print(grid);
        }

        var rowValues = grid.Rows.Select((x, i) => x.Count(c => c.Data == 'O') * (long)(grid.Rows.Count - i)).ToList();

        // 163150 wrong (did the math wrong)
        // 89317 wrong.
        // 96155 too low
        // 110128 correct
        Console.WriteLine(rowValues.Sum());
    }

    private static void Print(Grid<char> grid)
    {
        foreach (var row in grid.Rows)
        {
            Console.WriteLine(new string(row.AsData().ToArray()));
        }
        Console.WriteLine();
    }

    private static void TiltNorth(Grid<char> grid)
    {
        foreach (var cell in grid.AllCells)
        {
            if (cell.Data == 'O')
            {
                var cellsAbove = grid.GetCellsAbove(cell).Reverse().ToList();
                foreach (var cellAbove in cellsAbove)
                {
                    if (cellAbove.Data != '.')
                    {
                        if (cellAbove.Row != cell.Row - 1)
                        {
                            var cellToMoveTo = grid.Columns[cell.Col][cellAbove.Row + 1];
                            cellToMoveTo.Data = 'O';
                            cell.Data = '.';
                        }
                        break;
                    }
                    else if (cellAbove.Row == 0)
                    {
                        cellAbove.Data = 'O';
                        cell.Data = '.';
                    }
                }
            }
        }
    }

    private static void TiltSouth(Grid<char> grid)
    {
        foreach (var cell in grid.AllCells.Reversed())
        {
            if (cell.Data == 'O')
            {
                var cellsBelow = grid.GetCellsBelow(cell).ToList();
                foreach (var cellBelow in cellsBelow)
                {
                    if (cellBelow.Data != '.')
                    {
                        if (cellBelow.Row != cell.Row + 1)
                        {
                            var cellToMoveTo = grid.Columns[cell.Col][cellBelow.Row - 1];
                            cellToMoveTo.Data = 'O';
                            cell.Data = '.';
                        }
                        break;
                    }
                    else if (cellBelow.Row == grid.Rows.Count-1)
                    {
                        cellBelow.Data = 'O';
                        cell.Data = '.';
                    }
                }
            }
        }
    }

    private static void TiltWest(Grid<char> grid)
    {
        foreach (var cell in grid.Columns.SelectMany(c => c))
        {
            if (cell.Data == 'O')
            {
                var cellsLeft = grid.GetCellsLeftOf(cell).Reverse().ToList();
                foreach (var cellLeft in cellsLeft)
                {
                    if (cellLeft.Data != '.')
                    {
                        if (cellLeft.Col != cell.Col - 1)
                        {
                            var cellToMoveTo = grid.Rows[cell.Row][cellLeft.Col + 1];
                            cellToMoveTo.Data = 'O';
                            cell.Data = '.';
                        }
                        break;
                    }
                    else if (cellLeft.Col == 0)
                    {
                        cellLeft.Data = 'O';
                        cell.Data = '.';
                    }
                }
            }
        }
    }

    private static void TiltEast(Grid<char> grid)
    {
        foreach (var cell in grid.Columns.SelectMany(c => c).Reverse())
        {
            if (cell.Data == 'O')
            {
                var cellsRight = grid.GetCellsRightOf(cell).ToList();
                foreach (var cellRight in cellsRight)
                {
                    if (cellRight.Data != '.')
                    {
                        if (cellRight.Col != cell.Col + 1)
                        {
                            var cellToMoveTo = grid.Rows[cell.Row][cellRight.Col - 1];
                            cellToMoveTo.Data = 'O';
                            cell.Data = '.';
                        }
                        break;
                    }
                    else if (cellRight.Col == grid.Columns.Count-1)
                    {
                        cellRight.Data = 'O';
                        cell.Data = '.';
                    }
                }
            }
        }
    }
}

