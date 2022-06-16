using Funcan.Domain;
using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class InflectionTests {
    private readonly InflectionPointsPlotter inflectionPointsPlotter = new();

    [Test]
    public void TestCube(){
        MathFunction.TryCreate("x^3", out var mathFunction);
        var range = new FunctionRange(-5, 5);
        var collection = inflectionPointsPlotter.GetPointSets(mathFunction, range).ToList();
        Assert.AreEqual(1, collection.Count);
        var set = collection.First();
        Assert.AreEqual(1, set.Points.Count);
        Assert.Contains(new Point(0, 0), set.Points.ToList());
    }

    public void MainTest(string function, FunctionRange range, params Point[] points){
        MathFunction.TryCreate(function, out var mathFunction);
        var collection = inflectionPointsPlotter.GetPointSets(mathFunction, range).ToList();
        Assert.True(collection.Count != 0);
        var resultPoints = collection.First().Points.OrderBy(point => point.X);
        foreach (var pair in points.Zip(resultPoints)){
            Assert.AreEqual(pair.First.X, pair.Second.X, 0.1);
            Assert.AreEqual(pair.First.Y, pair.Second.Y, 0.1);
        }
    }

    [Test]
    public void TestTrigonometry(){
        var pi = Math.PI;
        var range = new FunctionRange(-5, 7);
        MainTest("sin(x)", range, new Point(pi * -1, 0), new Point(0, 0),
            new Point(pi, 0), new Point(pi * 2, 0));
        MainTest("cos(x)", range, new Point(pi * -1.5, 0), new Point(pi * -0.5, 0),
            new Point(pi * 0.5, 0), new Point(pi * 1.5, 0));
    }

    [Test]
    public void TestPolynomial(){
        var range = new FunctionRange(-10, 10);
        MainTest("6x^2 -x^3", range, new Point(2, 16));
        MainTest("2x^4 - x^5 + 4", range, new Point(1.2, 5.659));
    }
}