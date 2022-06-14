using System;
using System.Collections.Generic;
using AngouriMath;
using AngouriMath.Core.Compilation.IntoLinq;
using Funcan.Domain.Models;

namespace Funcan.Domain.Plotters;

public class InflectionPointsPlotter : IPlotter
{
    public PlotterInfo PlotterInfo => new PlotterInfo("inflection", DrawType.Dots);

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        var secondDerivative = function.Function.Differentiate("x").Differentiate("x");
        yield return ExtendedMath.
            GetCriticalPoints(function, functionRange, new MathFunction(secondDerivative.Stringize()));
    }
}