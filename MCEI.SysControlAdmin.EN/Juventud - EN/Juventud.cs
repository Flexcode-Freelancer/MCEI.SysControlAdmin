#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MCEI.SysControlAdmin.EN.ProfessionOrStudy___EN;


#endregion

namespace MCEI.SysControlAdmin.EN.Juventud___EN
{
    public class Juventud
    {
        #region Atributos de la Entidad
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre Es Requerido")]
        [StringLength(50, ErrorMessage = "Maximo 50 caracteres")]
        [Display(Name = "Nombres")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Debe contener solo Letras")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Apellido Es Requerido")]
        [StringLength(50, ErrorMessage = "Maximo 50 caracteres")]
        [Display(Name = "Apellidos")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Debe contener solo Letras")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Fecha De Nacimiento Es Requerida")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date, ErrorMessage = "Por favor, introduce una fecha válida")]
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;

        [Required(ErrorMessage = "La Edad Es Requerida")]
        [StringLength(3, ErrorMessage = "Maximo 3 caracteres")]
        [Display(Name = "Edad")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "La edad debe contener solo números")]
        public string Age { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Genero Es Requerido")]
        [StringLength(20, ErrorMessage = "Maximo 20 caracteres")]
        [Display(Name = "Genero")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Debe contener solo Letras")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Estado Civil Es Requerido")]
        [StringLength(30, ErrorMessage = "Maximo 30 caracteres")]
        [Display(Name = "Estado Civil")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ/ ]+$", ErrorMessage = "Debe contener solo Letras")]
        public string CivilStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Telefono Es Requerido")]
        [StringLength(9, ErrorMessage = "Maximo 9 caracteres")]
        [Display(Name = "Telefono")]
        [RegularExpression("^[0-9-]+$", ErrorMessage = "El Telefono debe contener solo números")]
        public string Phone { get; set; } = string.Empty;

        [ForeignKey("ProfessionOrStudy")]
        [Required(ErrorMessage = "Profesion u Oficio Es Requerido")]
        [Display(Name = "Profesion u Oficio")]
        public int IdProfessionOrStudy { get; set; }

        [Required(ErrorMessage = "Bautismo En Agua Es Requerido")]
        [StringLength(25, ErrorMessage = "Maximo 25 caracteres")]
        [Display(Name = "Bautizmo En Agua")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ, ]+$", ErrorMessage = "Debe contener solo Letras")]
        public string WaterBaptism { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bautizmo Del Espiritu Santo Es Requerido")]
        [StringLength(2, ErrorMessage = "Maximo 2 caracteres")]
        [Display(Name = "Bautizmo Por El Espiritu Santo")]
        public string BaptismOfTheHolySpirit { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Estatus Es Requerido")]
        [Display(Name = "Estatus")]
        public byte Status { get; set; }

        [Required(ErrorMessage = "Los Comentarios u Observaciones Son Requeridas")]
        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Comentarios u Observaciones")]
        public string CommentsOrObservations { get; set; } = string.Empty;

        [Display(Name = "Fecha de Creacion")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Fecha de Modificacion")]
        public DateTime DateModification { get; set; }

        [Display(Name = "Fotografia")]
        public byte[]? ImageData { get; set; }

        #endregion

        public ProfessionOrStudy? ProfessionOrStudy { get; set; } //propiedad de navegación

        //public List<ServerJei> ServerJei { get; set; } = new List<ServerJei>(); // Propiedad de navegacion

        //public List<HistoryServerJei> HistoryServerJei { get; set; } = new List<HistoryServerJei>(); // Propiedad de navegacion
    }
}
