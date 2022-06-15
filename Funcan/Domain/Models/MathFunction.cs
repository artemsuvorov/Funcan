using System;
using System.Linq;
using System.Text.RegularExpressions;
using AngouriMath;
using AngouriMath.Core.Exceptions;
using AngouriMath.Extensions;

namespace Funcan.Domain.Models;

public class MathFunction {
    public Entity Entity { get; }

    public MathFunction(string entity){
        entity = Regex.Replace(entity, @"(?<!arc)((tan)\(x\))", "(sin(x)/cos(x))");
        entity = Regex.Replace(entity, @"(?<!arc)((cot)\(x\))", "(cos(x)/sin(x))");
        entity.Simplify();
        Entity = entity;
    }

    public static bool TryCreate(string str, out MathFunction function){
        if (!IsValid(str)){
            function = null;
            return false;
        }

        function = new MathFunction(str);
        return true;
    }

    private static bool IsValid(string functionStr){
        Entity function;
        try{
            function = functionStr;
        }
        catch (UnhandledParseException){
            return false;
        }

        var vars = function.Vars.Where(variable => variable != "x").ToList();
        return vars.Count == 0;
    }

    public static MathFunction operator /(MathFunction a, MathFunction b){
        return new MathFunction($"({a.Entity}) / ({b.Entity})");
    }

    public static MathFunction operator +(MathFunction a, MathFunction b){
        return new MathFunction($"({a.Entity}) + ({b.Entity})");
    }

    public static MathFunction operator -(MathFunction a, MathFunction b){
        return new MathFunction($"({a.Entity}) - ({b.Entity})");
    }

    public static MathFunction operator *(MathFunction a, MathFunction b){
        return new MathFunction($"({a.Entity}) * ({b.Entity})");
    }

    public static MathFunction operator *(double a, MathFunction b){
        return new MathFunction($"{a} * ({b.Entity})");
    }

    public static MathFunction operator +(MathFunction b, double a){
        return new MathFunction($"({b.Entity}) + {a}");
    }
}