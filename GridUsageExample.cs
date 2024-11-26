using MoreLinq;

namespace AdventOfCode;

internal class GridUsageExample
{
    public static void Run()
    {
        // Day 8 2022
        var lines = File.ReadAllLines("Indata_2022\\Day8.txt");

        var grid = new Grid<Tree>(lines, line => line.Select(c => new Tree(int.Parse(c.ToString()))).ToList());

        void MarkVisibleTrees(IEnumerable<Cell<Tree>> treeLine) 
        {
            var lastHeight = -1;
            foreach (var tree in treeLine.AsData())
            {
                if (tree.Height > lastHeight)
                {
                    lastHeight = tree.Height;
                    tree.Visible = true;
                }
            }
        };

        grid.Rows.ForEach(MarkVisibleTrees);
        grid.RowsReversed.ForEach(MarkVisibleTrees);
        grid.Columns.ForEach(MarkVisibleTrees);
        grid.ColumnsReversed.ForEach(MarkVisibleTrees);

        var count = grid.AllData.Count(tree => tree.Visible);
        //560 too low
        Console.WriteLine(count);

        List<int> scores = new();

        grid.AllCells.ForEach(CalculateScore);

        void CalculateScore(Cell<Tree> cell)
        {
            var tree = cell.Data;
            var l = grid.GetCellsRightOf(cell).AsData().TakeUntil(x => x.Height >= tree.Height).Count();
            var r = grid.GetCellsLeftOf(cell).AsData().Reverse().TakeUntil(x => x.Height >= tree.Height).Count();
            var t = grid.GetCellsBelow(cell).AsData().TakeUntil(x => x.Height >= tree.Height).Count();
            var b = grid.GetCellsAbove(cell).AsData().Reverse().TakeUntil(x => x.Height >= tree.Height).Count();

            scores.Add(l * r * t * b);
        }
    }

    class Tree
    {
        public Tree(int height)
        {
            Height = height;
        }

        public int Height { get; }
        public bool Visible { get; set; }

        public override string ToString()
        {
            return $"({Height}, {Visible})";
        }
    }
}
