using System.Collections.Generic;
using System.Linq;
using Funcan.Domain.Models;

namespace Funcan.Domain.Utils;

public static class PointsetWrapper
{
    public static Plot Wrap(IEnumerable<PointSet> pointSets, PlotterInfo plotterInfo) =>
        new(pointSets.ToList(), plotterInfo.DrawType, plotterInfo.Color);
}