using System.Collections.Generic;
using Newtonsoft.Json;

namespace Funcan.Controllers
{
    public class PointSet : IPointSet
    {
        public List<Point> Points { get; }
        public string Color { get; }

        public PointSet(List<Point> points, string color = "Blue")
        {
            Points = points;
            Color = color;
        }

        public void AddPoint(Point point) => Points.Add(point);

        public string GetJsonBody() => JsonConvert.SerializeObject(this);
    }
}