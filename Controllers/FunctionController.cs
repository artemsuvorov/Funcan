using System;
using System.Collections.Generic;
using System.Linq;
using Funcan.Domain;
using Funcan.Domain.Models;
using Funcan.Domain.Parsers;
using Funcan.Domain.Plotters;
using Funcan.Domain.Repository;
using Funcan.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Funcan.Controllers;

[ApiController]
[Route("[controller]")]
public class FunctionController : Controller
{
    private readonly IHistory history;
    private readonly IFunctionParser functionParser;
    private readonly ILogger<FunctionController> logger;
    private readonly IEnumerable<IPlotter> plotters;

    public FunctionController(
        IFunctionParser functionParser,
        ILogger<FunctionController> logger,
        IEnumerable<IPlotter> plotters,
        IHistory history)
    {
        this.history = history;
        this.functionParser = functionParser;
        this.logger = logger;
        this.plotters = plotters;
    }


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

        var userId = HttpContext.Request.Cookies["user_id"];
        if (userId is not null && int.TryParse(userId, out var id))
            history.Save(id, new HistoryEntry(inputFunction, from, to, plotterInfos.ToList()));

        try
        {
            // var function = new MathFunction(inputFunction);
            var function = functionParser.Parse(inputFunction);
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
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<PlotterInfo>> GetAnalysisOptions() =>
        plotters.Select(plotter => plotter.PlotterInfo).ToList();

    [HttpGet]
    [Route("History")]
    [ProducesResponseType(200, Type = typeof(List<HistoryEntry>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<HistoryEntry>> GetHistory()
    {
        var userId = HttpContext.Request.Cookies["user_id"];
        if (userId is not null && int.TryParse(userId, out var id))
            return history.Get(id);
    
        return null;
    }
}