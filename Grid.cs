namespace AdventOfCode;

internal class Grid<T>
{
    public static (Grid<T> Grid, List<(int Row, int Col)> translatedPositions) CreateGrid(List<(int Row, int Col)> positions)
    {
        int rowMin = positions.Select(x => x.Row).Min();
        int rowMax = positions.Select(x => x.Row).Max();
        int colMin = positions.Select(x => x.Col).Min();
        int colMax = positions.Select(x => x.Col).Max();

        var height = rowMax - rowMin;
        var width = colMax - colMin;

        var rows = new List<List<Cell<T>>>();
        for (int i = 0; i <= height; i++)
        {
            var row = new List<Cell<T>>();
            for (int j = 0; j <= width; j++)
            {
                row.Add(new Cell<T>(i, j, default));
            }
            rows.Add(row);
        }

        var rowTranslation = -rowMin;
        var colTranslation = -colMin;

        var translated = positions.Select(x => (x.Row + rowTranslation, x.Col + colTranslation)).ToList();

        return (new Grid<T>(rows), translated);
    }

    public Grid(IEnumerable<string> rows, Func<string, IEnumerable<T>> convertRowStringToCellDataCollection)
            : this(rows
                .Select((row, i) => (RowIndex: i, DataList: convertRowStringToCellDataCollection(row)))
                .Select(x => x.DataList.Select((cellData, colIndex) => new Cell<T>(x.RowIndex, colIndex, cellData)).ToList())
                .ToList())
    { }

    public Grid(IEnumerable<List<Cell<T>>> cellRows)
    {
        Rows = cellRows.ToList();
        RowsReversed = Rows
            .Select(row => row.Reversed().ToList())
            .ToList();
        Columns = Enumerable.Range(0, NumberCols)
            .Select(i => Rows.Select(row => row[i]).ToList())
            .ToList();
        ColumnsReversed = Columns
            .Select(x => x.Reversed().ToList())
            .ToList();
        AllCells = Rows.SelectMany(row => row).ToList();
    }

    public int NumberRows => Rows.Count;
    public int NumberCols => Rows[0].Count;
    public List<Cell<T>> AllCells { get; }
    public List<T> AllData => AllCells.Select(x => x.Data).ToList();
    public List<List<Cell<T>>> Rows { get; }
    public List<List<Cell<T>>> RowsReversed { get; }
    public List<List<Cell<T>>> Columns { get; }
    public List<List<Cell<T>>> ColumnsReversed { get; }

    internal Cell<T> GetCell((int Row, int Col) position)
        => Rows[position.Row][position.Col];

    internal IEnumerable<Cell<T>> GetCellsRightOf(Cell<T> cell)
        => Rows[cell.Row].Skip(cell.Col + 1);

    internal IEnumerable<Cell<T>> GetCellsLeftOf(Cell<T> cell)
        => Rows[cell.Row].Take(cell.Col);

    internal IEnumerable<Cell<T>> GetCellsBelow(Cell<T> cell)
        => Columns[cell.Col].Skip(cell.Row + 1);

    internal IEnumerable<Cell<T>> GetCellsAbove(Cell<T> cell)
        => Columns[cell.Col].Take(cell.Row);

    public (Cell<T>? Top, Cell<T>? Right, Cell<T>? Bottom, Cell<T>? Left) GetCellsConnectedTo(Cell<T> cell)
    {
        return (
            GetCellsAbove(cell).Reverse().Take(1).FirstOrDefault(),
            GetCellsRightOf(cell).Take(1).FirstOrDefault(),
            GetCellsBelow(cell).Take(1).FirstOrDefault(),
            GetCellsLeftOf(cell).Reverse().Take(1).FirstOrDefault());
    }

    public IEnumerable<Cell<T>> CellsAddjacentTo(Cell<T> cell)
    {
        var adjacentPositions = new[]
        {
            (cell.Row-1, cell.Col-1), (cell.Row-1, cell.Col), (cell.Row-1, cell.Col+1),
            (cell.Row, cell.Col-1),                           (cell.Row, cell.Col+1),
            (cell.Row+1, cell.Col-1), (cell.Row+1, cell.Col), (cell.Row+1, cell.Col+1),
        };

        return GetCellsOnValidPositions(adjacentPositions);
    }

    public bool IsValidPosition((int Row, int Col) position)
        => IsValidRowIndex(position.Row) && IsValidColIndex(position.Col);

    public bool IsValidRowIndex(int value) 
        => value >= 0 && value < NumberRows;

    public bool IsValidColIndex(int value)
        => value >= 0 && value < NumberCols;

    public IEnumerable<Cell<T>> GetCellsOnValidPositions(IEnumerable<(int Row, int Col)> positions)
    {
        return positions
            .Where(x => IsValidRowIndex(x.Row) && IsValidColIndex(x.Col))
            .Select(x => Rows[x.Row][x.Col]);
    }

    public void Print()
    {
        Print(cellData => cellData!.ToString()!);
    }

    public void Print(Func<T, string> cellDataToString)
    {
        foreach (var row in Rows)
        {
            foreach (var cell in row)
            {
                Console.Write(cellDataToString(cell.Data));
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

class Cell<T>
{
    public Cell(int row, int col, T data)
    {
        Row = row;
        Col = col;
        Data = data;
    }

    public int Row { get; set; }
    public int Col { get; set; }
    public T Data { get; set; }

    public override string ToString()
    {
        return $"Row: {Row}, Col: {Col}, Data: [{Data}]";
    }
}

static class CellExtensions {
    public static IEnumerable<T> AsData<T>(this IEnumerable<Cell<T>> cells)
        => cells.Select(c => c.Data);
}

static class ListExtensions
{
    public static IEnumerable<T> Reversed<T>(this List<T> list)
        => list.AsEnumerable().Reverse();
}
