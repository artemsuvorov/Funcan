using Funcan.Domain;
using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class AsymptoteTests
{
    [Test]
    public void AsymptoteValidTests(
        [Values("1/x")] string function)
    {
        double ObliqueAsymptote() => 0;
        const int horizontalAsymptote = 0;

        bool AsymptoteChecker(Point point) => Math.Abs(point.Y - ObliqueAsymptote()) < 0.001 ||
                                              Math.Abs(point.X - horizontalAsymptote) < 0.001;

        var func = new MathFunction(function);
        var discontinuitiesPlotter = new DiscontinuitiesPlotter();
        var asymptotePlotter = new AsymptotePlotter(
            new FunctionPlotter(discontinuitiesPlotter), new DiscontinuitiesPlotter()
        );
        var allAsymptotePoints = asymptotePlotter.GetPointSets(func, new FunctionRange(-10, 10));
        foreach (var pointSet in allAsymptotePoints)
        {
            foreach (var point in pointSet.Points)
            {
                Assert.True(AsymptoteChecker(point));
            }
        }
    }
}