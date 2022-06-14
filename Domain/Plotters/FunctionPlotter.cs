using System;
using System.Collections.Generic;
using System.Linq;
using AngouriMath;
using AngouriMath.Core.Compilation.IntoLinq;
using Funcan.Domain.Models;
using PointSet = Funcan.Domain.Models.PointSet;

namespace Funcan.Domain.Plotters;

public class FunctionPlotter : IPlotter
{
    private DiscontinuitiesPlotter DiscontinuitiesPlotter { get; }

    public FunctionPlotter(DiscontinuitiesPlotter discontinuitiesPlotter) =>
        DiscontinuitiesPlotter = discontinuitiesPlotter;

    public PlotterInfo PlotterInfo => new("function", DrawType.Line);

    public IEnumerable<PointSet> GetPointSets(MathFunction function, FunctionRange functionRange)
    {
        IEnumerable<Point> discontinuities = DiscontinuitiesPlotter
            .GetPointSets(function, functionRange).First().Points.OrderBy(point => point.X);

        var points = new PointSet();
        var compiledFunc = function.Function.Compile<Func<double, double>>(new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
        {
            (typeof (double), "x")
        });

        for (var x = functionRange.From; x <= functionRange.To; x += Settings.Step)
        {
            var y = compiledFunc(x);
            var point = new Point(x, y);
            if (discontinuities.Any() && discontinuities.First().X < point.X)
            {
                yield return points;
                discontinuities = discontinuities.Skip(1);
                points = new PointSet();
                points.Add(point);
            }
            else
            {
                points.Add(new Point(x, y));
            }
        }

        yield return points;
    }
}