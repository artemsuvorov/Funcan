using System.Collections.Generic;
using Funcan.Domain.Models;
using Funcan.Domain.Utils;

namespace Funcan.Domain.Plotters;

public class ExtremaPlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new PlotterInfo("extrema", DrawType.Dots);

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        var derivative = function.Entity.Differentiate("x");
        var compileFunc = function.Compile();
        var pointSet = new PointSet();
        var derivativeFunc = new MathFunction(derivative.Stringize());
        foreach (var x in ExtendedMath.GetExtremaPoints(derivativeFunc, functionRange))
            pointSet.Add(new Point(x, compileFunc(x)));
        yield return pointSet;
    }
}