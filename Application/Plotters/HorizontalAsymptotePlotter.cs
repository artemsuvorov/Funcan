using System;
using Funcan.Controllers;
using Funcan.Domain;
using Funcan.Infrastructure.Plotters;
using Range = Funcan.Domain.Range;

namespace Funcan.Application.Plotters;

public class HorizontalAsymptotePlotter : IPlotter
{
    public PointSet GetPointSets(Func<double, double> function, Range range)
    {
        var points = new PointSet(new Style(new Color("Green"), Style.DisplayingType.Line));
        var step = 0.00001;
        for (var x = range.From; x < range.To; x += step)
        {
            var leftLimit = DifferentialMath.GetLeftLimit(function, x);
            var rightLimit = DifferentialMath.GetRightLimit(function, x);
            if (double.IsInfinity(leftLimit) && double.IsInfinity(rightLimit))
            {
                for (var y = range.From; y < range.To; y += 10)
                    points.Add(new Point(x, y));
            }
        }

        return points;
    }
}