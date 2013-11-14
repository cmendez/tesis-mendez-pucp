using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class ProductosViewModel
    {
        public int ProductoId { get; set; }

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

        [Display(Name = "Categoría")]
        public int CategoriaProductoId { get; set; }
        public string NombreCategoria { get; set; }
        
        public int ImagenId { get; set; }
        
        [Display(Name = "Imagen Producto")]
        public int Imagen { get; set; }
        public HttpPostedFileBase File { get; set; }
        
        public string Precio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0, 99999, ErrorMessage = "El precio máximo es 99999")] 
        [Display(Name = "Precio")]
        public double PrecioProducto { get; set; }

        public int SuministradorId { get; set; }
        public string Suministrador { get; set; }
        public int NroClicksVisita { get; set; }
        public int NroBusquedas { get; set; }
        public string FechaRegistro { get; set; }
        [Display(Name = "¿Será visible al público?")]
        public int IsVisible { get; set; }
        [Display(Name = "¿Eliminar producto?")]
        public int IsEliminado { get; set; }
    }
}