using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Tests;

[TestClass]
public class GridTests
{
    [TestMethod]
    public void Inits_correctly_from_string_rows()
    {
        var stringRows = new string[]
        {
            "a b c",
            "d e f",
            "g h i"
        };

        var grid = new Grid<string>(stringRows, row => row.Split(" "));

        grid.NumberRows.Should().Be(3);
        grid.NumberCols.Should().Be(3);

        grid.AllCells.Should().HaveCount(9);
        grid.AllCells[0].Row.Should().Be(0);
        grid.AllCells[0].Col.Should().Be(0);
        grid.AllCells[0].Data.Should().Be("a");

        grid.AllCells[1].Row.Should().Be(0);
        grid.AllCells[1].Col.Should().Be(1);
        grid.AllCells[1].Data.Should().Be("b");

        grid.AllCells[8].Row.Should().Be(2);
        grid.AllCells[8].Col.Should().Be(2);
        grid.AllCells[8].Data.Should().Be("i");

        grid.AllData.Should().HaveCount(9);
        grid.AllData.Should().ContainInConsecutiveOrder("a", "b", "c", "d", "e", "f", "g", "h", "i");

        grid.RowsReversed[2].AsData().Should().ContainInConsecutiveOrder("i", "h", "g");

        grid.Columns[1].AsData().Should().ContainInConsecutiveOrder("b", "e", "h");
        grid.ColumnsReversed[1].AsData().Should().ContainInConsecutiveOrder("h", "e", "b");
    }
}
