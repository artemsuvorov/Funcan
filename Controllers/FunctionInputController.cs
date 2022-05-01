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
        private          Dictionary<Regex, string>          _dictionary;
        private readonly ILogger<WeatherForecastController> _logger;

        public FunctionInputController(ILogger<WeatherForecastController> logger)
        {
            _logger     = logger;
            _dictionary = new Dictionary<Regex, string>();
        }

        [HttpGet]
        public string GetFunction(string input)
        {
            var expression         = new NCalc.Expression(input);
            var functionExpression = CreateExpression(expression);
            var f                  = functionExpression.Compile();
            return f(2).ToString(CultureInfo.InvariantCulture);
        }

        private static Expression<Func<double, double>> CreateExpression(NCalc.Expression expression)
        {
            Func<double, double> function = x =>
            {
                expression.Parameters["x"] = x;
                return double.Parse(expression.Evaluate().ToString() ?? throw new InvalidOperationException());
            };

            return x => function(x);
        }
    }
}