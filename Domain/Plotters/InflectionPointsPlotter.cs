using System;
using System.Collections.Generic;
using Funcan.Domain.Models;

namespace Funcan.Domain.Plotters;

public class InflectionPointsPlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new PlotterInfo("inflection", DrawType.Dots);

    public IEnumerable<PointSet> GetPointSets(Func<double, double> function, FunctionRange functionRange)
    {
        var pointSet = new PointSet();
        var eps = 0.1;
        var previous = DifferentialMath.GetDerivative(function, functionRange.From - Settings.Step);
        var current = DifferentialMath.GetDerivative(function, functionRange.From);
        for (var x = functionRange.From; x < functionRange.To; x += Settings.Step)
        {
            var next = DifferentialMath.GetDerivative(function, x + Settings.Step);
            if (current == 0)
                Console.WriteLine(next);
            if (Math.Abs(current) < eps && previous * next > 0)
            {
                pointSet.Add(new Point(x, function(x)));
            }

            previous = current;
            current = next;
        }

        yield return pointSet;
    }
}