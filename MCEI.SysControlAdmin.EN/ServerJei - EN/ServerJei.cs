﻿#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MCEI.SysControlAdmin.EN.Juventud___EN;
using MCEI.SysControlAdmin.EN.Privilege___EN;

#endregion

namespace MCEI.SysControlAdmin.EN.ServerJei___EN
{
    public class ServerJei
    {
        #region ATRIBUTOS DE LA ENTIDAD
        [Key]
        public int Id { get; set; }

        [ForeignKey("Juventud")]
        [Required(ErrorMessage = "La Juventud es Requerida")]
        [Display(Name = "Joven")]
        public int IdMembership { get; set; }

        [ForeignKey("Privilege")]
        [Required(ErrorMessage = "El Privilegio Es Requerido")]
        [Display(Name = "Privilegio")]
        public int IdPrivilege { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [Display(Name = "Estado")]
        public byte Status { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Fecha de Modificacion")]
        public DateTime DateModification { get; set; }
        #endregion

        public Privilege? Privilege { get; set; } // Propiedadd de Navegacion

        public Juventud? Juventud { get; set; }// Propiedad de Navegacion
    }
}