using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("EncuestasClientes")]
    public class EncuestaCliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EncuestaClienteId { get; set; }
        //public int TrabajoProveedorId { get; set; }
        //public /*virtual*/ TrabajoProveedor TrabajoProveedor { get; set; }
        public DateTime Fecha { get; set; }
        public virtual ICollection<RespuestaPorCriterio> RespuestasPorCriterio { get; set; }
        public string ComentariosCliente { get; set; }
        public string ComentariosProveedor { get; set; }
        public int PuntajeTotal { get; set; }
        public int IsVisible { get; set; }
        public int IsEliminado { get; set; }
    }
}