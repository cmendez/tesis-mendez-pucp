using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class DemandaServiciosGeneralesViewModel
    {
        [Display(Name = "Año")]
        public int Año { get; set; }

        [Display(Name = "Mes")]
        public int Mes { get; set; }

        [Display(Name = "Servicio")]
        public string NombreServicio { get; set; }

        [Display(Name = "Distrito")]
        public string Distrito { get; set; }

        [Display(Name = "Nro. Trabajos")]
        public int NroTrabajos { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00#}", ApplyFormatInEditMode = true)]
        [Display(Name = "Calif. Promedio")]
        public double CalificacionPromedio { get; set; }

        //[Display(Name = "Nro. Proveedores")]
        //public int NroProveedores { get; set; }
    }
}