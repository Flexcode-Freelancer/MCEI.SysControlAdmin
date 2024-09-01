#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.HistoryServer___BL;
using MCEI.SysControlAdmin.BL.HistoryServerJei___BL;
using MCEI.SysControlAdmin.BL.Juventud___BL;
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.BL.Server___BL;
using MCEI.SysControlAdmin.BL.ServerJei___BL;
using MCEI.SysControlAdmin.DAL.HistoryServer___DAL;
using MCEI.SysControlAdmin.DAL.Membership___DAL;
using MCEI.SysControlAdmin.DAL.Server___DAL;
using MCEI.SysControlAdmin.EN.HistoryServer___EN;
using MCEI.SysControlAdmin.EN.HistoryServerJei___EN;
using MCEI.SysControlAdmin.EN.Juventud___EN;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using MCEI.SysControlAdmin.EN.Server___EN;
using MCEI.SysControlAdmin.EN.ServerJei___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.ServerJei___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador")]
    public class ServerJeiController : Controller
    {
        // Cramos La Instancia Para Accerder a Los Metodos
        ServerJeiBL serverJeiBL = new ServerJeiBL();
        JuventudBL juventudBL = new JuventudBL();
        PrivilegeBL privilegeBL = new PrivilegeBL();
        HistoryServerJeiBL historyServerJeiBL = new HistoryServerJeiBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Index(ServerJei serverJei = null!)
        {
            if (serverJei == null)
                serverJei = new ServerJei();

            var serversJei = await serverJeiBL.SearchIncludeAsync(serverJei);
            var juventud = await juventudBL.GetAllAsync();
            var privilege = await privilegeBL.GetAllAsync();

            ViewBag.Juventud = juventud;
            ViewBag.Privileges = privilege;
            return View(serversJei);
        }
        #endregion

        // Metod que extrae por Id y devolver a la vista en foramto Json
        [Authorize(Roles = "Desarrollador")]
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

        #region METODO PARA CREAR
        // Accion Para Mostrar La Vista De Crear
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Juventud = await juventudBL.GetAllAsync();
            ViewBag.Privilege = await privilegeBL.GetAllAsync();
            ViewBag.Error = "";
            return View();
        }

        [Authorize(Roles = "Desarrollador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServerJei serverJei)
        {
            try
            {
                serverJei.DateCreated = DateTime.Now;
                serverJei.DateModification = DateTime.Now;

                // Guarda en la tabla Server
                int result = await serverJeiBL.CreateAsync(serverJei);

                // Crear un nuevo objeto de tipo HistoryServer y mapear las propiedades de Server
                var historyServerJei = new HistoryServerJei
                {
                    IdJuventud = serverJei.IdJuventud,
                    IdPrivilege = serverJei.IdPrivilege,
                    Status = serverJei.Status,
                    DateCreated = serverJei.DateCreated,
                    DateModification = serverJei.DateModification,
                };

                // Guarda en la tabla HistoryServer
                int resultHistoryJei = await historyServerJeiBL.CreateAsync(historyServerJei);

                TempData["SuccessMessageCreate"] = "Servidor Juvenil Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Juventud = await juventudBL.GetAllAsync();
                ViewBag.Privilege = await privilegeBL.GetAllAsync();
                return View(serverJei);
            }
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Acción que muestra la vista de modificar
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ServerJei serverJei = await serverJeiBL.GetByIdAsync(new ServerJei { Id = id });
                if (serverJei == null)
                {
                    return NotFound();
                }
                ViewBag.Juventud = await juventudBL.GetAllAsync();
                ViewBag.Privilege = await privilegeBL.GetAllAsync();
                return View(serverJei);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // Acción que recibe los datos del formulario para ser enviados a la base de datos
        [Authorize(Roles = "Desarrollador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServerJei serverJei)
        {
            try
            {
                if (id != serverJei.Id)
                {
                    return BadRequest();
                }
                serverJei.DateModification = DateTime.Now;
                int result = await serverJeiBL.UpdateAsync(serverJei);

                // Crear un nuevo objeto de tipo HistoryServer y mapear las propiedades de Server
                var historyServerJei = new HistoryServerJei
                {
                    IdJuventud = serverJei.IdJuventud,
                    IdPrivilege = serverJei.IdPrivilege,
                    Status = serverJei.Status,
                    DateCreated = serverJei.DateCreated,
                    DateModification = serverJei.DateModification,
                };

                // Guarda en la tabla HistoryServer
                int resultHistoryJei = await historyServerJeiBL.CreateAsync(historyServerJei);

                TempData["SuccessMessageUpdate"] = "Servidor Juvenil Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Juventud = await juventudBL.GetAllAsync();
                ViewBag.Privilege = await privilegeBL.GetAllAsync();
                return View(serverJei);
            }
        }
        #endregion

        #region METODO PARA MOSTRAR DETALLES
        // Acción Que Muestra El Detalle De Un Registro
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var serverJei = await serverJeiBL.GetByIdAsync(new ServerJei { Id = id });
                if (serverJei == null)
                {
                    return NotFound();
                }

                // Obtén las entidades relacionadas Membership y Privilege
                serverJei.Juventud = await juventudBL.GetByIdAsync(new Juventud { Id = serverJei.IdJuventud });
                serverJei.Privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = serverJei.IdPrivilege });

                // Comprueba si las entidades relacionadas existen
                if (serverJei.Juventud == null || serverJei.Privilege == null)
                {
                    return NotFound();
                }
                return View(serverJei); // Retorna los detalles a la vista
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devuelve la vista sin ningún objeto Course
            }
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Accion Que Muestra La Vista De Eliminar
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ServerJei serverJei = await serverJeiBL.GetByIdAsync(new ServerJei { Id = id });
                if (serverJei == null)
                {
                    return NotFound();
                }
                // Obtén las entidades relacionadas Membership y Privilege
                serverJei.Juventud = await juventudBL.GetByIdAsync(new Juventud { Id = serverJei.IdJuventud });
                serverJei.Privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = serverJei.IdPrivilege });

                // Comprueba si las entidades relacionadas existen
                if (serverJei.Juventud == null || serverJei.Privilege == null)
                {
                    return NotFound();
                }
                return View(serverJei); // Retorna los detalles a la vista
            }
            catch (Exception ex)
            {
                ViewBag.Errors = ex.Message;
                return View();
            }
        }

        // Accion Que Recibe Los Datos Del Formulario Para Ser Enviados a La BD
        [Authorize(Roles = "Desarrollador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, ServerJei serverJei)
        {
            try
            {
                ServerJei serverJeiDB = await serverJeiBL.GetByIdAsync(serverJei);
                int result = await serverJeiBL.DeleteAsync(serverJeiDB);
                TempData["SuccessMessageDelete"] = "Servidor Juvenil Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var serverJeiDB = await serverJeiBL.GetByIdAsync(serverJei);
                if (serverJeiDB == null)
                    serverJeiDB = new ServerJei();
                if (serverJeiDB.Id > 0)
                    serverJeiDB.Juventud = await juventudBL.GetByIdAsync(new Juventud { Id = serverJeiDB.IdJuventud });
                serverJeiDB.Privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = serverJeiDB.IdPrivilege });
                return View(serverJeiDB);
            }
        }
        #endregion
    }
}
