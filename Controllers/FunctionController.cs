using System;
using System.Collections.Generic;
using System.Linq;
using Funcan.Application.Plotters;
using Funcan.Domain;
using Funcan.Solvers;
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

    public FunctionController(
        IFunctionParser functionParser,
        ILogger<FunctionController> logger,
        FunctionPlotter functionPlotter
    )
    {
        this.functionParser = functionParser;
        this.logger = logger;
        this.functionPlotter = functionPlotter;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<PointSet>))]
    public ActionResult<List<PointSet>> Index(
        [FromQuery(Name = "input")] string inputFunction,
        [FromQuery(Name = "from")] double from = -10,
        [FromQuery(Name = "to")] double to = 10
    )
    {
        try
        {
            var function = functionParser.Parse(inputFunction);
            return functionPlotter.GetPointSets(function, new Range(from, to)).ToList();
        }
        catch (ArgumentException)
        {
            return new BadRequestResult();
        }
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<PointSet>))]
    [Route("break-points")]
    public ActionResult<List<PointSet>> BreakPoints() => null;
}