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

    private static void Validate(string function)
    {
        var a = function.Solve("x");
        
    }
}