using MCEI.SysControlAdmin.WebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MCEI.SysControlAdmin.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        [Authorize(Roles = "Desarrollador")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Desarrollador")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
