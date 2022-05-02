using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Funcan.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FunctionInputController
    {
        private          Dictionary<Regex, string>        _dictionary;
        private readonly ILogger<FunctionInputController> _logger;

        public FunctionInputController(ILogger<FunctionInputController> logger)
        {
            _logger     = logger;
            _dictionary = new Dictionary<Regex, string>();
        }

        [HttpGet]
        public string GetFunction(string input)
        {
            var validatedFunction  = ValidateInputFunction(input);
            var expression         = new NCalc.Expression(validatedFunction);
            var functionExpression = CreateExpression(expression);
            var f                  = functionExpression.Compile();
            var printer = new FunctionPainter(f);
            printer.Paint();
            return validatedFunction;
        }

        private static string ValidateInputFunction(string input)
        {
            var regex = new Regex(@"(\((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!))\)|\w)\^(\((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!))\)|\w)");
            return regex.Replace(input, match => $"Pow({match.Groups[1]},{match.Groups[2]})");
        }

        private static Expression<Func<double, double, double>> CreateExpression(NCalc.Expression expression)
        {
            Func<double, double, double> function = (x, y) =>
            {
                expression.Parameters["x"] = x;
                expression.Parameters["y"] = y;
                return double.Parse(expression.Evaluate().ToString() ?? throw new InvalidOperationException());
            };

            return (x, y) => function(x, y);
        }
    }
}