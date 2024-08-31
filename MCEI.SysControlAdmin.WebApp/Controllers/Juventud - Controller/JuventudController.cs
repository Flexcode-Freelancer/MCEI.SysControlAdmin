#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Juventud___BL;
using MCEI.SysControlAdmin.BL.ProfessionOrStudy___BL;
using MCEI.SysControlAdmin.EN.Juventud___EN;
using MCEI.SysControlAdmin.EN.ProfessionOrStudy___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Juventud___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador")]
    public class JuventudController : Controller
    {
        // Creamos Una Instancia Para Acceder a Los Metodos
        JuventudBL juventudBL = new JuventudBL();
        ProfessionOrStudyBL professionOrStudyBL = new ProfessionOrStudyBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Index(Juventud juventud = null!)
        {
            if (juventud == null)
                juventud = new Juventud();

            var juventuds = await juventudBL.SearchIncludeProfessionOrStudyAsync(juventud);
            var professionOrStudies = await professionOrStudyBL.GetAllAsync();

            ViewBag.ProfessionOrStudies = professionOrStudies;
            return View(juventuds);
        }
        #endregion

        #region METODO PARA CREAR
        // Accion Para Mostrar La Vista De Crear
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Create()
        {
            ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
            ViewBag.Error = "";
            return View();
        }

        // Accion Que Recibe Los Datos Del Formulario Para Ser Enviados a La BD
        [Authorize(Roles = "Desarrollador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Juventud juventud, IFormFile imagen)
        {
            try
            {
                // Mapeo de img a ArrayByte
                if (imagen != null && imagen.Length > 0)
                {
                    byte[] imagenData = null!;
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        imagenData = memoryStream.ToArray();
                    }

                    juventud.ImageData = imagenData; // Asigna el array de bytes al campo de la imagen en tu modelo Membership
                }
                juventud.DateCreated = DateTime.Now;
                juventud.DateModification = DateTime.Now;
                int result = await juventudBL.CreateAsync(juventud);
                TempData["SuccessMessageCreate"] = "Joven Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
                return View(juventud);
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
                Juventud juventud = await juventudBL.GetByIdAsync(new Juventud { Id = id });
                if (juventud == null)
                {
                    return NotFound();
                }
                // Convertir el array de bytes en imagen para mostrar en la vista
                if (juventud.ImageData != null && juventud.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(juventud.ImageData);
                }
                ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
                return View(juventud);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto Membership
            }
        }

        // Acción que recibe los datos del formulario para ser enviados a la base de datos
        [Authorize(Roles = "Desarrollador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Juventud juventud, IFormFile imagen)
        {
            try
            {
                if (id != juventud.Id)
                {
                    return BadRequest();
                }
                if (imagen != null && imagen.Length > 0) // Verificar si se ha subido una nueva imagen
                {
                    byte[] imagenData = null!;
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        imagenData = memoryStream.ToArray();
                    }
                    juventud.ImageData = imagenData; // Asignar el array de bytes de la nueva imagen a la entidad Membership
                }
                else
                {
                    // Si no se proporciona una nueva imagen, se conserva la imagen existente
                    Juventud existingJuventud = await juventudBL.GetByIdAsync(new Juventud { Id = id });
                    juventud.ImageData = existingJuventud.ImageData;
                }
                juventud.DateModification = DateTime.Now;
                int result = await juventudBL.UpdateAsync(juventud);
                TempData["SuccessMessageUpdate"] = "Joven Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
                return View(juventud); // Devolver la vista con el objeto Membership para que el usuario pueda corregir los datos
            }
        }
        #endregion

        #region METODO PARA MOSTRAR DETALLES
        // Accion Que Muestra El Detalle De Un Registro
        [Authorize(Roles = "Desarrollador")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                Juventud juventud = await juventudBL.GetByIdAsync(new Juventud { Id = id });
                if (juventud == null)
                {
                    return NotFound();
                }
                // Convertir el array de bytes en imagen para mostrar en la vista
                if (juventud.ImageData != null && juventud.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(juventud.ImageData);
                }
                juventud.ProfessionOrStudy = await professionOrStudyBL.GetByIdAsync(new ProfessionOrStudy { Id = juventud.IdProfessionOrStudy });
                return View(juventud); // Retornamos los Detalles a La Vista
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto Membership
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
                Juventud juventud = await juventudBL.GetByIdAsync(new Juventud { Id = id });
                if (juventud == null)
                {
                    return NotFound();
                }
                // Convertir el array de bytes en imagen para mostrar en la vista
                if (juventud.ImageData != null && juventud.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(juventud.ImageData);
                }
                ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
                return View(juventud);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto Membership
            }
        }

        // Accion Que Recibe Los Datos Del Formulario Para Ser Enviados a La BD
        [Authorize(Roles = "Desarrollador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Juventud juventud)
        {
            try
            {
                Juventud juventudDB = await juventudBL.GetByIdAsync(juventud);
                int result = await juventudBL.DeleteAsync(juventudDB);
                TempData["SuccessMessageDelete"] = "Joven Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var juventudBD = await juventudBL.GetByIdAsync(juventud);
                if (juventudBD == null)
                    juventudBD = new Juventud();
                if (juventudBD.Id > 0)
                    juventudBD.ProfessionOrStudy = await professionOrStudyBL.GetByIdAsync(new ProfessionOrStudy { Id = juventudBD.IdProfessionOrStudy });
                return View(juventudBD);
            }
        }
        #endregion
    }
}
