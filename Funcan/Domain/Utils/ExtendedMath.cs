using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AngouriMath;
using AngouriMath.Core;
using AngouriMath.Core.Compilation.IntoLinq;
using AngouriMath.Core.Exceptions;
using AngouriMath.Extensions;
using Funcan.Domain.Exceptions;
using Funcan.Domain.Models;

namespace Funcan.Domain.Utils;

public static class ExtendedMath
{
    private const double Delta = 0.01;

    public static MathFunction GetDerivative(MathFunction function) =>
        new MathFunction(function.Entity.Differentiate("x").Stringize());

    public static double GetRightLimit(MathFunction function, double point) =>
        GetLimit(function, point, ApproachFrom.Right);

    public static double GetLeftLimit(MathFunction function, double point) =>
        GetLimit(function, point, ApproachFrom.Left);

    private static double GetLimit(
        MathFunction function, double point, ApproachFrom from = ApproachFrom.BothSides
    )
    {
        Entity entityPoint = double.IsNegativeInfinity(point) ? "-oo" :
            double.IsPositiveInfinity(point) ? "+oo" : point;
        var limit = function.Entity.Limit("x", entityPoint, from);
        if (limit is Entity.Limitf || limit.Stringize() == "NaN")
            throw new ComplicatedFunctionException();
        if (limit is Entity.Divf || limit.Evaled.Stringize().Length > 10) limit = "+oo";
        if (limit.Evaled.IsFinite) return double.Parse(limit.Evaled.Stringize());
        return limit.Stringize() == "+oo" ? double.PositiveInfinity : double.NegativeInfinity;
    }

    public static PointSet GetZerosFunctionInRange(MathFunction function, FunctionRange range)
    {
        var set = ($"{function.Entity} = 0 and x in RR"
            .Solve("x").Evaled.DirectChildren);
        return FindSolutionsInCollections(set, range, function.Compile());
    }

    private static PointSet FindSolutionsInCollections(
        IEnumerable<Entity> set, FunctionRange range, Func<double, double> func)
    {
        var zeros = new PointSet();
        foreach (var entity in set)
        {
            var param = entity.Vars.FirstOrDefault();
            if (param is null)
            {
                if (entity is not Entity.Number.Real number) continue;
                foreach (var zero in FindSolutions((double)number.EDecimal, func, range))
                    zeros.Add(zero);
            }
            else
            {
                var compiledSolution = entity.Compile<Func<double, double>>(
                    new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
                    {
                        (typeof(double), param)
                    });
                for (var i = range.From; i < range.To; i++)
                    foreach (var zero in FindSolutions(compiledSolution(i), func, range))
                        zeros.Add(zero);
            }
        }

        return zeros;
    }

    private static IEnumerable<Point> FindSolutions(double x, Func<double, double> func,
        FunctionRange range)
    {
        if (x > range.From && x < range.To && func(x) < Delta)
        {
            yield return new Point(x, 0);
        }
    }

    public static IEnumerable<double> GetExtremaPoints(
        MathFunction derivative,
        FunctionRange functionRange
    )
    {
        var compiledDerivative = derivative.Compile();
        var zeros = GetZerosFunctionInRange(derivative, functionRange);
        foreach (var point in zeros.Points)
        {
            var n1 = compiledDerivative(point.X - Delta);
            var n2 = compiledDerivative(point.X + Delta);
            if (n1 * n2 < 0) yield return point.X;
        }
    }
}