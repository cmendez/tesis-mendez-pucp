using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("TiposServicios")]
    public class TipoServicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipoServicioId { get; set; }
        public string NombreServicio { get; set; }
        public string DescripcionServicio { get; set; }
        public virtual ICollection<Proveedor> Proveedores { get; set; }
        public virtual ICollection<TrabajoProveedor> TrabajosProveedores { get; set; }
        public int IsEliminado { get; set; }
    }
}