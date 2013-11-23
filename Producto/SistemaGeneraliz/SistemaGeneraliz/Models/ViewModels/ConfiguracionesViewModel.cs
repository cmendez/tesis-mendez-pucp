using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class ConfiguracionesViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Leads gratis al registrarse")]
        public int LeadsGratisRegistro { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Puntaje Promedio Inicial Proveedores")]
        public int PuntajePromedioInicialProveedores { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Puntuación mínima requerida del proveedor para ser considerado en la lógica del algoritmo")]
        public int PuntuacionMinimaAlgoritmo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Cantidad máxima de proveedores que se devuelven al buscar proveedores dado un servicio, para la lógica del algoritmo")]
        public int CantidadMaximaProveedoresAlgoritmo { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Leads de recompensa a mejores proveedores")]
        public int NroLeadsRecompensa { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Cantidad de proveedores que reciben recompensa")]
        public int NroProveedoresRecompensa { get; set; }
    }
}