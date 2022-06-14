using System;
using System.Linq;
using AngouriMath;
using AngouriMath.Core.Exceptions;

namespace Funcan.Domain;

public class MathFunction
{
    public Entity Function { get; }

    public MathFunction(string function)
    {
        Validate(function);
        function = function.Replace("tan(x)", "(sin(x)/cos(x))");
        function = function.Replace("cot(x)", "(cos(x)/sin(x))");
        Function = function;
    }

    private static void Validate(string functionStr)
    {
        try
        {
            Entity function = functionStr;
            MathS.Parse(function.Stringize()).Switch(
                valid => valid,
                _ => throw new ArgumentException("Некорректное выражение")
            );
            var vars = function.Vars.Where(variable => variable != "x").ToList();
            if (vars.Count > 0) throw new ArgumentException("Некорректное выражение");
        }
        catch (UnhandledParseException)
        {
            throw new ArgumentException("Некорректное выражение");
        }
    }

    public static MathFunction operator /(MathFunction a, MathFunction b)
    {
        return new MathFunction($"({a.Function}) * (1 / ({b.Function}))");
    }

    public static MathFunction operator +(MathFunction a, MathFunction b)
    {
        return new MathFunction($"({a.Function}) + ({b.Function})");
    }

    public static MathFunction operator -(MathFunction a, MathFunction b)
    {
        return new MathFunction($"({a.Function}) - ({b.Function})");
    }

    public static MathFunction operator *(MathFunction a, MathFunction b)
    {
        return new MathFunction($"({a.Function}) * ({b.Function})");
    }

    public static MathFunction operator *(double a, MathFunction b)
    {
        return new MathFunction($"{a} * ({b.Function})");
    }

    public static MathFunction operator +(MathFunction b, double a)
    {
        return new MathFunction($"({b.Function}) + {a}");
    }
}