using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGeneraliz.Models.Entities
{
	[Table("Suministrador")]
    public class Suministrador
    {
		[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int SuministradorId { get; set; }
		public int PersonaId { get; set; }
        public virtual Persona Persona { get; set; }
		public int LeadsDisponibles { get; set; }
		public int LeadsReserva { get; set; }
		public string PaginaWeb { get; set; }
		public string Facebook { get; set; }
		public string AcercaDeMi { get; set; }
		public int IsDestacado { get; set; }
    }
}