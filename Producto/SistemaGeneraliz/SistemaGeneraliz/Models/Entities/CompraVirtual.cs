using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("ComprasVirtuales")]
    public class CompraVirtual
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CompraVirtualId { get; set; }
        public int OfertaPromoDsctoId { get; set; }
        public virtual OfertaPromoDscto OfertaPromoDscto { get; set; }
        public int ProveedorId { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public int LeadsPagados { get; set; }
        public int IsEliminado { get; set; }

    }
}