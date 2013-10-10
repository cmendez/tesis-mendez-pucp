using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class EncuestasClientesViewModel
    {
        public int ClienteId { get; set; }
        public int TrabajoProveedorId { get; set; }
        public int EncuestaClienteId { get; set; }
        [DisplayName("Fecha Trabajo")]
        public string FechaTrabajo { get; set; }
        [DisplayName("Foto")]
        public string FotoProveedor { get; set; }
        [DisplayName("Proveedor")]
        public string NombreProveedor { get; set; }
        [DisplayName("Documento")]
        public string DocumentoProveedor { get; set; }
        [DisplayName("Servicios Brindados")]
        public string Servicios { get; set; }
        [DisplayName("Descripción Trabajo")]
        public string DescripcionCliente { get; set; }
        [DisplayName("Dirección")]
        public string DireccionCiente { get; set; }
    }
}