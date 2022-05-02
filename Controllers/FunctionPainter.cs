using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Plotly.NET;


namespace Funcan.Controllers
{
    public class FunctionPainter
    {
        private readonly Func<double, double, double> _mathFunc;
        private static readonly double Epsilon = Math.Pow(Math.E, -3);

        public FunctionPainter(Func<double, double, double> mathFunc)
        {
            _mathFunc = mathFunc;
        }

        public void Paint()
        {
            //var xy = (x, y);
            var points = GetPoints();
            var tuples = new List<Tuple<double, double>>()
                {new Tuple<double, double>(1, 1), new Tuple<double, double>(2, 4), new Tuple<double, double>(3, 9)};
            var chart = Chart2D.Chart.Spline<double, double, string>(points);
            chart.WithXAxisStyle(title: Title.init("xAxis"), ShowGrid: false, ShowLine: true)
                .WithYAxisStyle(title: Title.init("yAxis"), ShowGrid: false, ShowLine: true)
                .Show();
        }

        public IEnumerable<Tuple<double, double>> GetPoints()
        {
            List<Tuple<double, double>> points = new List<Tuple<double, double>>();
            for (double x = -2; x <= 2; x+=0.05f)
            for (double y = 0; y <= 2; y += 0.05f)
            {
                var result = _mathFunc(x, y);
                Console.WriteLine(result);
                if (Math.Abs(result) < Epsilon)
                    points.Add(Tuple.Create(x, y));
            }

            return points;
        }
    }
}