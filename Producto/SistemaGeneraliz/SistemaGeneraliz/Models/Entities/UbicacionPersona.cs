using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("UbicacionesPersonas")]
    public class UbicacionPersona
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UbicacionPersonaId { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int DistritoId { get; set; }
        public virtual Distrito Distrito { get; set; }
        public int PersonaId { get; set; }
        public virtual Persona Persona { get; set; }
        public int IsVisible { get; set; }
        public int IsEliminado { get; set; }
    }
}