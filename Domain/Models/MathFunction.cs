using System;
using System.Linq;
using System.Text.RegularExpressions;
using AngouriMath;
using AngouriMath.Core.Exceptions;
using AngouriMath.Extensions;

namespace Funcan.Domain.Models;

public class MathFunction
{
    public Entity Function { get; }

    public MathFunction(string function)
    {
        Validate(function);
        function = Regex.Replace(function, @"(?<!arc)((tan)\(x\))", "(sin(x)/cos(x))");
        function = Regex.Replace(function, @"(?<!arc)((cot)\(x\))", "(cos(x)/sin(x))");
        function.Simplify();
        Function = function;
    }

    private static void Validate(string functionStr)
    {
        var errorMessage = "Некорректное выражение";
        try
        {
            Entity function = functionStr;
            MathS.Parse(function.Stringize()).Switch(
                valid => valid,
                _ => throw new ArgumentException(errorMessage)
            );
            var vars = function.Vars.Where(variable => variable != "x").ToList();
            if (vars.Count > 0) throw new ArgumentException(errorMessage);
        }
        catch (UnhandledParseException)
        {
            throw new ArgumentException(errorMessage);
        }
    }

    public static MathFunction operator /(MathFunction a, MathFunction b)
    {
        return new MathFunction($"({a.Function}) / ({b.Function})");
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