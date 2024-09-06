#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.ProfessionOrStudy___BL;
using MCEI.SysControlAdmin.EN.Membership___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Recommendation___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador")]
    public class RecommendationController : Controller
    {
        // Creamos Una Instancia Para Acceder a Los Metodos
        MembershipBL membershipBL = new MembershipBL();
        ProfessionOrStudyBL professionOrStudyBL = new ProfessionOrStudyBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador")]
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

        #region METODO PARA REPORTE
        // Metodo Para Generar Ficha o Reporte En PDF 
        public async Task<ActionResult> GeneratePDFfile(int id)
        {
            var generatePDF = await membershipBL.GetByIdAsync(new Membership { Id = id });
            string fileName = $"CartaDeRecomendación_{generatePDF.Name}_{generatePDF.LastName}_{generatePDF.Dui}_MCEI.pdf";
            return new ViewAsPdf("GeneratePDFfile", generatePDF)
            {
                FileName = fileName
            };
        }
        #endregion
    }
}