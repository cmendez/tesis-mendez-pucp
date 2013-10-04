using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("Configuraciones")]
    public class Configuracion
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ConfiguracionId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int ValorNumerico { get; set; }
        public string ValorTexto { get; set; }
    }
}