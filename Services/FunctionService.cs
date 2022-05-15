using System;
using System.Collections.Generic;
using System.Linq;
using Funcan.Solvers;
using Newtonsoft.Json;

namespace Funcan.Services
{
    public class FunctionService
    {
        private IEnumerable<IFuncSolver> Solvers { get; }

        public FunctionService(IEnumerable<IFuncSolver> solvers) => Solvers = solvers;

        public string ResolveInput(string input)
        {
            var expression = new NCalc.Expression(input);
            var function = CreateFunction(expression);
            var pointSets = Solvers.Select(solver => solver.Solve(function));
            return JsonConvert.SerializeObject(pointSets);
        }

        private static Func<double, double> CreateFunction(NCalc.Expression expression)
        {
            return x =>
            {
                expression.Parameters["x"] = x;
                return double.Parse(expression.Evaluate().ToString() ?? throw new InvalidOperationException());
            };
        }
    }
}