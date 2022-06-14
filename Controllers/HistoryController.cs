using Funcan.Domain.Models;
using Funcan.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Funcan.Controllers.Session;

namespace Funcan.Controllers
{
    [Route("[controller]")]
    public class HistoryController : Controller
    {
        private readonly ISessionManager sessionManager;
        private readonly IHistory history;

        public HistoryController(ISessionManager sessionManager, IHistory history)
        {
            this.sessionManager = sessionManager;
            this.history = history;
        }

        [HttpPut]
        [Route("Add")]
        public ActionResult AddHistoryEntry(
            [FromQuery(Name = "input")] string inputFunction,
            [FromBody] IEnumerable<PlotterInfo> analysisOptions,
            [FromQuery(Name = "from")] double from = -10,
            [FromQuery(Name = "to")] double to = 10
        )
        {
            if (!sessionManager.ContainsSessionId(HttpContext))
                return BadRequest();

            var id = sessionManager.GetSessionId(HttpContext);
            history.Save(id, new HistoryEntry(inputFunction, from, to, analysisOptions.ToList()));

            return Ok();
        }

        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(200, Type = typeof(List<HistoryEntry>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public ActionResult<List<HistoryEntry>> GetAllHistory()
        {
            if (!sessionManager.ContainsSessionId(HttpContext))
                return BadRequest();

            var id = sessionManager.GetSessionId(HttpContext);
            return history.Get(id);
        }
    }
}