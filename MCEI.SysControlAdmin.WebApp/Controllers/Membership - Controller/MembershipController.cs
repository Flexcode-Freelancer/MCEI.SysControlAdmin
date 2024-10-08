﻿#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.ProfessionOrStudy___BL;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.ProfessionOrStudy___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Membership___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class MembershipController : Controller
    {
        // Creamos Una Instancia Para Acceder a Los Metodos
        MembershipBL membershipBL = new MembershipBL();
        ProfessionOrStudyBL professionOrStudyBL = new ProfessionOrStudyBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Index(Membership membership = null!)
        {
            if (membership == null)
                membership = new Membership();

            var memberships = await membershipBL.SearchIncludeProfessionOrStudyAsync(membership);
            var professionOrStudies = await professionOrStudyBL.GetAllAsync();

            ViewBag.ProfessionOrStudies = professionOrStudies;
            return View(memberships);
        }
        #endregion

        #region METODO PARA CREAR
        // Accion Para Mostrar La Vista De Crear
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Create()
        {
            ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
            ViewBag.Error = "";
            return View();
        }

        // Accion Que Recibe Los Datos Del Formulario Para Ser Enviados a La BD
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Membership membership, IFormFile imagen)
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

                    membership.ImageData = imagenData; // Asigna el array de bytes al campo de la imagen en tu modelo Membership
                }
                membership.DateCreated = DateTime.Now;
                membership.DateModification = DateTime.Now;
                int result = await membershipBL.CreateAsync(membership);
                TempData["SuccessMessageCreate"] = "Miembro Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
                return View(membership);
            }
        }

        #endregion

        #region METODO PARA MODIFICAR
        // Acción que muestra la vista de modificar
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Membership membership = await membershipBL.GetByIdAsync(new Membership { Id = id });
                if (membership == null)
                {
                    return NotFound();
                }
                // Convertir el array de bytes en imagen para mostrar en la vista
                if (membership.ImageData != null && membership.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(membership.ImageData);
                }
                ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
                return View(membership);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto Membership
            }
        }

        // Acción que recibe los datos del formulario para ser enviados a la base de datos
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Membership membership, IFormFile imagen)
        {
            try
            {
                if (id != membership.Id)
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
                    membership.ImageData = imagenData; // Asignar el array de bytes de la nueva imagen a la entidad Membership
                }
                else
                {
                    // Si no se proporciona una nueva imagen, se conserva la imagen existente
                    Membership existingMembership = await membershipBL.GetByIdAsync(new Membership { Id = id });
                    membership.ImageData = existingMembership.ImageData;
                }
                membership.DateModification = DateTime.Now;
                int result = await membershipBL.UpdateAsync(membership);
                TempData["SuccessMessageUpdate"] = "Miembro Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
                return View(membership); // Devolver la vista con el objeto Membership para que el usuario pueda corregir los datos
            }
        }
        #endregion

        #region METODO PARA MOSTRAR DETALLES
        // Accion Que Muestra El Detalle De Un Registro
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                Membership membership = await membershipBL.GetByIdAsync(new Membership { Id = id });
                if (membership == null)
                {
                    return NotFound();
                }
                // Convertir el array de bytes en imagen para mostrar en la vista
                if (membership.ImageData != null && membership.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(membership.ImageData);
                }
                membership.ProfessionOrStudy = await professionOrStudyBL.GetByIdAsync(new ProfessionOrStudy { Id = membership.IdProfessionOrStudy });
                return View(membership); // Retornamos los Detalles a La Vista
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
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Membership membership = await membershipBL.GetByIdAsync(new Membership { Id = id });
                if (membership == null)
                {
                    return NotFound();
                }
                // Convertir el array de bytes en imagen para mostrar en la vista
                if (membership.ImageData != null && membership.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(membership.ImageData);
                }
                ViewBag.ProfessionOrStudies = await professionOrStudyBL.GetAllAsync();
                return View(membership);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto Membership
            }
        }

        // Accion Que Recibe Los Datos Del Formulario Para Ser Enviados a La BD
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Membership membership)
        {
            try
            {
                Membership membershipDB = await membershipBL.GetByIdAsync(membership);
                int result = await membershipBL.DeleteAsync(membershipDB);
                TempData["SuccessMessageDelete"] = "Miembro Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var membershipBD = await membershipBL.GetByIdAsync(membership);
                if (membershipBD == null)
                    membershipBD = new Membership();
                if (membershipBD.Id > 0)
                    membershipBD.ProfessionOrStudy = await professionOrStudyBL.GetByIdAsync(new ProfessionOrStudy { Id = membershipBD.IdProfessionOrStudy });
                return View(membershipBD);
            }
        }
        #endregion

        #region METODO PARA REPORTE
        // Metodo Para Generar Ficha o Reporte En PDF 
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<ActionResult> GeneratePDFfile(int id)
        {
            var generatePDF = await membershipBL.GetByIdAsync(new Membership { Id = id });
            string fileName = $"Ficha_{generatePDF.Name}_{generatePDF.LastName}_{generatePDF.Dui}_MCEI.pdf";
            return new ViewAsPdf("GeneratePDFfile", generatePDF)
            {
                FileName = fileName
            };
        }
        #endregion
    }
}
