using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        [Route("/Login")]
        public IActionResult Index()
        {
            return View("Login");
        }
    }
}
