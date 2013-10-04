using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("TrabajosProveedores")]
    public class TrabajoProveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrabajoProveedorId { get; set; }
        public int TrabajoId { get; set; }
        public virtual Trabajo Trabajo { get; set; }
        public int ProveedorId { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public virtual ICollection<TipoServicio> TiposServicios { get; set; }
        public string DescripcionProveedor { get; set; }
        public string NroRpH_Factura { get; set; }
        public string TipoRpH_Factura { get; set; }
        public double MontoCobrado { get; set; }
        /*public int EncuestaClienteId { get; set; }
        public virtual EncuestaCliente EncuestaCliente { get; set; }*/
    }
}