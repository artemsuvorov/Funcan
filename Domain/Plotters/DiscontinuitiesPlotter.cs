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
        var compiledFunc = function.Function.Compile("x");
        var zeros = new PointSet();
        foreach (var entity in function.Function.Nodes)
        {
            if (entity is Entity.Divf divf)
                zeros.AddPointSet(
                    ExtendedMath.GetZerosFunctionInRange(new MathFunction(divf.NodeSecondChild.Stringize()),
                        functionRange));
        }

        yield return zeros;
    }
}