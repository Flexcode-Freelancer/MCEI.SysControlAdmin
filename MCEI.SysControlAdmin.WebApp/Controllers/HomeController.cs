using MCEI.SysControlAdmin.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MCEI.SysControlAdmin.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
