#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.HistoryServer___BL;
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.BL.Server___BL;
using MCEI.SysControlAdmin.EN.HistoryServer___EN;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using MCEI.SysControlAdmin.EN.Server___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using Rotativa.AspNetCore;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Server___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador")]
    public class ServerController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        ServerBL serverBL = new ServerBL();
        MembershipBL membershipBL = new MembershipBL();
        PrivilegeBL privilegeBL = new PrivilegeBL();
        HistoryServerBL historyServerBL = new HistoryServerBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador")]
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

        #region METODO PARA CREAR
        // Accion Para Mostrar La Vista De Crear
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Membership = await membershipBL.GetAllAsync();
            ViewBag.Privilege = await privilegeBL.GetAllAsync();
            ViewBag.Error = "";
            return View();
        }

        [Authorize(Roles = "Desarrollador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Server server)
        {
            try
            {
                server.DateCreated = DateTime.Now;
                server.DateModification = DateTime.Now;

                // Guarda en la tabla Server
                int result = await serverBL.CreateAsync(server);

                // Crear un nuevo objeto de tipo HistoryServer y mapear las propiedades de Server
                var historyServer = new HistoryServer
                {
                    IdMembership = server.IdMembership,
                    IdPrivilege = server.IdPrivilege,
                    Status = server.Status,
                    DateCreated = server.DateCreated,
                    DateModification = server.DateModification,
                };

                // Guarda en la tabla HistoryServer
                int resultHistory = await historyServerBL.CreateAsync(historyServer);

                TempData["SuccessMessageCreate"] = "Servidor Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Membership = await membershipBL.GetAllAsync();
                ViewBag.Privilege = await privilegeBL.GetAllAsync();
                return View(server);
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
                Server server = await serverBL.GetByIdAsync(new Server { Id = id });
                if (server == null)
                {
                    return NotFound();
                }
                ViewBag.Membership = await membershipBL.GetAllAsync();
                ViewBag.Privilege = await privilegeBL.GetAllAsync();
                return View(server);
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
        public async Task<IActionResult> Edit(int id, Server server)
        {
            try
            {
                if (id != server.Id)
                {
                    return BadRequest();
                }
                server.DateModification = DateTime.Now;
                int result = await serverBL.UpdateAsync(server);

                // Crear un nuevo objeto de tipo HistoryServer y mapear las propiedades de Server
                var historyServer = new HistoryServer
                {
                    IdMembership = server.IdMembership,
                    IdPrivilege = server.IdPrivilege,
                    Status = server.Status,
                    DateCreated = server.DateCreated,
                    DateModification = server.DateModification,
                };

                // Guarda en la tabla HistoryServer
                int resultHistory = await historyServerBL.CreateAsync(historyServer);

                TempData["SuccessMessageUpdate"] = "Servidor Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Membership = await membershipBL.GetAllAsync();
                ViewBag.Privilege = await privilegeBL.GetAllAsync();
                return View(server);
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
                // Obtiene el curso por su ID incluyendo las entidades relacionadas Trainer y Schedule
                var server = await serverBL.GetByIdAsync(new Server { Id = id });
                if (server == null)
                {
                    return NotFound();
                }

                // Obtén las entidades relacionadas Membership y Privilege
                server.Membership = await membershipBL.GetByIdAsync(new Membership { Id = server.IdMembership });
                server.Privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = server.IdPrivilege });

                // Comprueba si las entidades relacionadas existen
                if (server.Membership == null || server.Privilege == null)
                {
                    return NotFound();
                }
                return View(server); // Retorna los detalles a la vista
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
                Server server = await serverBL.GetByIdAsync(new Server { Id = id });
                if (server == null)
                {
                    return NotFound();
                }
                // Obtén las entidades relacionadas Membership y Privilege
                server.Membership = await membershipBL.GetByIdAsync(new Membership { Id = server.IdMembership });
                server.Privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = server.IdPrivilege });

                // Comprueba si las entidades relacionadas existen
                if (server.Membership == null || server.Privilege == null)
                {
                    return NotFound();
                }
                return View(server); // Retorna los detalles a la vista
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
        public async Task<IActionResult> Delete(int id, Server server)
        {
            try
            {
                Server serverDB = await serverBL.GetByIdAsync(server);
                int result = await serverBL.DeleteAsync(serverDB);
                TempData["SuccessMessageDelete"] = "Servidor Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var serverDB = await serverBL.GetByIdAsync(server);
                if (serverDB == null)
                    serverDB = new Server();
                if (serverDB.Id > 0)
                    serverDB.Membership = await membershipBL.GetByIdAsync(new Membership { Id = serverDB.IdMembership });
                serverDB.Privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = serverDB.IdPrivilege });
                return View(serverDB);
            }
        }
        #endregion

        #region METODO PARA REPORTE
        // Metodo Para Generar Ficha o Reporte En PDF
        public async Task<ActionResult> GeneratePDFfile(int id)
        {
            var generatePDF = await serverBL.GetByIdAsync(new Server { Id = id });
            string fileName = $"Ficha_Servidor_{generatePDF.Membership!.Name}_{generatePDF.Membership.LastName}_MCEI.pdf";
            return new ViewAsPdf("GeneratePDFfile", generatePDF)
            {
                FileName = fileName,
            };
        }
        #endregion
    }
}
