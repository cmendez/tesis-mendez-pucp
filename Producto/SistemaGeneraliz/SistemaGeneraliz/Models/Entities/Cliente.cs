using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
	[Table("Cliente")]
    public class Cliente
    {
		[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int ClienteId { get; set; }
		public int PersonaId { get; set; }
        public virtual Persona Persona { get; set; }
    }
}