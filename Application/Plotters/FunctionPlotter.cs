using System;
using System.Collections.Generic;
using Funcan.Domain;
using Funcan.Infrastructure.Plotters;
using Point = Funcan.Controllers.Point;
using PointSet = Funcan.Domain.PointSet;
using Range = Funcan.Domain.Range;

namespace Funcan.Application.Plotters;

public class FunctionPlotter : IPlotter
{
    public IEnumerable<PointSet> GetPointSets(Func<double, double> function, Range range)
    {
        var points = new PointSet(new Style(new Color("Blue"), Style.DisplayingType.Line));
        for (var x = range.From; x <= range.To; x += 0.2d)
        {
            var y = function(x);
            points.Add(new Point(x, y));
        }

        return new List<PointSet> { points };
    }
}