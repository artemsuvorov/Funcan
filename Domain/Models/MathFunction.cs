// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Funcan.Controllers;
//
// namespace Funcan.Domain;
//
// public class MathFunction
// {
//     public readonly Func<double, double> Func;
//     public List<PointSet> PointSets { get; set; }
//
//     public MathFunction(Func<double, double> func, List<PointSet> pointSets)
//     {
//         Func = func;
//         PointSets = pointSets;
//     }
//
//     // public PointSet GetDiscontinuities()
//     // {
//     //     var lims = GetLimits();
//     //     return lims.Where(t => double.IsNaN(t.Item2) || double.IsInfinity(t.Item2));
//     // }
//
//     private List<(double, double)> GetLimits()
//     {
//         return PointSets.SelectMany(x => x.Points).Select(point => (point.X, Limit.GetLimit(Func, point.X))).ToList();
//     }
//
//     // public List<PointSet> GetPointSet(Range range)
//     // {
//     //     var points = new PointSet(new Style(new Color("Blue"), Style.DisplayingType.Line));
//     //     for (var x = range.From; x <= range.To; x += 0.2d)
//     //     {
//     //         var y = Func(x);
//     //         points.Add(new Point(x, y));
//     //     }
//     //
//     //     this.PointSet = points;
//     //     return new List<PointSet> { points };
//     // }
//     //
//     // public PointSet GetTochkiRazriva()
//     // {
//     //     pointSet.Points.Where(x => GetLimit(x, func) == double.NaN);
//     // }
//     
//     
// }