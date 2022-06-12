using System;
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
        throw new NotImplementedException();
    }

    public static MathFunction GetLimit(MathFunction function)
    {
        throw new NotImplementedException();
    }

    public static PointSet SolveEquationInRange(MathFunction function, FunctionRange range)
    {
        throw new NotImplementedException();
    }
}