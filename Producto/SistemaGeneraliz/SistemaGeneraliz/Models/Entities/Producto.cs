using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductoId { get; set; }
        public string NombreCorto { get; set; }
        public string NombreCompleto { get; set; }
        public string Descripcion { get; set; }
        public int CategoriaProductoId { get; set; }
        public virtual CategoriaProducto CategoriaProducto { get; set; }
        public int? ImagenId { get; set; }
        [ForeignKey("ImagenId")]
        public virtual Imagen Imagen { get; set; }
        public double Precio { get; set; }
        public int SuministradorId { get; set; }
        public virtual Suministrador Suministrador { get; set; }
        public int NroClicksVisita { get; set; }
        public int NroBusquedas { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IsVisible { get; set; }
        public int IsEliminado { get; set; }
    }
}