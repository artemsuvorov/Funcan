using System.Collections.Generic;

namespace Funcan.Domain.Models;

public record Plot(IEnumerable<PointSet> PointSet, DrawType DrawType, Color Color);