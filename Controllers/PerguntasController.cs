using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Controllers
{
    [Authorize]
    public class PerguntasController : Controller
    {
        [HttpGet]
        [Route("/Editar")]
        public IActionResult Index()
        {
            return View("Editar");
        }
    }
}
