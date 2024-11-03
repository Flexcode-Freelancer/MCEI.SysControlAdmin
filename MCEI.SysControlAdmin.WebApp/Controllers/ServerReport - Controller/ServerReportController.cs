#region REFERENCIAS
// Referencia Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.HistoryServer___BL;
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.BL.Server___BL;
using MCEI.SysControlAdmin.EN.Server___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.ServerReport___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador")]
    public class ServerReportController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        HistoryServerBL historyServerBL = new HistoryServerBL();
        MembershipBL membershipBL = new MembershipBL();
        PrivilegeBL privilegeBL = new PrivilegeBL();
        ServerBL serverBL = new ServerBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<IActionResult> Index(Server server = null!)
        {
            if (server == null)
                server = new Server();

            var servers = await serverBL.SearchIncludeAsync(server);
            var membership = await membershipBL.GetAllAsync();
            var privilege = await privilegeBL.GetAllAsync();

            ViewBag.Memberships = membership;
            ViewBag.Privileges = privilege;
            return View(servers);
        }
        #endregion

        #region METODO PARA REPORTE POR DUI
        // Metodo Para Generar Reporte En PDF Basado En El DUI
        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<ActionResult> GeneratePDFfileByDUI(string dui)
        {
            var historyServerList = await historyServerBL.GetByDUIAsync(dui);
            string fileName = $"ReporteHistorialServidor_{dui}.pdf";

            return new ViewAsPdf("GeneratePDFfileByDUI", historyServerList)
            {
                FileName = fileName,
            };
        }
        #endregion
    }
}
