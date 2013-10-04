using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class ProveedorBusquedaViewModel
    {
        public int ProveedorId { get; set; }
        [DisplayName("Calificación")]
        public string Puntaje { get; set; }
        [DisplayName("Foto")]
        public string RutaFoto { get; set; }
        [DisplayName("Nombre / Razón Social")]
        public string NombreCompleto { get; set; }
        [DisplayName("DNI / RUC")]
        public string TipoDocumento { get; set; }
        [DisplayName("Documento")]
        public string Documento { get; set; }
        [DisplayName("Servicio Ofrecido")]
        public string Servicio { get; set; }
        public string ServicioId { get; set; }
        [DisplayName("Sobre Mí")]
        public string Descripcion { get; set; }
        [DisplayName(" ")]
        public string VerTrabajos { get; set; }
        [DisplayName(" ")]
        public string VerComentarios { get; set; }
    }
}