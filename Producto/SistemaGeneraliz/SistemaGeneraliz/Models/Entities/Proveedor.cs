using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGeneraliz.Models.Entities
{
	[Table("Proveedor")]
    public class Proveedor
    {
		[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int ProveedorId { get; set; }
		public int PersonaId { get; set; }
        public virtual Persona Persona { get; set; }
		public int LeadsDisponibles { get; set; }
		public string Especialidad { get; set; }
		public double PuntuacionPromedio { get; set; }
		public int NroTrabajosTerminados { get; set; }
		public int NroBusquedas { get; set; }
		public int NroClicksVisita { get; set; }
		public int NroComentarios { get; set; }
		public int NroCalificaciones { get; set; }
		public int NroRecomendaciones { get; set; }
		public int NroVolveriaContratarlo { get; set; }
		public string PaginaWeb { get; set; }
		public string Facebook { get; set; }
		public string AcercaDeMi { get; set; }
		public int IsDestacado { get; set; }
    }
}