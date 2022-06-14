using System.Collections.Generic;
using Funcan.Domain.Models;

namespace Funcan.Service;

public interface IPlotterService
{
    List<Plot> GetPlots(MathFunction function, FunctionRange range, IEnumerable<PlotterInfo> analysisOptions);

    List<PlotterInfo> GetPlotterInfos();
}