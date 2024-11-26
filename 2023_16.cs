using AdventOfCode;

internal class _2023_16
{
    public class Tile
    {        
        public Tile(char type)
        {
            Type = type;
        }

        public char Type { get; }

        public HashSet<Direction> VisitedBy { get; } = new HashSet<Direction>();

        public bool Energized => VisitedBy.Count > 0;

        public override string ToString()
            => $"{Type} {Energized} [{string.Join(',', VisitedBy)}]";
    }

    public class Beam
    {
        private HashSet<(Direction, (int Row, int Col))> _history = new();

        public Beam(Direction direction, (int Row, int Col) position)
        {
            Direction = direction;
            Position = position;
            _history.Add((Direction, position));
        }

        public Direction Direction { get; set; }
        public (int Row, int Col) Position { get; set; }

         public bool Move()
        {
            Position = Position.Move(Direction);

            // return true if looped.
            return !_history.Add((Direction, Position));
        }

        internal void ActOnRightLeaningMirror()
        {
            // Mirror /
            Direction = Direction switch
            {
                Direction.Left or Direction.Right => Direction.TurnLeft(),
                Direction.Up or Direction.Down => Direction.TurnRight()
            };
        }

        internal void ActOnLeftLeaningMirror()
        {
            // Mirror \
            Direction = Direction switch
            {
                Direction.Left or Direction.Right => Direction.TurnRight(),
                Direction.Up or Direction.Down => Direction.TurnLeft()
            };
        }

        internal Beam? ActOnVerticalSplitter()
        {
            // Splitter |
            if (Direction is Direction.Up or Direction.Down)
                return null;

            Direction = Direction.Up;
            return new Beam(Direction.GetOppositeDirection(), Position);                           
        }

        internal Beam? ActOnHorizontalSplitter()
        {
            // Splitter -
            if (Direction is Direction.Left or Direction.Right)
                return null;

            Direction = Direction.Left;
            return new Beam(Direction.GetOppositeDirection(), Position);
        }

        public override string ToString()
            => $"{Direction} {Position}";

    }

    public static void Run()
    {
        var lines = File.ReadAllLines("Indata_2023\\16_test.txt");

        var grid = new Grid<Tile>(lines, x => x.Select(c => new Tile(c)));

        var startingBeams = GetStartingBeams(grid).ToList();

        List<(Beam, int)> results = new();

        foreach (var startingBeam in startingBeams)
        {
            var currentGrid = new Grid<Tile>(lines, x => x.Select(c => new Tile(c)));
            var beams = new List<Beam> { startingBeam };

            while (beams.Any())
            {
                MoveBeams(beams, currentGrid);
                currentGrid.Print(GetStringRepresentation);
            }

            results.Add((startingBeam, currentGrid.AllData.Count(x => x.Energized)));
        }

        Console.WriteLine(results.Max(x => x.Item2));
    }

    private static IEnumerable<Beam> GetStartingBeams(Grid<Tile> grid)
    {
        foreach(var row in grid.Rows)
        {
            yield return new Beam(Direction.Right, (row[0].Row, -1));
            yield return new Beam(Direction.Left, (row[0].Row, row.Last().Col + 1));
        }

        foreach (var col in grid.Columns)
        {
            yield return new Beam(Direction.Down, (-1, col[0].Col));
            yield return new Beam(Direction.Up, (col.Last().Row + 1, col[0].Col));
        }
    }

    static string GetStringRepresentation(Tile tile)
    {
        if (tile.Type is not '.')
            return tile.Type.ToString();

        return tile.VisitedBy.Count switch
            {
                0 => ".",
                1 => GetArrow(tile.VisitedBy.First()),
                > 1 => tile.VisitedBy.Count.ToString(),
            };                 
    }

    private static string GetArrow(Direction direction)
    {
        return direction switch
        {
            Direction.Up => "^",
            Direction.Down => "v",
            Direction.Left => "<",
            Direction.Right => ">"
        };
    }

    public static void MoveBeams(List<Beam> beams, Grid<Tile> grid)
    {
        var count = beams.Count;
        var beamsToRemove = new List<Beam>();

        for (int i = 0; i < count; i++) 
        {
            var beam = beams[i];
            var looped = beam.Move();

            // Must be removed before this, check with tile instead.
            //if (looped)
            //{
            //    beamsToRemove.Add(beam);
            //    continue;
            //}

            // Went off the grid?
            if (!grid.IsValidPosition(beam.Position))
            {
                beamsToRemove.Add(beam);
                continue;
            }

            var tile = grid.GetCell(beam.Position);

            if (tile.Data.VisitedBy.Contains(beam.Direction))
            {
                beamsToRemove.Add(beam);
                continue;
            }    

            tile.Data.VisitedBy.Add(beam.Direction);

            Beam? splitBeam = null;

            if (tile.Data.Type is '/')
                beam.ActOnRightLeaningMirror();
            else if (tile.Data.Type is '\\')
                beam.ActOnLeftLeaningMirror();
            else if (tile.Data.Type is '|')
                splitBeam = beam.ActOnVerticalSplitter();
            else if (tile.Data.Type is '-')
                splitBeam = beam.ActOnHorizontalSplitter();

            if (splitBeam != null)
                beams.Add(splitBeam);
        }

        beamsToRemove.ForEach(x => beams.Remove(x));
    }
}

