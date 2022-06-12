using System;
using NCalc;

namespace Funcan.Domain.Parsers;

public class FunctionParser : IFunctionParser
{
    public Func<double, double> Parse(string function)
    {
        var expression = new Expression(function)
        {
            Parameters =
            {
                ["e"] = Math.E,
                ["pi"] = Math.PI
            }
        };

        return x =>
        {
            expression.Parameters["x"] = x;
            return double.Parse(expression.Evaluate().ToString() ?? throw new ArgumentException(expression.Error));
        };
    }
}