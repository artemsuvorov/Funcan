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
    public static double GetLeftLimit(Func<double, double> f, double x)
    {
        return f(x - double.Epsilon);
    }

    public static double GetRightLimit(Func<double, double> f, double x)
    {
        return f(x + double.Epsilon);
    }

    public static double GetLimit(Func<double, double> f, double x)
    {
        var right = GetRightLimit(f, x);
        var left = GetLeftLimit(f, x);
        if (right is double.NaN || left is double.NaN) return double.NaN;
        if (double.IsInfinity(right) && double.IsInfinity(left)) return double.PositiveInfinity;
        return Math.Abs(right - left) < double.Epsilon ? right : double.NaN;
    }

    public static double GetDerivative(Func<double, double> f, double x)
    {
        Func<double, double> newF = xCoordinate => (f(xCoordinate + 0.00001) - f(xCoordinate)) / (0.00001f);
        var limit = GetLimit(newF, x);
        return double.IsInfinity(limit) ? double.NaN : limit;
    }

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
        foreach (var entity in set)
        {
            var solution = entity;
            var param = solution.Vars.FirstOrDefault();
            if (param is null)
            {
                if (solution is Entity.Number.Real number)
                {
                    var xParam = (double) number.EDecimal;
                    if (xParam > range.From && xParam < range.To && compiledFunc(xParam) < 0.01)
                    {
                        var point = new Point(xParam, 0);
                        zeros.Add(point);
                    }
                }
            }
            else
            {
                var compiledSolution = solution.Compile<Func<double, double>>(new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1]
                {
                    (typeof (double), param)
                });;
                for (var i = range.From; i < range.To; i++)
                {
                    var xParam = compiledSolution(i);
                    if (xParam > range.From && xParam < range.To && compiledFunc(xParam) < 0.01)
                    {
                        var point = new Point(xParam, 0);
                        zeros.Add(point);
                    }
                }
            }
        }
        return zeros;
    }
}