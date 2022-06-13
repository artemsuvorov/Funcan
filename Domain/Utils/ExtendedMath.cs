using System;
using System.Collections.Generic;
using System.Linq;
using AngouriMath;
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
        PointSet zeros = new PointSet();
        var compiledFunc = function.Function.Compile("x");
        var set = function.Function.SolveEquation("x").DirectChildren;
        foreach (var entity in set)
        {
            foreach (var solution in entity.Evaled.DirectChildren)
            {
                var param = solution.Vars.FirstOrDefault();
                if (param is null)
                {
                    var xParam = double.Parse(solution.Stringize());
                    var point = new Point(xParam, compiledFunc.Call(xParam).Real);
                    zeros.Add(point);
                }
                else
                {
                    var compiledSolution = solution.Compile(param);
                    for (var i = range.From; i < range.To; i++)
                    {
                        var xComplex = compiledSolution.Call(i);
                        if (xComplex.Imaginary != 0 && xComplex.Real > range.From && xComplex.Real < range.To)
                        {
                            var xParam = xComplex.Real;
                            var point = new Point(xParam, compiledFunc.Call(xParam).Real);
                            zeros.Add(point);
                        }
                    }
                }
            }
        }
        return zeros;
    }
}