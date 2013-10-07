using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("CriteriosCalificacion")]
    public class CriterioCalificacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CriterioCalificacionId { get; set; }
        public string NombreCriterio { get; set; }
        public string TipoPregunta { get; set; }
        public string PreguntaAsociada { get; set; }
        public int PuntajeMaximo { get; set; }
        public virtual ICollection<RespuestaPorCriterio> RespuestasPorCriterio { get; set; }
        public int IsEliminado { get; set; }
    }
}