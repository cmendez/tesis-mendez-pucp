using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class HistorialTrabajosViewModel
    {
        [DisplayName("TrabajoProveedorId")]
        public int TrabajoProveedorId { get; set; }
        [DisplayName("Fecha")]
        public string FechaTrabajo { get; set; }
        [DisplayName("Puntuación")]
        public string Puntuacion { get; set; }
        [DisplayName("Cliente")]
        public string NombreCliente { get; set; }
        [DisplayName("Documento")]
        public string DocumentoCliente { get; set; }
        [DisplayName("Servicios")]
        public string Servicios { get; set; }
        [DisplayName("Descripción")]
        public string DescripcionCliente { get; set; }
        [DisplayName("Honorarios/Factura")]
        public string ReciboHonorarios_Factura { get; set; }
        [DisplayName("Monto Cobrado")]
        public string MontoCobrado { get; set; }
        [DisplayName("Detalles")]
        public string LinkModificarDetalles { get; set; }
    }
}