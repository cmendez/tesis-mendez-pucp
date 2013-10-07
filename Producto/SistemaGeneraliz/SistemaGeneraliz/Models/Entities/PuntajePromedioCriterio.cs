using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("PuntajesPromedioCriterio")]
    public class PuntajePromedioCriterio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PuntajePromedioCriterioId { get; set; }
        public int ProveedorId { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public int CriterioCalificacionId { get; set; }
        public virtual CriterioCalificacion CriterioCalificacion { get; set; }
        public int Puntaje { get; set; }
    }
}