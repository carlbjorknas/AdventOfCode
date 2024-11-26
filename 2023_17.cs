using AdventOfCode;

// https://www.youtube.com/watch?v=2pDSooPLLkI

internal class _2023_17
{
    record Visit((int Row, int Col) Position, Direction Direction, int AccHeatLoss, int NumberConsecutives);

    public static void Run()
    {
        var lines = File.ReadAllLines("Indata_2023\\17.txt");

        var grid = new Grid<(int HeatLoss, int? AccHeatLoss)>(lines, line => line.Select(c => (int.Parse(c.ToString()), (int?)null)));
        var seen = new HashSet<((int, int), Direction, int)>();

        var firstCell = grid.AllCells.First();
        var lastCell = grid.AllCells.Last();

        var pq = new PriorityQueue<Visit, int>();
        var start = new Visit((0, 0), Direction.Right, 0, 0);
        pq.Enqueue(start, start.AccHeatLoss);

        while (pq.Count > 0)
        {
            var current = pq.Dequeue();

            //Console.WriteLine(current);

            if (!seen.Add((current.Position, current.Direction, current.NumberConsecutives)))
            {
                //Console.WriteLine("Already seen!");
                continue;
            }

            var cell = grid.GetCell(current.Position);

            var accHeatloss = current.AccHeatLoss + (cell == firstCell ? 0 : cell.Data.HeatLoss);

            if (cell == lastCell)
            {
                Console.WriteLine(accHeatloss);
                break;
            }

            // To be able to print grid
            if (cell.Data.AccHeatLoss == null || cell.Data.AccHeatLoss > accHeatloss)
                cell.Data = (cell.Data.HeatLoss, accHeatloss);

            foreach (var dir in Enum.GetValues<Direction>())
            {
                if (dir == current.Direction.GetOppositeDirection())
                    continue;

                if (dir == current.Direction && cell != firstCell)
                {
                    if (current.NumberConsecutives != 10)
                    {
                        var nextPos = current.Position.Move(dir);
                        if (!grid.IsValidPosition(nextPos))
                            continue;
                        pq.Enqueue(new Visit(nextPos, dir, accHeatloss, current.NumberConsecutives + 1), accHeatloss);
                    }
                }
                else
                {
                    var nextPos = current.Position.Move(dir, 4);
                    if (!grid.IsValidPosition(nextPos))
                        continue;

                    var intermediatePos = current.Position.Move(dir);
                    var dirHeatLoss = accHeatloss + grid.GetCell(intermediatePos).Data.HeatLoss;
                    intermediatePos = intermediatePos.Move(dir);
                    dirHeatLoss += grid.GetCell(intermediatePos).Data.HeatLoss;
                    intermediatePos = intermediatePos.Move(dir);
                    dirHeatLoss += grid.GetCell(intermediatePos).Data.HeatLoss;
                    
                    pq.Enqueue(new Visit(nextPos, dir, dirHeatLoss, 4), dirHeatLoss);
                }
            }

            //grid.Print(x => $"{x.AccHeatLoss?.ToString() ?? ".",4}");

            //Console.WriteLine(string.Join(", ", pq.UnorderedItems.Select(x => x.Priority).OrderBy(x => x)));
            //Console.WriteLine();
        }

        //Console.WriteLine(grid.Rows.Last().Last().Data.AccHeatLoss);
    }
}

// Part 1
//record Visit((int Row, int Col) Position, Direction Direction, int AccHeatLoss, int NumberConsecutives);

//public static void Run()
//{
//    var lines = File.ReadAllLines("Indata_2023\\17.txt");

//    var grid = new Grid<(int HeatLoss, int? AccHeatLoss)>(lines, line => line.Select(c => (int.Parse(c.ToString()), (int?)null)));
//    var seen = new HashSet<((int, int), Direction, int)>();

//    var firstCell = grid.AllCells.First();
//    var lastCell = grid.AllCells.Last();

//    var pq = new PriorityQueue<Visit, int>();
//    var start = new Visit((0, 0), Direction.Right, 0, 0);
//    pq.Enqueue(start, start.AccHeatLoss);

//    while(pq.Count > 0)
//    {
//        var current = pq.Dequeue();

//        //Console.WriteLine(current);

//        if (!seen.Add((current.Position, current.Direction, current.NumberConsecutives)))
//        {
//            //Console.WriteLine("Already seen!");
//            continue;
//        }            

//        var cell = grid.GetCell(current.Position);

//        var accHeatloss = current.AccHeatLoss + (cell == firstCell ? 0 : cell.Data.HeatLoss);

//        if (cell == lastCell)
//        {
//            Console.WriteLine(accHeatloss);
//            break;
//        }

//        // To be able to print grid
//        if (cell.Data.AccHeatLoss == null || cell.Data.AccHeatLoss > accHeatloss)
//            cell.Data = (cell.Data.HeatLoss, accHeatloss);

//        foreach (var dir in Enum.GetValues<Direction>())
//        {
//            if (dir == current.Direction.GetOppositeDirection())
//                continue;

//            var nextPos = current.Position.Move(dir);

//            if (!grid.IsValidPosition(nextPos))
//                continue;

//            if (dir == current.Direction)
//            {
//                if (current.NumberConsecutives != 3)
//                {
//                    pq.Enqueue(new Visit(nextPos, dir, accHeatloss, current.NumberConsecutives + 1), accHeatloss);
//                }
//            }
//            else
//            {
//                pq.Enqueue(new Visit(nextPos, dir, accHeatloss, 1), accHeatloss);
//            }                
//        }            

//        //grid.Print(x => $"{x.AccHeatLoss?.ToString() ?? ".",3}");
//    }

//    //Console.WriteLine(grid.Rows.Last().Last().Data.AccHeatLoss);
//}