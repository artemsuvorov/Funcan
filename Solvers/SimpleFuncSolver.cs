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
            for (var x = -10d; x <= 10d; x += 0.2d)
            {
                var y = function(x);
                points.Add(new Point(x, y));
            }

            return points;
        }
    }
}