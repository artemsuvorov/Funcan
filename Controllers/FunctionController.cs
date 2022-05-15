using Funcan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Funcan.Controllers;

[ApiController]
[Route("[controller]")]
public class FunctionController
{
    private readonly ILogger<FunctionController> logger;
    private readonly FunctionService functionService;

    public FunctionController(ILogger<FunctionController> logger, FunctionService functionService)
    {
        this.logger = logger;
        this.functionService = functionService;
    }

    [HttpGet]
    public string Index(string input) => functionService.ResolveInput(input);

    // [HttpGet]
    // public string Mock() => functionService.ResolveInput("Pow(x, 2)");
}