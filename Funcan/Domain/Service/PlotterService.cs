using System.Collections.Generic;
using System.Linq;
using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using Funcan.Domain.Utils;

namespace Funcan.Service;

public class PlotterService : IPlotterService
{
    private IEnumerable<IPlotter> Plotters { get; }
    public PlotterService(IEnumerable<IPlotter> plotters) => Plotters = plotters;

    public List<Plot> GetPlots(MathFunction function, FunctionRange range, IEnumerable<PlotterInfo> analysisOptions)
    {
        var plotterInfos = analysisOptions.ToList();
        var necessaryPlotters = plotterInfos.Select(option => option.Name).ToHashSet();

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