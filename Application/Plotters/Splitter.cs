using System;
using System.Collections.Generic;
using Funcan.Controllers;
using Funcan.Domain;

namespace Funcan.Application.Plotters;

public class Splitter
{
    public IEnumerable<PointSet> Split(PointSet points, Func<double, double> func)
    {
        var buffer = new PointSet(points.Style);
        foreach (var point in points.Points)
        {
            var y = DifferentialMath.GetLimit(func, point.X);
            if (double.IsInfinity(y) || double.IsNaN(y))
            {
                yield return buffer = new PointSet(new Style(new Color("Blue"), Style.DisplayingType.Line));
            }
            buffer.Add(point);
        }

        yield return buffer;
    }
}