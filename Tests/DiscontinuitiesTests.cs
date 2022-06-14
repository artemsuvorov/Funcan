using Funcan.Domain;
using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class DiscontinuitiesTests
{
    [Test]
    public static void ValidDiscontinuities([Values("1/x", "1/(x^2)")] string function)
    {
        var func = new MathFunction(function);
        var discontinuitiesPlotter = new DiscontinuitiesPlotter();
        var discontinuities = discontinuitiesPlotter.GetPointSets(func, new FunctionRange(-10, 10));
        var ans = discontinuities.SelectMany(pointSet => pointSet.Points).ToList();
        var expected = new List<Point> {new (0, 0)};
        Assert.AreEqual(expected, ans);
    }
    
    [Test]
    public static void NoDiscontinuities([Values("x^2", "x", "e^x")] string function)
    {
        var func = new MathFunction(function);
        var discontinuitiesPlotter = new DiscontinuitiesPlotter();
        var discontinuities = discontinuitiesPlotter.GetPointSets(func, new FunctionRange(-10, 10));
        var ans = discontinuities.SelectMany(pointSet => pointSet.Points).ToList();
        var expected = new List<Point> ();
        Assert.AreEqual(expected, ans);
    }
}