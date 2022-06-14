using System;
using Microsoft.AspNetCore.Http;

namespace Funcan.Controllers.Session;

public class CookieSessionManager : ISessionManager
{
    private int sessionId;

    public void StartSession(HttpContext httpContext)
    {
        var options = new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddHours(1)
        };
        var userId = sessionId++;
        httpContext.Response.Cookies.Append("user_id", userId.ToString(), options);
    }

    public bool ContainsSessionId(HttpContext httpContext)
    {
        return httpContext.Request.Cookies.ContainsKey("user_id");
    }

    public int GetSessionId(HttpContext httpContext)
    {
        var userId = httpContext.Request.Cookies["user_id"];
        return int.Parse(userId ?? throw new InvalidOperationException());
    }
}