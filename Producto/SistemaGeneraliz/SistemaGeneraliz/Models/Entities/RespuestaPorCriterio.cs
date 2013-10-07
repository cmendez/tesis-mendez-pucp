using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("RespuestasPorCriterios")]
    public class RespuestaPorCriterio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RespuestaPorCriterioId { get; set; }
        public int EncuestaClienteId { get; set; }
        public virtual EncuestaCliente EncuestaCliente { get; set; }
        public int PuntajeOtorgado { get; set; }
        public string RespuestaPregunta { get; set; }
    }
}