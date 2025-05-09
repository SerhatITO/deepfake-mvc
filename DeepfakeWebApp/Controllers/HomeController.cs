using Microsoft.AspNetCore.Mvc;

namespace DeepfakeWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
