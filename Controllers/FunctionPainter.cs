using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Plotly.NET;


namespace Funcan.Controllers
{
    public class FunctionPainter
    {
        private readonly        Func<double, double> _mathFunc;
        private static readonly double               Epsilon = Math.Pow(Math.E, -3);

        public FunctionPainter(Func<double, double> mathFunc)
        {
            _mathFunc = mathFunc;
        }

        public void Paint()
        {
            var points = GetPoints();
            var chart  = Chart2D.Chart.Spline<double, double, string>(points);
            chart.WithXAxisStyle(title: Title.init("xAxis"), ShowGrid: false, ShowLine: true)
                .WithYAxisStyle(title: Title.init("yAxis"), ShowGrid: false, ShowLine: true)
                .SaveHtml(@"Functions/function");
        }

        private IEnumerable<Tuple<double, double>> GetPoints()
        {
            var points = new List<Tuple<double, double>>();
            for (double x = -100; x <= 100; x += 0.001f)
            {
                var y = _mathFunc(x);
                points.Add(Tuple.Create(x, y));
            }

            return points;
        }
    }
}