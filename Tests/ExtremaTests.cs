using Funcan.Domain;
using AngouriMath;
using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class ExtremaTests
{
    ExtremaPlotter extremaPlotter = new();
    [Test]
    public void TestSqr()
    {
        var mathFunction = new MathFunction("x^2");
        var range = new FunctionRange(-5, 5);
        var collection = extremaPlotter.GetPointSets(mathFunction, range);
        Assert.AreEqual(1, collection.Count());
        var set = collection.First();
        Assert.AreEqual(1, set.Points.Count);
        Assert.Contains(new Point(0, 0), set.Points.ToList());
    }
    
    public void Polynomial(string expression, int count, params Point[] points)
    {
        var mathFunction = new MathFunction(expression);
        var range = new FunctionRange(-5, 5);
        var collection = extremaPlotter.GetPointSets(mathFunction, range);
        Assert.AreEqual(1, collection.Count());
        var set = collection.First();
        Assert.AreEqual(count, set.Points.Count);
        var tuples = points.Zip(set.Points);
        foreach (var tuple in tuples)
        {
            Assert.AreEqual(tuple.First.X, tuple.Second.X, 0.01);
            Assert.AreEqual(tuple.First.Y, tuple.Second.Y, 0.01);
        }
    }
    [Test]
    public void TestSimplePolynomial()
    {
        Polynomial("x^2 - 5x + 4", 1, new Point(2.5, -2.25));
        Polynomial("x^3 + x^2", 2, new Point(-0.667, 0.148), new Point(0, 0));
    }
}