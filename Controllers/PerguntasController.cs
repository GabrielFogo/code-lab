using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Controllers
{
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
