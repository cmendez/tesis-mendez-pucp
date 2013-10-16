using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("Personas")]
    public class Persona
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PersonaId { get; set; }
		public virtual ICollection<Cliente> Clientes { get; set; }
		public virtual ICollection<Proveedor> Proveedores { get; set; }
		public virtual ICollection<Suministrador> Suministradores { get; set; }
        public string UserName { get; set; }
        public string TipoPersona { get; set; }
        public string TipoUsuario { get; set; }
        public long? RUC { get; set; }
        public int? DNI { get; set; }
        public string RazonSocial { get; set; }
        public string PrimerNombre { get; set; }
		public string SegundoNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaCreacion { get; set; }
		public string Sexo { get; set; }
		public string DireccionCompleta { get; set; }
        public virtual ICollection<UbicacionPersona> UbicacionesPersonas { get; set; }
		public string Email1 { get; set; }
		public string Email2 { get; set; }
		public string Telefono1 { get; set; }
		public string Telefono2 { get; set; }
		public string Telefono3 { get; set; }
		//public string ImagenPrincipal { get; set; }
        public int? ImagenId { get; set; }
        [ForeignKey("ImagenId")]
        public virtual Imagen Imagen { get; set; }
		/*public string Password { get; set; }*/
		public DateTime? UltimaActualizacionPersonal { get; set; }
		public int IsHabilitado { get; set; }
		public int IsEliminado { get; set; }
    }
}