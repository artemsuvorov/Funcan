using System;
using System.Collections.Generic;
using System.Linq;
using AngouriMath;
using AngouriMath.Core.Compilation.IntoLinq;
using AngouriMath.Extensions;
using Funcan.Domain.Models;
using Funcan.Domain.Utils;

namespace Funcan.Domain;

public static class ExtendedMath
{
    public static MathFunction GetDerivative(MathFunction function)
    {
        return new MathFunction(function.Function.Differentiate("x").Stringize());
    }

    public static MathFunction GetLimit(MathFunction function, double point)
    {
        Entity entityPoint = double.IsNegativeInfinity(point) ? "-oo" :
            double.IsPositiveInfinity(point) ? "+oo" : point.ToString();
        return new MathFunction(function.Function.Limit("x", entityPoint).Stringize());
    }

    public static PointSet GetZerosFunctionInRange(MathFunction function, FunctionRange range)
    {
        var zeros = new PointSet();
        var compiledFunc = function.Function.Compile<Func<double, double>>(new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
        {
            (typeof (double), "x")
        });
        var set = ($"{function.Function} = 0 and x in RR".Solve("x").Evaled.DirectChildren);
        FindSolutionsInCollections(set, zeros, range, compiledFunc);
        return zeros;
    }

    public static void FindSolutionsInCollections(
        IEnumerable<Entity> set, PointSet zeros, FunctionRange range, Func<double, double> func)
    {
        foreach (var entity in set)
        {
            var solution = entity;
            var param = solution.Vars.FirstOrDefault();
            if (param is null)
            {
                if (solution is Entity.Number.Real number)
                {
                    var xParam = (double) number.EDecimal;
                    AddIfSolution(xParam, func, zeros, range);
                }
            }
            else
            {
                var compiledSolution = solution.Compile<Func<double, double>>(
                    new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
                    {
                        (typeof (double), param)
                    });;
                for (var i = range.From; i < range.To; i++)
                {
                    var xParam = compiledSolution(i);
                    AddIfSolution(xParam, func, zeros, range);
                }
            }
        }
    }

    public static void AddIfSolution(double x, Func<double, double> func, PointSet points, FunctionRange range)
    {
        var delta = 0.01;
        if (x > range.From && x < range.To && func(x) < delta)
        {
            var point = new Point(x, 0);
            points.Add(point);
        }
    }

    public static PointSet GetCriticalPoints(MathFunction function, FunctionRange functionRange, MathFunction derivative)
    {
        var compiledFunc = function.Function.Compile<Func<double, double>>(
            new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
        {
            (typeof (double), "x")
        });
        var compiledDerivative = derivative.Function.Compile<Func<double, double>>(
            new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
        {
            (typeof (double), "x")
        });
        var zeros = GetZerosFunctionInRange(new MathFunction(derivative.Function.Stringize()), functionRange);
        var delta = 0.01;
        var criticalPoints = new PointSet();
        foreach (var point in zeros.Points)
        {
            var n1 = compiledDerivative(point.X - delta);
            var n2 = compiledDerivative(point.X + delta);
            if (n1 * n2 < 0) criticalPoints.Add(point with {Y = compiledFunc(point.X)});
        }
        return criticalPoints;
    }
}