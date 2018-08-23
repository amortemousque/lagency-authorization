using Microsoft.AspNetCore.Mvc;

namespace LagencyUser.Web.Controllers
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