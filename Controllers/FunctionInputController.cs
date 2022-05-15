using System;
using System.Collections.Generic;
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
        public ContentResult GetFunction(string input)
        {
            var expression         = new NCalc.Expression(input);
            var functionExpression = CreateExpression(expression);
            // Draw(functionExpression);
            var points = MakePoints(functionExpression.Compile());

            return new ContentResult
            {
                ContentType = "text/html",
                Content     = points.GetJsonBody()
            };
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

        private static PointSet MakePoints(Func<double, double> function)
        {
            var points = new PointSet(new List<Point>());
            for (double x = -100; x <= 100; x += 1f)
            {
                var y = function(x);
                points.AddPoint(new Point(x, y));
            }

            return points;
        }
    }
}