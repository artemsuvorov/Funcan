using System;
using System.Collections.Generic;
using AngouriMath;
using AngouriMath.Core.Compilation.IntoLinq;
using Funcan.Domain.Models;
using Funcan.Domain.Utils;

namespace Funcan.Domain.Plotters;

public class InflectionPointsPlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new PlotterInfo("inflection", DrawType.Dots);

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        var secondDerivative = function.Entity.Differentiate("x").Differentiate("x");
        var compileFunc = function.Compile();
        var secondDerivativeFunc = new MathFunction(secondDerivative.Stringize());
        var pointSet = new PointSet();
        foreach (var x in ExtendedMath.GetExtremaPoints(secondDerivativeFunc, functionRange))
            pointSet.Add(new Point(x, compileFunc(x)));
        yield return pointSet;
    }
}