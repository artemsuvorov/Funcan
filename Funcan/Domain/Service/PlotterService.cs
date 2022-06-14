using System.Collections.Generic;
using System.Linq;
using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using Funcan.Domain.Utils;

namespace Funcan.Domain.Service;

public class PlotterService : IPlotterService
{
    private IEnumerable<IPlotter> Plotters { get; }
    public PlotterService(IEnumerable<IPlotter> plotters) => Plotters = plotters;

    public List<Plot> GetPlots(MathFunction function, FunctionRange range, IEnumerable<string> plotters)
    {
        var necessaryPlotters = plotters.ToHashSet();

        return Plotters
            .Where(plotter => necessaryPlotters.Contains(plotter.PlotterInfo.Name))
            .Select(plotter =>
                PointsetWrapper.Wrap(
                    plotter.GetPointSets(function, range),
                    plotter.PlotterInfo
                ))
            .ToList();
    }

    public List<PlotterInfo> GetPlotterInfos() =>
        Plotters.Select(plotter => plotter.PlotterInfo).ToList();
}