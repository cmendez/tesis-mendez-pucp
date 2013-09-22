using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class SuministradorJuridicoViewModel : PersonaJuridicaViewModel
    {
        public int SuministradorId { get; set; }
        public virtual Persona Persona { get; set; }

        [Display(Name = "Leads Disponibles")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1,9999)]
        public int LeadsDisponibles { get; set; }

        [Display(Name = "Leads Reserva")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 9999)]
        public int LeadsReserva { get; set; }

        [Url]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Display(Name = "Página Web")]
        public string PaginaWeb { get; set; }

        [Url]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Display(Name = "Cuenta de Facebook (link)")]
        public string Facebook { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Acerca de mí")]
        public string AcercaDeMi { get; set; }

        public int IsDestacado { get; set; }
    }
}