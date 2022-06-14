using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using NUnit.Framework;

namespace Tests;

class FunctionHandler
{
    public static void CheckValidAsymptote(Func<Point, bool> checker, string function, FunctionRange range)
    {
        MathFunction.TryCreate(function, out var func);
        var discontinuitiesPlotter = new DiscontinuitiesPlotter();
        var asymptotePlotter = new AsymptotePlotter(
            new FunctionPlotter(discontinuitiesPlotter), discontinuitiesPlotter
        );
        var allAsymptotePoints = asymptotePlotter.GetPointSets(func, range);
        foreach (var pointSet in allAsymptotePoints)
        {
            foreach (var point in pointSet.Points)
            {
                Assert.True(checker(point));
            }
        }
    }
}

[TestFixture]
public class AsymptoteTests
{
    [Test]
    public void AsymptoteTestEasy1()
    {
        var function = "1/x";
        double ObliqueAsymptote(double _) => 0;
        var horizontalAsymptote = 0;

        bool AsymptoteChecker(Point point) => Math.Abs(point.Y - ObliqueAsymptote(point.X)) < 0.001 ||
                                              Math.Abs(point.X - horizontalAsymptote) < 0.001;

        FunctionHandler.CheckValidAsymptote(AsymptoteChecker, function, new FunctionRange(-10, 10));
    }

    [Test]
    public void AsymptoteTestEasy2()
    {
        var function = "1/(x^2)";
        double ObliqueAsymptote(double _) => 0;
        var horizontalAsymptote = 0;

        bool AsymptoteChecker(Point point) => Math.Abs(point.Y - ObliqueAsymptote(point.X)) < 0.001 ||
                                              Math.Abs(point.X - horizontalAsymptote) < 0.001;

        FunctionHandler.CheckValidAsymptote(AsymptoteChecker, function, new FunctionRange(-10, 10));
    }
}