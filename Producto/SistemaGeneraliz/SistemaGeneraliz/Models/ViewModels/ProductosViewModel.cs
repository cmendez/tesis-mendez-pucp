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
        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        public string NombreCorto { get; set; }
        public string NombreCompleto { get; set; }
        public string Descripcion { get; set; }
        public int CategoriaProductoId { get; set; }
        public int ImagenId { get; set; }
        public string Precio { get; set; }
        public int SuministradorId { get; set; }
        public string Suministrador { get; set; }
        public int NroClicksVisita { get; set; }
        public int NroBusquedas { get; set; }
        public string FechaRegistro { get; set; }
        public int IsVisible { get; set; }
        public int IsEliminado { get; set; }
    }
}