using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class RecargasLeadsViewModel
    {
        [DisplayName("Recarga ID")]
        public int RecargaLeadsId { get; set; }
        [DisplayName("Proveedor")]
        public string NombreProveedor { get; set; }
        [DisplayName("Documento")]
        public string DocumentoProveedor { get; set; }
        [DisplayName("Fecha")]
        public string FechaRecarga { get; set; }
        [DisplayName("Monto")]
        public string MontoRecarga { get; set; }
        public string TipoMoneda { get; set; }
        [DisplayName("Leads")]
        public int CantidadLeads { get; set; }
    }
}