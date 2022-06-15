using System.Collections.Generic;

namespace Funcan.Domain.Models;

public class PointSet {
    private readonly List<Point> points = new();
    public void Add(Point point) => points.Add(point);

    public void AddPointSet(PointSet set) => points.AddRange(set.points);
    public IReadOnlyList<Point> Points => points;
}