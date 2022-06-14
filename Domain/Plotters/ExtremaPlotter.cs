using System;
using System.Collections.Generic;
using AngouriMath;
using AngouriMath.Core.Compilation.IntoLinq;
using Funcan.Domain.Models;

namespace Funcan.Domain.Plotters;

public class ExtremaPlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new PlotterInfo("extrema", DrawType.Dots);

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        var derivative = function.Function.Differentiate("x");
        yield return ExtendedMath
            .GetCriticalPoints(function, functionRange, new MathFunction(derivative.Stringize()));
    }
}