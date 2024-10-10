#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.HistoryServer___BL;
using MCEI.SysControlAdmin.BL.HistoryServerJei___BL;
using MCEI.SysControlAdmin.BL.Juventud___BL;
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.DAL.HistoryServer___DAL;
using MCEI.SysControlAdmin.EN.HistoryServer___EN;
using MCEI.SysControlAdmin.EN.HistoryServerJei___EN;
using MCEI.SysControlAdmin.EN.Juventud___EN;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.HistoryServerJei___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Directivo Juvenil")]
    public class HistoryServerJeiController : Controller
    {
        // Creamos La Instancias Para Acceder a Los Metodos
        HistoryServerJeiBL historyServerJeiBL = new HistoryServerJeiBL();
        JuventudBL juventudBL = new JuventudBL();
        PrivilegeBL privilegeBL = new PrivilegeBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador, Directivo Juvenil")]
        public async Task<IActionResult> Index(HistoryServerJei historyServerJei = null!)
        {
            if (historyServerJei == null)
                historyServerJei = new HistoryServerJei();

            var historyServersJei = await historyServerJeiBL.SearchIncludeAsync(historyServerJei);
            var juventud = await juventudBL.GetAllAsync();
            var privilege = await privilegeBL.GetAllAsync();

            ViewBag.Juventud = juventud;
            ViewBag.Privileges = privilege;
            return View(historyServersJei);
        }
        #endregion

        // Metod que extrae por Id y devolver a la vista en foramto Json
        [Authorize(Roles = "Desarrollador, Directivo Juvenil")]
        [HttpGet]
        public async Task<IActionResult> GetJuventudDetails(int id)
        {
            var juventud = await juventudBL.GetByIdAsync(id);
            if (juventud == null)
            {
                return NotFound();
            }

            var juventudDetails = new
            {
                Birthdate = juventud.DateOfBirth.ToString("dd/MM/yyyy"), // Para que se muestre correctamente en el input date
                Age = juventud.Age,
                Gender = juventud.Gender,
                Phone = juventud.Phone,
                CivilStatus = juventud.CivilStatus,
                Profession = juventud.ProfessionOrStudy?.Name, // Verifica si esta es la clave correcta
                WaterBaptism = juventud.WaterBaptism,
                SpiritBaptism = juventud.BaptismOfTheHolySpirit,
                Photo = juventud.ImageData
            };
            return Json(juventudDetails);
        }

        #region METODO PARA MOSTRAR DETALLES
        // Acción Que Muestra El Detalle De Un Registro
        [Authorize(Roles = "Desarrollador, Directivo Juvenil")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var historyServerJei = await historyServerJeiBL.GetByIdAsync(new HistoryServerJei { Id = id });
                if (historyServerJei == null)
                {
                    return NotFound();
                }

                // Obtén las entidades relacionadas Membership y Privilege
                historyServerJei.Juventud = await juventudBL.GetByIdAsync(new Juventud { Id = historyServerJei.IdJuventud });
                historyServerJei.Privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = historyServerJei.IdPrivilege });

                // Comprueba si las entidades relacionadas existen
                if (historyServerJei.Juventud == null || historyServerJei.Privilege == null)
                {
                    return NotFound();
                }
                return View(historyServerJei); // Retorna los detalles a la vista
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devuelve la vista sin ningún objeto Course
            }
        }
        #endregion
    }
}
