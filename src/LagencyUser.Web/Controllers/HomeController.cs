using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LagencyUser.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LagencyUser.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Account
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
    