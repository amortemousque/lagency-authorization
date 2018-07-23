using Microsoft.AspNetCore.Mvc;

namespace IdentityServerWithAspNetIdentity.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}