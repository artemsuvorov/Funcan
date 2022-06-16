using System.Collections.Generic;

namespace Funcan.Domain.Models;

public record Plot(List<PointSet> PointSet, PlotterInfo PlotterInfo);