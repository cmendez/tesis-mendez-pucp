using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class HistorialTrabajosViewModel
    {
        [DisplayName("TrabajoProveedorId")]
        public int TrabajoProveedorId { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha")]
        public DateTime FechaTrabajo { get; set; }
        [DisplayName("Puntuación")]
        public string Puntuacion { get; set; }
        [DisplayName("Comentarios")]
        public string Comentarios { get; set; }
        [DisplayName("Cliente")]
        public string NombreCliente { get; set; }
        [DisplayName("Documento")]
        public string DocumentoCliente { get; set; }
        [DisplayName("Proveedor")]
        public string NombreProveedor { get; set; }
        [DisplayName("Documento")]
        public string DocumentoProveedor { get; set; }
        [DisplayName("Servicios")]
        public string Servicios { get; set; }
        [DisplayName("Descripción")]
        public string DescripcionCliente { get; set; }
        [DisplayName("Rec. Hon./Fac.")]
        public string ReciboHonorarios_Factura { get; set; }
        [DisplayName("Monto Cobrado")]
        public double MontoCobrado { get; set; }
        [DisplayName("Detalles")]
        public string LinkModificarDetalles { get; set; }
        [DisplayName("EncuestaRespondida")]
        public int EncuestaRespondida { get; set; }
    }
}