#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Privilege___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class PrivilegeController : Controller
    {
        // Creamos Una Instancia Para Acceder a Los Metodos
        PrivilegeBL privilegeBL = new PrivilegeBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Index(Privilege privilege = null!)
        {
            if (privilege == null)
                privilege = new Privilege();

            var privileges = await privilegeBL.SearchAsync(privilege);
            return View(privileges);
        }
        #endregion

        #region METODO PARA CREAR
        // Accion Para Mostrar La Vista De Crear
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public ActionResult Create()
        {
            ViewBag.Error = "";
            return View();
        }

        // Accion Que Recibe Los Datos Del Formulario Para Ser Enviados a La BD
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Privilege privilege)
        {
            try
            {
                int result = await privilegeBL.CreateAsync(privilege);
                TempData["SuccessMessageCreate"] = "Privilegio Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Accion Que Muestra La Vista De Modificar
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Edit(int id)
        {
            var privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = id });
            ViewBag.Error = "";
            return View(privilege);
        }

        // Accion Que Recibe Los Datos Del Formulario Para Ser Enviados a La BD
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Privilege privilege)
        {
            try
            {
                int result = await privilegeBL.UpdateAsync(privilege);
                TempData["SuccessMessageUpdate"] = "Privilegio Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(privilege);
            }
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Accion Que Muestra La Vista De Eliminar
        [Authorize(Roles = "Desarrollador, Administrador Digitador")]
        public async Task<IActionResult> Delete(int id)
        {
            var privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = id });
            ViewBag.Error = "";
            return View(privilege);
        }

        // Accion Que Recibe Los Datos Del Formulario Para Ser Enviados a La BD
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Privilege privilege)
        {
            try
            {
                int result = await privilegeBL.DeleteAsync(privilege);
                TempData["SuccessMessageDelete"] = "Privilegio Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(privilege);
            }
        }
        #endregion

        #region METODO PARA MOSTRAR DETALLES
        // Accion Que Muestra El Detalle De Un Registro
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Details(int id)
        {
            var privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = id }); //Convertimos para mostrar muy bien
            return View(privilege); // Retornamos los Detalles a La Vista
        }
        #endregion
    }
}
