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
        MathFunction.TryCreate(secondDerivative.Stringize(), out var secondDerivativeFunc);
        yield return ExtendedMath.
            GetCriticalPoints(function, functionRange, secondDerivativeFunc);
    }
}