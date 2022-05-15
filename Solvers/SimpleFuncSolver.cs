using System;
using System.Drawing;
using Funcan.Domain;
using Point = Funcan.Controllers.Point;
using PointSet = Funcan.Domain.PointSet;

namespace Funcan.Solvers
{
    public class SimpleFuncSolver : IFuncSolver
    {
        public PointSet Solve(Func<double, double> function)
        {
            var points = new PointSet(new Style(Color.Blue, Style.DisplayingType.Line));
            for (double x = -10; x <= 10; x += 0.5f)
            {
                var y = function(x);
                points.Add(new Point(x, y));
            }

            return points;
        }
    }
}