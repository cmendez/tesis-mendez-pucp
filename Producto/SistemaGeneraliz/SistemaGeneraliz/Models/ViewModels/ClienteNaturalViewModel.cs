using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class ClienteNaturalViewModel : PersonaNaturalViewModel
    {
        public int ClienteId { get; set; }
        public virtual Persona Persona { get; set; }

        [StringLength(11, ErrorMessage = "El campo {0} debe tener {2} caracteres de longitud.", MinimumLength = 11)]
        [RegularExpression(@"[0-9]{1,11}", ErrorMessage = "El campo {0} debe contener solo dígitos.")]
        [Display(Name = "RUC")]
        public string RUC { get; set; }
    }
}