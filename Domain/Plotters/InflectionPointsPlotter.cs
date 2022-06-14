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
        var compiledFunc = function.Function.Compile<Func<double, double>>(new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
        {
            (typeof (double), "x")
        });
        var derivative = function.Function.Differentiate("x").Differentiate("x");
        var compiledDerivative = derivative.Compile<Func<double, double>>(new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
        {
            (typeof (double), "x")
        });
        var zeros = ExtendedMath.GetZerosFunctionInRange(new MathFunction(derivative.Stringize()), functionRange);
        var delta = 0.1;
        var extremas = new PointSet();
        foreach (var point in zeros.Points)
        {
            var n1 = compiledDerivative(point.X - delta);
            var n2 = compiledDerivative(point.X + delta);
            if (n1 * n2 < 0) extremas.Add(point with {Y = compiledFunc(point.X)});
        }
        yield return extremas;
    }
}