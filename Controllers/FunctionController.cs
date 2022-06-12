using System;
using System.Collections.Generic;
using System.Linq;
using Funcan.Domain.Models;
using Funcan.Domain.Parsers;
using Funcan.Domain.Plotters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Funcan.Controllers;

[ApiController]
[Route("[controller]")]
public class FunctionController
{
    private readonly IFunctionParser functionParser;
    private readonly ILogger<FunctionController> logger;
    private readonly IEnumerable<IPlotter> plotters;

    public FunctionController(
        IFunctionParser functionParser,
        ILogger<FunctionController> logger,
        IEnumerable<IPlotter> plotters
    )
    {
        this.functionParser = functionParser;
        this.logger = logger;
        this.plotters = plotters;
    }


    [HttpPost]
    [ProducesResponseType(200, Type = typeof(List<Plot>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<Plot>> Index(
        [FromQuery(Name = "input")] string inputFunction,
        [FromBody] IEnumerable<string> plotterNames,
        [FromQuery(Name = "from")] double from = -10,
        [FromQuery(Name = "to")] double to = 10
    )
    {
        var necessaryPlotters = plotterNames.ToHashSet();
        try
        {
            var function = functionParser.Parse(inputFunction);
            var plots = plotters
                .Where(plotter => necessaryPlotters.Contains(plotter.PlotterInfo.Name))
                .Select(plotter => new Plot(plotter.GetPointSets(function, new FunctionRange(from, to)),
                    plotter.PlotterInfo.DrawType, plotter.PlotterInfo.Color)).ToList();
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
    public ActionResult<List<PlotterInfo>> PlottersInfo() => plotters.Select(plotter => plotter.PlotterInfo).ToList();
}