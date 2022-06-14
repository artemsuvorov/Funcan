using System;
using System.Collections.Generic;
using System.Linq;
using Funcan.Domain.Models;

namespace Funcan.Domain.Plotters;

public class AsymptotePlotter : IPlotter
{
    private FunctionPlotter FunctionPlotter { get; }
    private DiscontinuitiesPlotter DiscontinuitiesPlotter { get; }

    public AsymptotePlotter(FunctionPlotter functionPlotter, DiscontinuitiesPlotter discontinuitiesPlotter)
    {
        FunctionPlotter = functionPlotter;
        DiscontinuitiesPlotter = discontinuitiesPlotter;
    }

    public PlotterInfo PlotterInfo => new PlotterInfo("asymptote", DrawType.Line);

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        var discontinuities = DiscontinuitiesPlotter.GetPointSets(function, functionRange);
        var list = new List<PointSet>();
        PointSet vertical = null;
        PointSet horizontal = null;
        try
        {
            vertical = GetVerticalAsymptotePoints(discontinuities, function, functionRange);
            horizontal = GetObliqueAsymptotesPoints(function, functionRange);
        }
        catch (ArgumentException)
        {
        }

        if (vertical != null) yield return vertical;
        if (horizontal != null) yield return vertical;
    }

    private PointSet GetObliqueAsymptotesPoints(MathFunction function,
        FunctionRange range)
    {
        // y = kx + b
        var points = new PointSet();
        var defaultLinearFunction = new MathFunction("x");
        var kFunction = function / defaultLinearFunction;
        var firstKLimit = ExtendedMath.GetLeftLimit(kFunction, double.PositiveInfinity);
        var secondKLimit = ExtendedMath.GetRightLimit(kFunction, double.NegativeInfinity);
        if (Math.Abs(firstKLimit - secondKLimit) > 0.001 || double.IsInfinity(firstKLimit) ||
            double.IsInfinity(secondKLimit)) return points;
        var bFunction = function - (firstKLimit * defaultLinearFunction);
        var firstBLimit = ExtendedMath.GetLeftLimit(bFunction, double.PositiveInfinity);
        var secondBLimit = ExtendedMath.GetRightLimit(bFunction, double.NegativeInfinity);
        if (Math.Abs(firstBLimit - secondBLimit) > 0.001) return points;
        var asymptote = firstKLimit * defaultLinearFunction + secondBLimit;
        return FunctionPlotter.GetPointSets(asymptote, range).FirstOrDefault();
    }

    private PointSet GetVerticalAsymptotePoints(IEnumerable<PointSet> discontinuities, MathFunction function,
        FunctionRange range)
    {
        var points = new PointSet();
        foreach (var pointSet in discontinuities)
        {
            foreach (var point in pointSet.Points)
            {
                var leftLimit = ExtendedMath.GetLeftLimit(function, point.X);
                var rightLimit = ExtendedMath.GetRightLimit(function, point.X);
                if (!double.IsInfinity(leftLimit) && !double.IsInfinity(rightLimit)) continue;
                for (var y = range.From; y < range.To; y++)
                {
                    points.Add(point with {Y = y});
                }
            }
        }

        return points;
    }
}