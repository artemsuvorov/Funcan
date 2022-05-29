using System;
using System.Collections.Generic;
using Funcan.Controllers;
using Funcan.Domain;
using Funcan.Infrastructure.Plotters;
using Range = Funcan.Domain.Range;

namespace Funcan.Application.Plotters;

public class DiscontinuitiesPlotter : IPlotter
{
    public PointSet GetPointSets(Func<double, double> function, Range range)
    {
        var breakPoints = new PointSet(new Style(new Color("Red"), Style.DisplayingType.Dots));
        for (var x = range.From; x <= range.To; x += 0.2d)
        {
            var y = DifferentialMath.GetLimit(function, x);
            if (double.IsInfinity(y) || double.IsNaN(y))
            {
                breakPoints.Add(new Point(x, function(x)));
            }
        }

        return breakPoints;
    }
}