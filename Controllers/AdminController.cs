using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
