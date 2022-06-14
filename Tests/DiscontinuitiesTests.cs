using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class DiscontinuitiesTests
{
    private readonly DiscontinuitiesPlotter plotter = new();

    private void MainTest(MathFunction function, FunctionRange range, params Point[] expectedPoints)
    {
        var collection = plotter.GetPointSets(function, range).ToList();
        Assert.AreEqual(1, collection.Count);
        var actualPoints = collection.First().Points.OrderBy(point => point.X);
        foreach (var pair in expectedPoints.Zip(actualPoints))
        {
            var expected = pair.First;
            var actual = pair.Second;
            Assert.AreEqual(expected.X, actual.X, 0.1);
            Assert.AreEqual(expected.Y, actual.Y, 0.1);
        }
    }

    [Test]
    public void SimpleFunctionTest()
    {
        var range = new FunctionRange(-5, 5);
        MathFunction.TryCreate("1/x", out var function);
        MainTest(function, range, new Point(0, 0));
    }

    [Test]
    public void ComplicatedFunctionTest()
    {
        var range = new FunctionRange(-10, 10);
        MathFunction.TryCreate("1 / (x^2 -4)", out var func1);
        MainTest(func1, range, new Point(-2, 0), new Point(2, 0));
        MathFunction.TryCreate("1/ (x^3 - 5x^2 + 6x)", out var func2);
        MainTest(func2, range, new Point(0, 0), new Point(2, 0), new Point(3, 0));
        MathFunction.TryCreate("(x^2 - 7x + 6) / (x^3 + 8)", out var func3);
        MainTest(func3, range, new Point(-2, 0));
    }
}