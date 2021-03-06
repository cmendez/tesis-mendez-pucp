﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGeneraliz.Models.Entities
{
	[Table("Proveedores")]
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
		public int NroBusquedasCliente { get; set; }
		public int NroClicksVisita { get; set; }
		public int NroComentarios { get; set; }
		public int NroCalificaciones { get; set; }
		public int NroRecomendaciones { get; set; }
		public int NroVolveriaContratarlo { get; set; }
		public string PaginaWeb { get; set; }
		public string Facebook { get; set; }
		public string AcercaDeMi { get; set; }
		public int IsDestacado { get; set; }
        public virtual ICollection<RecargaLeads> RecargasLeads { get; set; }
        public virtual ICollection<TipoServicio> TiposServicios { get; set; }
        public virtual ICollection<TrabajoProveedor> TrabajosProveedores { get; set; }
        public virtual ICollection<CompraVirtual> ComprasVirtuales { get; set; }

        [NotMapped]
        public double Distancia { get; set; }
        [NotMapped]
        public double Factor { get; set; }
    }
}