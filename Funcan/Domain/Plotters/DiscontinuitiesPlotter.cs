using System.Collections.Generic;
using AngouriMath;
using Funcan.Domain.Models;
using Funcan.Domain.Utils;

namespace Funcan.Domain.Plotters;

public class DiscontinuitiesPlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new("discontinuities", DrawType.Dots);

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        var zeros = new PointSet();
        foreach (var entity in function.Entity.Nodes)
        {
            if (entity is not Entity.Divf divf) continue;
            MathFunction.TryCreate(divf.NodeSecondChild.Stringize(), out var denominator);
            zeros.AddPointSet(ExtendedMath.GetZerosFunctionInRange(denominator, functionRange));
        }

        yield return zeros;
    }
}