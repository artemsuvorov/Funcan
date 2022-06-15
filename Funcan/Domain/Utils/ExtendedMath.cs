using System;
using System.Collections.Generic;
using System.Linq;
using AngouriMath;
using AngouriMath.Core;
using AngouriMath.Core.Compilation.IntoLinq;
using AngouriMath.Core.Exceptions;
using AngouriMath.Extensions;
using Funcan.Domain.Exceptions;
using Funcan.Domain.Models;

namespace Funcan.Domain.Utils;

public static class ExtendedMath {
    public static MathFunction GetDerivative(MathFunction function){
        MathFunction.TryCreate(function.Entity.Differentiate("x").Stringize(), out var derivative);
        return derivative;
    }

    public static double GetRightLimit(MathFunction function, double point){
        return GetLimit(function, point, ApproachFrom.Right);
    }

    public static double GetLeftLimit(MathFunction function, double point){
        return GetLimit(function, point, ApproachFrom.Left);
    }

    public static double GetLimit(MathFunction function, double point, ApproachFrom from = ApproachFrom.BothSides){
        Entity entityPoint = double.IsNegativeInfinity(point) ? "-oo" :
            double.IsPositiveInfinity(point) ? "+oo" : point;
        var limit = function.Entity.Limit("x", entityPoint, from);
        if (limit is Entity.Limitf) throw new ComplicatedFunctionException();
        if (limit.Stringize() == "NaN") throw new ComplicatedFunctionException();
        if (limit is Entity.Divf) limit = "+oo";
        if (limit.Evaled.Stringize().Length > 10) limit = "+oo";
        if (limit.Evaled.IsFinite){
            return double.Parse(limit.Evaled.Stringize());
        }

        return limit.Stringize() == "+oo" ? double.PositiveInfinity : double.NegativeInfinity;
    }

    public static PointSet GetZerosFunctionInRange(MathFunction function, FunctionRange range){
        var zeros = new PointSet();
        var compiledFunc = function.Entity.Compile<Func<double, double>>(new CompilationProtocol(), typeof(double),
            new (Type, Entity.Variable)[1] {
                (typeof(double), "x")
            });
        var set = ($"{function.Entity} = 0 and x in RR".Solve("x").Evaled.DirectChildren);
        FindSolutionsInCollections(set, zeros, range, compiledFunc);
        return zeros;
    }

    public static void FindSolutionsInCollections(
        IEnumerable<Entity> set, PointSet zeros, FunctionRange range, Func<double, double> func){
        foreach (var entity in set){
            var solution = entity;
            var param = solution.Vars.FirstOrDefault();
            if (param is null){
                if (solution is Entity.Number.Real number){
                    var xParam = (double)number.EDecimal;
                    AddIfSolution(xParam, func, zeros, range);
                }
            }
            else{
                var compiledSolution = solution.Compile<Func<double, double>>(
                    new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1] {
                        (typeof(double), param)
                    });
                ;
                for (var i = range.From; i < range.To; i++){
                    var xParam = compiledSolution(i);
                    AddIfSolution(xParam, func, zeros, range);
                }
            }
        }
    }

    public static void AddIfSolution(double x, Func<double, double> func, PointSet points, FunctionRange range){
        var delta = 0.01;
        if (x > range.From && x < range.To && func(x) < delta){
            var point = new Point(x, 0);
            points.Add(point);
        }
    }

    public static PointSet GetExtremaPoints(
        MathFunction function,
        FunctionRange functionRange,
        MathFunction derivative
    ){
        var compiledFunc = function.Entity.Compile<Func<double, double>>(
            new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1] {
                (typeof(double), "x")
            });
        var compiledDerivative = derivative.Entity.Compile<Func<double, double>>(
            new CompilationProtocol(), typeof(double), new (Type, Entity.Variable)[1] {
                (typeof(double), "x")
            });
        MathFunction.TryCreate(derivative.Entity.Stringize(), out var derivativeFunc);
        var zeros = GetZerosFunctionInRange(derivativeFunc, functionRange);
        var delta = 0.01;
        var criticalPoints = new PointSet();
        foreach (var point in zeros.Points){
            var n1 = compiledDerivative(point.X - delta);
            var n2 = compiledDerivative(point.X + delta);
            if (n1 * n2 < 0) criticalPoints.Add(point with { Y = compiledFunc(point.X) });
        }

        return criticalPoints;
    }
}