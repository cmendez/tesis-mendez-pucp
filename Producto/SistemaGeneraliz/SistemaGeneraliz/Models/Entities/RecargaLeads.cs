using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("RecargasLeads")]
    public class RecargaLeads
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RecargaLeadsId { get; set; }
        public int SuministradorId { get; set; }
        public virtual Suministrador Suministrador { get; set; }
        public int ProveedorId { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public DateTime? FechaRecarga { get; set; }
        public int MontoRecarga { get; set; }
        public string TipoMoneda { get; set; }
        public int CantidadLeads { get; set; }
    }
}