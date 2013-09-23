using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("PaisesCiudades")]
    public class PaisCiudad
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PaisCiudadId { get; set; }
        public string NombrePais { get; set; }
        public string NombreCiudad { get; set; }
        public virtual ICollection<Distrito> Distritos { get; set; }
        public int IsVisible { get; set; }
        public int IsEliminado { get; set; }
    }
}