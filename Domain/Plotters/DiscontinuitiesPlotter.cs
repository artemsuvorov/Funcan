using System;
using System.Collections.Generic;
using Funcan.Domain.Models;

namespace Funcan.Domain.Plotters;

public class DiscontinuitiesPlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new PlotterInfo("discontinuities", DrawType.Dots, new Color("red"));

    public IEnumerable<PointSet> GetPointSets(Func<double, double> function, FunctionRange functionRange)
    {
        var breakPoints = new PointSet();
        for (var x = functionRange.From; x <= functionRange.To; x += Settings.Step)
        {
            var y = DifferentialMath.GetLimit(function, x);
            if (double.IsInfinity(y) || double.IsNaN(y))
            {
                breakPoints.Add(new Point(x, function(x)));
            }
        }

        yield return breakPoints;
    }
}