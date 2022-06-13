using System;
using System.Collections.Generic;
using Funcan.Domain.Models;

namespace Funcan.Domain.Plotters;

public class AsymptotePlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new PlotterInfo("asymptote", DrawType.Line);
    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        yield return new PointSet();
        // var points = new PointSet();
        // for (var x = functionRange.From; x < functionRange.To; x += Settings.Step)
        // {
        //     var leftLimit = ExtendedMath.GetLeftLimit(function, x);
        //     var rightLimit = ExtendedMath.GetRightLimit(function, x);
        //     if (double.IsInfinity(leftLimit) && double.IsInfinity(rightLimit))
        //     {
        //         for (var y = functionRange.From; y < functionRange.To; y += 10)
        //             points.Add(new Point(x, y));
        //     }
        // }
        //
        // yield return points;
    }
}