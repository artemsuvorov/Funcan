using System;
using System.Collections.Generic;
using System.Linq;
using Funcan.Domain;
using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using Funcan.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Funcan.Controllers;

[ApiController]
[Route("[controller]")]
public class FunctionController : Controller
{
    private readonly IEnumerable<IPlotter> plotters;

    public FunctionController(IEnumerable<IPlotter> plotters) =>
        this.plotters = plotters;


    [HttpPost]
    [Route("")]
    [ProducesResponseType(200, Type = typeof(List<Plot>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<Plot>> GetFunction(
        [FromQuery(Name = "input")] string inputFunction,
        [FromBody] IEnumerable<PlotterInfo> analysisOptions,
        [FromQuery(Name = "from")] double from = -10,
        [FromQuery(Name = "to")] double to = 10
    )
    {
        var plotterInfos = analysisOptions.ToList();
        var necessaryPlotters = plotterInfos.Select(option => option.Name).ToHashSet();
        try
        {
            var function = new MathFunction(inputFunction);
            var plots = plotters
                .Where(plotter => necessaryPlotters.Contains(plotter.PlotterInfo.Name))
                .Select(plotter =>
                    PointsetWrapper.Wrap(
                        plotter.GetPointSets(function, new FunctionRange(from, to)),
                        plotter.PlotterInfo
                    ))
                .ToList();
            return plots;
        }
        catch (ArgumentException e)
        {
            var result = new ContentResult
            {
                Content = e.Message,
                StatusCode = StatusCodes.Status400BadRequest,
                ContentType = "string"
            };
            return result;
        }
    }

    [HttpGet]
    [Route("Plotters")]
    [ProducesResponseType(200, Type = typeof(List<PlotterInfo>))]
    public ActionResult<List<PlotterInfo>> GetAnalysisOptions() =>
        plotters.Select(plotter => plotter.PlotterInfo).ToList();
}