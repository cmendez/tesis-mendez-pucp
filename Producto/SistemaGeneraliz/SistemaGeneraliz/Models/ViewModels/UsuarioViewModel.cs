using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class UsuarioViewModel
    {
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Documento")]
        public string Documento { get; set; }

        [Display(Name = "Imagen")]
        public int ImagenId { get; set; }

        [Display(Name = "Tipo Usuario")]
        public string TipoUsuario { get; set; }

        [Display(Name = "Tipo Persona")]
        public string TipoPersona { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono1 { get; set; }

        [Display(Name = "Email1")]
        public string Email1 { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }
    }
}