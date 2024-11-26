namespace AdventOfCode;

public enum Direction { Left, Right, Up, Down }

public static class DirectionExtension
{
    public static Direction GetOppositeDirection(this Direction direction)
        =>
            direction switch
            {
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up
            };

    public static IEnumerable<Direction> GetAllOtherDirections(this Direction direction)
    {
        direction = direction.TurnLeft();
        yield return direction;
        direction = direction.TurnLeft();
        yield return direction;
        yield return direction.TurnLeft();
    }
            

    public static Direction TurnRight(this Direction direction)
        =>
            direction switch
            {
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left
            };

    public static Direction TurnLeft(this Direction direction)
        =>
            direction switch
            {
                Direction.Left => Direction.Down,
                Direction.Right => Direction.Up,
                Direction.Up => Direction.Left,
                Direction.Down => Direction.Right
            };

    public static string ToArrow(this Direction direction)
        =>
            direction switch
            {
                Direction.Left => "<",
                Direction.Right => ">",
                Direction.Up => "^",
                Direction.Down => "v"
            };
}

public static class PositionExtensions
{
    public static (int Row, int Col) Move(this (int Row, int Col) pos, Direction dir, int steps = 1)
        =>
            dir switch
            {
                Direction.Left => (pos.Row, pos.Col - steps),
                Direction.Right => (pos.Row, pos.Col + steps),
                Direction.Up => (pos.Row - steps, pos.Col),
                Direction.Down => (pos.Row + steps, pos.Col),
            };
}
