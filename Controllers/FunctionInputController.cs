using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plotly.NET;
using Expression = NCalc.Expression;

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
            var validatedFunction  = ValidateInputFunction(input);
            var expression         = new NCalc.Expression(validatedFunction);
            var functionExpression = CreateExpression(expression);
            Draw(functionExpression);

            Console.WriteLine(validatedFunction);
            return new ContentResult
            {
                ContentType = "text/html",
                Content     = File.ReadAllText(@"Functions/function.html")
            };
        }

        private static string ValidateInputFunction(string input)
        {
            var regex = new Regex(@"(\((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!))\)|\w+)\^(\((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!))\)|\w+)");
            return regex.Replace(input, match => $"Pow({match.Groups[1]},{match.Groups[2]})");
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

        private static void Draw(Expression<Func<double, double>> expression)
        {
            var points = MakePoints(expression.Compile());
            var chart  = Chart2D.Chart.Spline<double, double, string>(points);
            chart.WithXAxisStyle(Title.init("xAxis"), ShowGrid: false, ShowLine: true)
                .WithYAxisStyle(Title.init("yAxis"), ShowGrid: false, ShowLine: true)
                .SaveHtml(@"Functions/function");
        }

        private static IEnumerable<Tuple<double, double>> MakePoints(Func<double, double> function)
        {
            var points = new List<Tuple<double, double>>();
            for (double x = -100; x <= 100; x += 0.001f)
            {
                var y = function(x);
                points.Add(Tuple.Create(x, y));
            }

            return points;
        }
    }
}