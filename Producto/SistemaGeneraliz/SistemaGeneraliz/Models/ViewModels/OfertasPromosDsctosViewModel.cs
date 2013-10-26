using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class OfertasPromosDsctosViewModel
    {
        public int OfertaPromoDsctoId { get; set; }
        public string Tipo { get; set; }
        
        [StringLength(19, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Ingrese solo letras y números para el campo {0}")]
        [Display(Name = "Nombre Corto")]
        public string NombreCorto { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Ingrese solo letras y números para el campo {0}")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [StringLength(500, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        
        public int ImagenPrincipalId { get; set; }
        [Display(Name = "Imagen")]
        public int Imagen1 { get; set; }
        public HttpPostedFileBase File1 { get; set; }

        public int ImagenBannerId { get; set; }
        [Display(Name = "Imagen")]
        public int Imagen2 { get; set; }
        public HttpPostedFileBase File2 { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 99999, ErrorMessage = "El precio máximo es 99999")] 
        [Display(Name = "Costo en Leads")]
        public int CostoEnLeads { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 99999, ErrorMessage = "El precio máximo es 99999")]
        [Display(Name = "Cantidad Disponible")]
        public int CantidadDisponible { get; set; }

        public int SuministradorId { get; set; }
        public string Suministrador { get; set; }
        
        public int IsAdquiribleConLeads { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Fecha Inicio")]
        public DateTime FechaInicio { get; set; }

        public string FechaInicioString { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Fecha Fin")]
        public DateTime FechaFin { get; set; }

        public string FechaFinString { get; set; }

        public string FechaRegistro { get; set; }

        [Display(Name = "¿Será visible al público?")]
        public int IsVisible { get; set; }

        [Display(Name = "¿Eliminar producto?")]
        public int IsEliminado { get; set; }
    }
}