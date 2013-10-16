using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("CategoriasProducto")]
    public class CategoriaProducto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoriaProductoId { get; set; }
        public string NombreCategoria { get; set; }
        public string DescripcionCategoria { get; set; }
        //public virtual ICollection<SubcategoriaProducto> SubcategoriasProducto { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
        public int IsEliminado { get; set; }
        public double PrecioPromedio { get; set; }
    }
}