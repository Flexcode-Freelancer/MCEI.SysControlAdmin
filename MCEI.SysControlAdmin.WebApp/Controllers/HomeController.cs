using MCEI.SysControlAdmin.WebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MCEI.SysControlAdmin.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador, Directivo Juvenil")]
    public class HomeController : Controller
    {
        [Authorize(Roles = "Desarrollador, Administrador, Digitador, Directivo Juvenil")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Desarrollador, Administrador")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
