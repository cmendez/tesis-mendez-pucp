using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGeneraliz.Models.Entities
{
	[Table("Clientes")]
    public class Cliente
    {
		[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int ClienteId { get; set; }
		public int PersonaId { get; set; }
        public virtual Persona Persona { get; set; }
    }
}