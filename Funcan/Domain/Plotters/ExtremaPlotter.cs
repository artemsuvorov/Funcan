using System;
using System.Collections.Generic;
using AngouriMath;
using AngouriMath.Core.Compilation.IntoLinq;
using Funcan.Domain.Models;
using Funcan.Domain.Utils;

namespace Funcan.Domain.Plotters;

public class ExtremaPlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new PlotterInfo("extrema", DrawType.Dots);

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        var derivative = function.Entity.Differentiate("x");
        MathFunction.TryCreate(derivative.Stringize(), out var derivativeFunc);
        yield return ExtendedMath
            .GetCriticalPoints(function, functionRange, derivativeFunc);
    }
}