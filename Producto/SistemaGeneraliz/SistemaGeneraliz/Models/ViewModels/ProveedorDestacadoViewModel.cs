using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class ProveedorDestacadoViewModel
    {
		public int ProveedorId { get; set; }

        [Display(Name = "Proveedor")]
        public string NombreProveedor { get; set; }

        [Display(Name = "DNI/RUC")]
        public string Documento { get; set; }

        [Display(Name = "Imagen")]
        public int Imagen { get; set; }

        [Display(Name = "Leads Compras")]
        public int LeadsCompras { get; set; }

        [Display(Name = "Servicios")]
		public string Servicios { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.0#}", ApplyFormatInEditMode = true)]
        [Display(Name = "Calif. Prom.")]
		public double PuntuacionPromedio { get; set; }

        [Display(Name = "Cant. Trabajos")]
		public int NroTrabajos { get; set; }

        [Display(Name = "# Recomen.")]
		public string NroRecomendaciones { get; set; }

        [Display(Name = "# Volvería")]
        public string NroVolveriaContratarlo { get; set; }
    }
}