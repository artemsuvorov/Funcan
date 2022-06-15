using Microsoft.AspNetCore.Http;

namespace Funcan.Controllers.Session;

public interface ISessionManager {
    int GetSessionId(HttpContext httpContext);
    bool ContainsSessionId(HttpContext httpContext);
    void StartSession(HttpContext httpContext);
}