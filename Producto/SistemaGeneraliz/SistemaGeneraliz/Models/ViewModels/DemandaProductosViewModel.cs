using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class DemandaProductosViewModel
    {
        [Display(Name = "Nombre Corto")]
        public string NombreCorto { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Categoría")]
        public string NombreCategoria { get; set; }

        [Display(Name = "Imagen")]
        public int ImagenId { get; set; }
        
        [Display(Name = "Precio")]
        public double PrecioProducto { get; set; }

        [Display(Name = "Suministrador")]
        public string Suministrador { get; set; }

        [Display(Name = "Nro. visitas")]
        public int NroClicksVisita { get; set; }

        [Display(Name = "Nro. búsquedas")]
        public int NroBusquedas { get; set; }

    }
}