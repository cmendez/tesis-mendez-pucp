using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("Trabajos")]
    public class Trabajo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrabajoId { get; set; }
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<TrabajoProveedor> TrabajosProveedores { get; set; }
        public DateTime Fecha { get; set; }
        public string Direccion { get; set; }
        public string DescripcionCliente { get; set; }
        public string NroRecibosPorHonorarios { get; set; }
        public double HonorariosConseguidos { get; set; }
        public int IsTerminado { get; set; }
        public int IsEliminado { get; set; }
    }
}