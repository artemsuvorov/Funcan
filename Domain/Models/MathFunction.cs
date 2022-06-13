using System;
using System.Collections.Generic;
using System.Linq;
using AngouriMath;
using AngouriMath.Extensions;
using Funcan.Controllers;
using Funcan.Domain.Models;

namespace Funcan.Domain;

public class MathFunction
{
    public Entity Function { get; }

    public MathFunction(string function)
    {
        //Validate(function);
        Function = function;
    }

    private static void Validate(Entity function)
    {
        var parsed = MathS.Parse(function.Stringize()).Switch(
            valid => valid,
            _ => throw new ArgumentException("Некорректное выражение")
        );
        var vars = function.Vars.Where(variable => variable != "x").ToList();
        if (vars.Count > 0) throw new ArgumentException("Некорректное выражение");
    }
}