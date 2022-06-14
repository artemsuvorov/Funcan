using System;
using System.Globalization;
using System.Linq;
using AngouriMath;
using AngouriMath.Core;
using AngouriMath.Core.Compilation.IntoLinq;
using AngouriMath.Extensions;
using Funcan.Domain.Models;

namespace Funcan.Domain.Utils;

public static class ExtendedMath
{
    public static MathFunction GetDerivative(MathFunction function)
    {
        return new MathFunction(function.Function.Differentiate("x").Stringize());
    }

    public static double GetRightLimit(MathFunction function, double point)
    {
        return GetLimit(function, point, ApproachFrom.Right);
    }

    public static double GetLeftLimit(MathFunction function, double point)
    {
        return GetLimit(function, point, ApproachFrom.Left);
    }


    public static double GetLimit(MathFunction function, double point, ApproachFrom from = ApproachFrom.BothSides)
    {
        Entity entityPoint = double.IsNegativeInfinity(point) ? "-oo" :
            double.IsPositiveInfinity(point) ? "+oo" : point;
        var limit = function.Function.Limit("x", entityPoint, from);
        if (limit is Entity.Limitf) throw new ArgumentException("Слишком сложно(");
        if (limit.Stringize() == "NaN") throw new ArgumentException("Слишком сложно(");
        if (limit is Entity.Divf) limit = "+oo";
        if (limit.Evaled.Stringize().Length > 10) limit = "+oo";
        if (limit.Evaled.IsFinite)
        {
            return double.Parse(limit.Evaled.Stringize());
        }

        return limit.Stringize() == "+oo" ? double.PositiveInfinity : double.NegativeInfinity;
    }

    public static PointSet GetZerosFunctionInRange(MathFunction function, FunctionRange range)
    {
        var zeros = new PointSet();
        var compiledFunc = function.Function.Compile<Func<double, double>>(new CompilationProtocol(), typeof(double),
            new (Type, Entity.Variable)[1]
            {
                (typeof(double), "x")
            });
        var set = $"{function.Function} = 0 and x in RR".Solve("x").Evaled.DirectChildren;
        foreach (var entity in set)
        {
            var solution = entity;
            var param = solution.Vars.FirstOrDefault();
            if (param is null)
            {
                if (solution is not Entity.Number.Real number) continue;
                var xParam = (double) number.EDecimal;
                if (!(xParam > range.From) || !(xParam < range.To) || !(compiledFunc(xParam) < 0.01)) continue;
                var point = new Point(xParam, 0);
                zeros.Add(point);
            }
            else
            {
                var compiledSolution = solution.Compile<Func<double, double>>(new CompilationProtocol(), typeof(double),
                    new (Type, Entity.Variable)[1]
                    {
                        (typeof(double), param)
                    });
                for (var i = range.From; i < range.To; i++)
                {
                    var xParam = compiledSolution(i);
                    if (!(xParam > range.From) || !(xParam < range.To) || !(compiledFunc(xParam) < 0.01)) continue;
                    var point = new Point(xParam, 0);
                    zeros.Add(point);
                }
            }
        }

        return zeros;
    }
}