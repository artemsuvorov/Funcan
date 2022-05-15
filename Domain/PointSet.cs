using System.Collections.Generic;
using Funcan.Controllers;

namespace Funcan.Domain
{
    public class PointSet
    {
        public Style Style { get; }
        private readonly List<Point> points = new();
        public PointSet(Style style) => Style = style;
        public void Add(Point point) => points.Add(point);
        public IEnumerable<Point> Points => points;
    }
}