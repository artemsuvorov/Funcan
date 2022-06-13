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
        var compiledFunc = function.Function.Compile<Func<double, double>>(new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
        {
            (typeof (double), "x")
        });
        var derivative = function.Function.Differentiate("x");
        var zeros = ExtendedMath.GetZerosFunctionInRange(new MathFunction(derivative.Stringize()), functionRange);
        var delta = 0.001;
        var extremas = new PointSet();
        foreach (var point in zeros.Points)
        {
            var n1 = compiledFunc(point.X - delta);
            var n2 = compiledFunc(point.X + delta);
            if (n1 * n2 < 0) extremas.Add(point);
        }
        yield return extremas;
        // var pointSet = new PointSet();
        // var eps = 0.1;
        // var previous = ExtendedMath.GetDerivative(function, functionRange.From - Settings.Step);
        // var current = ExtendedMath.GetDerivative(function, functionRange.From);
        // for (var x = functionRange.From; x < functionRange.To; x += Settings.Step)
        // {
        //     var next = ExtendedMath.GetDerivative(function, x + Settings.Step);
        //     if (Math.Abs(current) < eps && previous * next < 0)
        //     {
        //         pointSet.Add(new Point(x, function(x)));
        //     }
        //
        //     previous = current;
        //     current = next;
        // }
        //
        // yield return pointSet;
    }
}