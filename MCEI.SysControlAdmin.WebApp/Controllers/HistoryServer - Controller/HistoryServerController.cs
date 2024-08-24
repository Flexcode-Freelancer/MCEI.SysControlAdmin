#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.HistoryServer___BL;
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.EN.HistoryServer___EN;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.HistoryServer___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador")]
    public class HistoryServerController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        HistoryServerBL historyServerBL = new HistoryServerBL();
        MembershipBL membershipBL = new MembershipBL();
        PrivilegeBL privilegeBL = new PrivilegeBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Index(HistoryServer historyServer = null!)
        {
            if (historyServer == null)
                historyServer = new HistoryServer();

            var historyServers = await historyServerBL.SearchIncludeAsync(historyServer);
            var membership = await membershipBL.GetAllAsync();
            var privilege = await privilegeBL.GetAllAsync();

            ViewBag.Memberships = membership;
            ViewBag.Privileges = privilege;
            return View(historyServers);
        }
        #endregion

        // Metod que extrae por Id y devolver a la vista en foramto Json
        [Authorize(Roles = "Desarrollador")]
        [HttpGet]
        public async Task<IActionResult> GetMembershipDetails(int id)
        {
            var member = await membershipBL.GetByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            var memberDetails = new
            {
                Dui = member.Dui,
                Birthdate = member.DateOfBirth.ToString("dd/MM/yyyy"), // Para que se muestre correctamente en el input date
                Age = member.Age,
                Gender = member.Gender,
                Phone = member.Phone,
                CivilStatus = member.CivilStatus,
                Profession = member.ProfessionOrStudy?.Name, // Verifica si esta es la clave correcta
                WaterBaptism = member.WaterBaptism,
                SpiritBaptism = member.BaptismOfTheHolySpirit,
                Photo = member.ImageData
            };
            return Json(memberDetails);
        }

        #region METODO PARA MOSTRAR DETALLES
        // Acción Que Muestra El Detalle De Un Registro
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var historyServer = await historyServerBL.GetByIdAsync(new HistoryServer { Id = id });
                if (historyServer == null)
                {
                    return NotFound();
                }

                // Obtén las entidades relacionadas Membership y Privilege
                historyServer.Membership = await membershipBL.GetByIdAsync(new Membership { Id = historyServer.IdMembership });
                historyServer.Privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = historyServer.IdPrivilege });

                // Comprueba si las entidades relacionadas existen
                if (historyServer.Membership == null || historyServer.Privilege == null)
                {
                    return NotFound();
                }
                return View(historyServer); // Retorna los detalles a la vista
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
