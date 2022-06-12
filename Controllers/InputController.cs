using Funcan.Domain.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Funcan.Controllers;

[ApiController]
[Route("[controller]")]
public class InputController : Controller
{
    private readonly ISessionManager sessionManager;

    public InputController(ISessionManager sessionManager)
    {
        this.sessionManager = sessionManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (!sessionManager.ContainsSessionId(HttpContext))
            sessionManager.StartSession(HttpContext);
        return View();
    }
}