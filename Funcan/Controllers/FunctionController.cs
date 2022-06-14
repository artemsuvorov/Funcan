using System.Collections.Generic;
using Funcan.Domain.Models;
using Funcan.Service;
using Microsoft.AspNetCore.Mvc;

namespace Funcan.Controllers;

[ApiController]
[Route("[controller]")]
public class FunctionController : Controller
{
    private IPlotterService PlotterService { get; }

    public FunctionController(IPlotterService plotterService) =>
        PlotterService = plotterService;

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
        // TODO: validate function
        var function = new MathFunction(inputFunction);
        if (true)
        {
            var plots = PlotterService.GetPlots(function, new FunctionRange(from, to), analysisOptions);
            return plots;
        }
    }

    [HttpGet]
    [Route("Plotters")]
    [ProducesResponseType(200, Type = typeof(List<PlotterInfo>))]
    public ActionResult<List<PlotterInfo>> GetAnalysisOptions() => PlotterService.GetPlotterInfos();
}