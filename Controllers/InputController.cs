using Microsoft.AspNetCore.Mvc;

namespace Funcan.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}