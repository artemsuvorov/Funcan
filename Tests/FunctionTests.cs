using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class FunctionTests
{
    [Test]
    [TestCase("x^2", new double[] { 25, 16, 9, 4, 1, 0 })]
    [TestCase("x^3", new double[] { -125, 125, 64, -64, 27, -27, 8, -8, 1, -1, 0 })]
    [TestCase("sin(x)", new double[] { -1, 1, 0 })]
    [TestCase("cos(x)", new double[] { -1, 1, 0 })]
    public void FunctionPlotterTest(string function, double[] expectedYs)
    {
        var mathFunction = new MathFunction(function);
        var range = new FunctionRange(-6, 6);
        var functionPlotter = new FunctionPlotter(new DiscontinuitiesPlotter());
        var ys = functionPlotter
            .GetPointSets(mathFunction, range)
            .First().Points.Select(point => Math.Round(point.Y))
            .ToHashSet();
        foreach (var y in expectedYs)
            Assert.True(ys.Contains(y));
    }

    [Test]
    [TestCase("x", 1, -1, 1)]
    [TestCase("1/x", 2, -1, 1)]
    public void FunctionPlotterWithDiscontinuities(string function, int expectedPointSetCount, int from, int to)
    {
        var mathFunction = new MathFunction(function);
        var range = new FunctionRange(from, to);
        var functionPlotter = new FunctionPlotter(new DiscontinuitiesPlotter());
        var pointSets = functionPlotter.GetPointSets(mathFunction, range);
        Assert.AreEqual(pointSets.Count(), expectedPointSetCount);
    }
}