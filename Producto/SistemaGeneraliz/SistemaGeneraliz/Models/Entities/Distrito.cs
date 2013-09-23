using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("Distritos")]
    public class Distrito
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DistritoId { get; set; }
        public string NombreDistrito { get; set; }
        public int PaisCiudadId { get; set; }
        public virtual PaisCiudad PaisCiudad { get; set; }
        public double LatitudDefault { get; set; }
        public double LongitudDefault { get; set; }
        public virtual ICollection<UbicacionPersona> UbicacionesPersonas { get; set; }
        public int IsVisible { get; set; }
        public int IsEliminado { get; set; }
    }
}