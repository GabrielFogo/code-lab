using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Controllers
{
    [Authorize (Policy = "AdminOnly")]
    public class PerguntasController : Controller
    {
        [HttpGet]
        [Route("/EditarHtml")]
        public IActionResult Index()
        {
            return View("EditarHtml");
        }

        public IActionResult Lorem()
        {
            return RedirectToAction("Inicio");
        }

        public IActionResult Inicio()
        {
            return View("Inicio");
        }
    }
}
