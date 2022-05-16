using System;
using System.Collections.Generic;
using System.Linq;
using Funcan.Domain;
using Funcan.Solvers;
using NCalc;
using Newtonsoft.Json;

namespace Funcan.Services;

public class FunctionService : IFunctionService
{
    private IEnumerable<IFuncSolver> Solvers { get; }

    public FunctionService(IEnumerable<IFuncSolver> solvers) => Solvers = solvers;

    public string ResolveInput(string input)
    {
        var expression = new Expression(input);
        var function = CreateFunction(expression);
        if (!CheckValidation(function)) return JsonConvert.SerializeObject(new ErrorMessage());
        var pointSets = Solvers.Select(solver => solver.Solve(function));
        return JsonConvert.SerializeObject(pointSets);
    }

    private static bool CheckValidation(Func<double, double> function)
    {
        try
        {
            function(0);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static Func<double, double> CreateFunction(Expression expression)
    {
        expression.Parameters["e"] = Math.E;
        expression.Parameters["pi"] = Math.PI;

        return x =>
        {
            expression.Parameters["x"] = x;
            return double.Parse(expression.Evaluate().ToString() ?? throw new ArgumentException());
        };
    }
}