using Microsoft.AspNetCore.Mvc;

namespace Funcan.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}