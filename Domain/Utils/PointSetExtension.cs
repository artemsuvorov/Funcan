using System;
using Funcan.Domain.Models;

namespace Funcan.Domain;

public static class PointSetExtension
{
    private static bool GetMonotone(PointSet pointSet, double inf, Func<double, double, bool> comparer)
    {
        var current = inf;
        foreach (var point in pointSet.Points)
        {
            if (comparer(point.Y, current)) return false;
            current = point.Y;
        }

        return true;
    }


    public static bool IsMonotone(this PointSet pointSet)
    {
        return GetMonotone(pointSet, double.PositiveInfinity, (d, d1) => d > d1) ||
               GetMonotone(pointSet, double.NegativeInfinity, (d, d1) => d < d1);
    }
}