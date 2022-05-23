using System;
using System.Collections.Generic;
using Funcan.Domain;
using Range = Funcan.Domain.Range;

namespace Funcan.Infrastructure.Plotters;

public interface IPlotter
{
    IEnumerable<PointSet> GetPointSets(Func<double, double> function, Range range);
}