using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Funcan.Application.Plotters;
using Funcan.Domain;
using Funcan.Solvers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Range = Funcan.Domain.Range;

namespace Funcan.Controllers;

[ApiController]
[Route("[controller]")]
public class FunctionController
{
    private readonly IFunctionParser functionParser;
    private readonly ILogger<FunctionController> logger;
    private readonly FunctionPlotter functionPlotter;
    private readonly ExtremaPlotter extremaPlotter;
    private readonly InflectionPointsPlotter inflectionPointsPlotter;
    private readonly DiscontinuitiesPlotter discontinuitiesPlotter;

    // public FunctionController(
    //     IFunctionParser functionParser,
    //     ILogger<FunctionController> logger,
    //     FunctionPlotter functionPlotter,
    // )
    // {
    //     this.functionParser = functionParser;
    //     this.logger = logger;
    //     this.functionPlotter = functionPlotter;
    // }

    public FunctionController(IFunctionParser functionParser, ILogger<FunctionController> logger, FunctionPlotter functionPlotter, ExtremaPlotter extremaPlotter, InflectionPointsPlotter inflectionPointsPlotter, DiscontinuitiesPlotter discontinuitiesPlotter)
    {
        this.functionParser = functionParser;
        this.logger = logger;
        this.functionPlotter = functionPlotter;
        this.extremaPlotter = extremaPlotter;
        this.inflectionPointsPlotter = inflectionPointsPlotter;
        this.discontinuitiesPlotter = discontinuitiesPlotter;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(200, Type = typeof(List<PointSet>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<PointSet>> GetFunction(
        [FromQuery(Name = "input")] string inputFunction,
        [FromQuery(Name = "from")] double from = -10,
        [FromQuery(Name = "to")] double to = 10
    )
    {
        try
        {
            var function = functionParser.Parse(inputFunction);
            var points = functionPlotter.GetPointSets(function, new Range(from, to));
            return new Splitter().Split(points, function).ToList();
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
    [Route("break-points")]
    [ProducesResponseType(200, Type = typeof(List<PointSet>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<PointSet>> GetBreakPoints() => null;

    [HttpGet]
    [Route("extremes")]
    [ProducesResponseType(200, Type = typeof(List<PointSet>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<PointSet>> GetExtremes(
        [FromQuery(Name = "input")] string inputFunction,
        [FromQuery(Name = "from")] double from = -10,
        [FromQuery(Name = "to")] double to = 10
    )  {
        try
        {
            var function = functionParser.Parse(inputFunction);
            var points = extremaPlotter.GetPointSets(function, new Range(from, to));
            return new Splitter().Split(points, function).ToList(); // functionPlotter.Huita();
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
    [Route("asymptotes")]
    [ProducesResponseType(200, Type = typeof(List<PointSet>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<PointSet>> GetAsymptotes() => null;

    [HttpGet]
    [Route("monotone")]
    [ProducesResponseType(200, Type = typeof(List<PointSet>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<PointSet>> GetMonotone() => null;

    [HttpGet]
    [Route("inflection-points")]
    [ProducesResponseType(200, Type = typeof(List<PointSet>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public ActionResult<List<PointSet>> GetInflectionPoints() => null;
}