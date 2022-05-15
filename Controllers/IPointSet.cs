using System.Collections.Generic;

namespace Funcan.Controllers;

public interface IPointSet
{
    public List<Point> Points { get;  }
    public string Color { get; }

    public string GetJsonBody();
}