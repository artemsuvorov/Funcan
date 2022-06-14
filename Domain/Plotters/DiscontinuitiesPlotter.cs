using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AngouriMath;
using Funcan.Domain.Models;
using static System.Text.RegularExpressions.Regex;

namespace Funcan.Domain.Plotters;

public class DiscontinuitiesPlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new PlotterInfo("discontinuities", DrawType.Dots);

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        var zeros = new PointSet();
        foreach (var entity in function.Function.Nodes)
        {
            if (entity is Entity.Divf divf)
                zeros.AddPointSet(ExtendedMath.GetZerosFunctionInRange(
                    new MathFunction(divf.NodeSecondChild.Stringize()), functionRange));
        }

        yield return zeros;
    }
}