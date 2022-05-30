using System;
using Funcan.Controllers;
using Funcan.Domain;
using Funcan.Infrastructure.Plotters;
using Range = Funcan.Domain.Range;

namespace Funcan.Application.Plotters;

public class InflectionPointsPlotter: IPlotter
{
    public PointSet GetPointSets(Func<double, double> function, Range range)
    {
        var pointSet = new PointSet(new Style(new Color("Cyan"), Style.DisplayingType.Dots));
        var eps = 0.1;
        var step = 0.0001;
        var previous = DifferentialMath.GetDerivative(function, range.From - step);
        var current = DifferentialMath.GetDerivative(function, range.From);
        for (var x = range.From; x < range.To; x += step)
        {
            var next = DifferentialMath.GetDerivative(function, x + step);
            if (current == 0) 
                Console.WriteLine(next);
            if (Math.Abs(current) < eps && previous * next > 0)
            {
                pointSet.Add(new Point(x, function(x)));
            }

            previous = current;
            current = next;
        }

        return pointSet;
    }
}