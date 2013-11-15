using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class ConversionLeadsViewModel
    {
        [Display(Name = "Suministrador")]
        public string RazonSocial { get; set; }

        [Display(Name = "Imagen")]
        public int ImagenPrincipal { get; set; }
        
        [Display(Name = "Leads Disponibles")]
        public int LeadsDisponibles { get; set; }

        [Display(Name = "Año")]
        public int Año { get; set; }
        
        [Display(Name = "Mes")]
        public int Mes { get; set; }

        [Display(Name = "Leads Mensuales")]
        public int LeadsMensuales { get; set; }

        [Display(Name = "Compras")]
        public int MontoComprasLogradas { get; set; }

        [Display(Name = "Recargas")]
        public int MontoRecargasLogradas { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.0#}", ApplyFormatInEditMode = true)]
        [Display(Name = "% Conversión")]
        public double TasaConversion { get; set; }
    }
}