using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class InicioController : Controller
    {
        [HttpGet]
        [Route("/Inicio")]
        public IActionResult Index()
        {
            return View("Inicio");
        }
        public IActionResult Lorem()
        {
            // Redireciona para a ação "About" no mesmo controlador
            return RedirectToAction("EditarHtml");
        }

        public IActionResult EditarHtml()
        {
            return View("EditarHtml");
        }
    }
}
