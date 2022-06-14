using Funcan.Domain;
using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class DiscontinuitiesTests
{
    private DiscontinuitiesPlotter plotter = new();
    public void MainTest(MathFunction function, FunctionRange range, params Point[] expectedPoints)
    {
        var collction = plotter.GetPointSets(function, range);
        Assert.AreEqual(1, collction.Count());
        var actualPoints = collction.First().Points.OrderBy(point => point.X);
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
        MainTest(new MathFunction("1/x"), range, new Point(0, 0));
    }

    [Test]
    public void ComplicatedFunctionTest()
    {
        var range = new FunctionRange(-10, 10);
        MainTest(new MathFunction("1 / (x^2 -4)"), range, new Point(-2, 0), new Point(2, 0));
        MainTest(new MathFunction("1/ (x^3 - 5x^2 + 6x)"), range, 
            new Point(0, 0), new Point(2, 0), new Point(3, 0));
        MainTest(new MathFunction("(x^2 - 7x + 6) / (x^3 + 8)"), range, new Point(-2, 0));
    }
}