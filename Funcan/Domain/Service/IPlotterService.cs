using System.Collections.Generic;
using Funcan.Domain.Models;

namespace Funcan.Domain.Service;

public interface IPlotterService
{
    List<Plot> GetPlots(MathFunction function, FunctionRange range, IEnumerable<string> plotters);

    List<PlotterInfo> GetPlotterInfos();
}