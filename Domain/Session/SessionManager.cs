using Microsoft.AspNetCore.Http;
using System;

namespace Funcan.Domain.Session
{
    public class CookieSessionManager : ISessionManager
    {
        private int sessionId = 0;

        public void StartSession(HttpContext httpContext)
        {
            var options = new CookieOptions()
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
            return int.Parse(userId);
        }
    }
}