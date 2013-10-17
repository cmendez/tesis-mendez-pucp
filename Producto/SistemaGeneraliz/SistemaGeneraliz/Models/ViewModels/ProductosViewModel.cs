using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class ProductosViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionCorta { get; set; }
        public string DescripcionDetalle { get; set; }
        public int CategoriaProductoId { get; set; }
        public int ImagenId { get; set; }
        public string Precio { get; set; }
        public int SuministradorId { get; set; }
        public int NroClicksVisita { get; set; }
        public int NroBusquedas { get; set; }
        public string FechaRegistro { get; set; }
        public int IsVisible { get; set; }
        public int IsEliminado { get; set; }
    }
}