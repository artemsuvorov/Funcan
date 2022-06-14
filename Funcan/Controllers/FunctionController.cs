using System.Collections.Generic;
using System.Linq;
using Funcan.Domain.Models;
using Funcan.Domain.Repository;
using Funcan.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace Funcan.Controllers;

[ApiController]
[Route("[controller]")]
public class FunctionController : Controller
{
    private IPlotterService PlotterService { get; }
    private IHistoryRepository HistoryRepository { get; }

    public FunctionController(IPlotterService plotterService, IHistoryRepository historyRepository)
    {
        PlotterService = plotterService;
        HistoryRepository = historyRepository;
    }

    [HttpPost]
    [Route("")]
    [ProducesResponseType(200, Type = typeof(List<Plot>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<Plot>> GetFunction(
        [FromQuery(Name = "input")] string inputFunction,
        [FromBody] IEnumerable<string> necessaryPlotters,
        [FromQuery(Name = "from")] double from = -10,
        [FromQuery(Name = "to")] double to = 10
    )
    {
        // TODO: validate function
        var function = new MathFunction(inputFunction);
        if (true)
        {
            var plotters = necessaryPlotters.ToList();
            var plots = PlotterService.GetPlots(function, new FunctionRange(from, to), plotters);
            var userId = HttpContext.Request.Cookies["user_id"];
            if (userId is not null && int.TryParse(userId, out var id))
            {
                HistoryRepository
                    .Save(id, new HistoryEntry(inputFunction, from, to, plotters));
            }

            return plots;
        }
    }

    [HttpGet]
    [Route("Plotters")]
    [ProducesResponseType(200, Type = typeof(List<PlotterInfo>))]
    public ActionResult<List<PlotterInfo>> GetAnalysisOptions() => PlotterService.GetPlotterInfos();

    [HttpGet]
    [Route("History")]
    [ProducesResponseType(200, Type = typeof(List<HistoryEntry>))]
    public ActionResult<List<HistoryEntry>> GetHistory()
    {
        var userId = HttpContext.Request.Cookies["user_id"];
        if (userId is not null && int.TryParse(userId, out var id))
            return HistoryRepository.Get(id);
        return new List<HistoryEntry>();
    }
}