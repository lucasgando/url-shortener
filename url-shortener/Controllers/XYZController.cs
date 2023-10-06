using Microsoft.AspNetCore.Mvc;

namespace url_shortener.Controllers
{
    public class XYZController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
