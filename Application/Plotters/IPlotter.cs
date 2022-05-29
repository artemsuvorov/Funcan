using System;
using Funcan.Domain;
using Range = Funcan.Domain.Range;

namespace Funcan.Infrastructure.Plotters;

public interface IPlotter
{
    PointSet GetPointSets(Func<double, double> function, Range range);
}