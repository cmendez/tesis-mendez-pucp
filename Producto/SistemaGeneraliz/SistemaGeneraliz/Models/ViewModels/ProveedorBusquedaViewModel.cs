using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        public int FotoId { get; set; }
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
        [Display(Name = "Email 1")]
        public string Email1 { get; set; }
        [Display(Name = "Email 2")]
        public string Email2 { get; set; }
        [Display(Name = "Teléfono 1")]
        public string Telefono1 { get; set; }
        [Display(Name = "Teléfono 2")]
        public string Telefono2 { get; set; }
        [Display(Name = "Teléfono 3")]
        public string Telefono3 { get; set; }
    }
}