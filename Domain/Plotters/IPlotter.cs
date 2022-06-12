using System;
using System.Collections.Generic;
using Funcan.Domain.Models;

namespace Funcan.Domain.Plotters;

public interface IPlotter
{
    PlotterInfo PlotterInfo { get; }
    IEnumerable<PointSet> GetPointSets(Func<double, double> function, FunctionRange functionRange);
}