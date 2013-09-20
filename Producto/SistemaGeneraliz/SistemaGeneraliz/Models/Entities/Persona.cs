using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("Persona")]
    public class Persona
    {
        //esta clase ya no tendrá los DataAnnotations, solo propiedades simples (los viewmodel sí los tendrán)

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PersonaId { get; set; }

        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Tipo Persona")]
        public int TipoPersona { get; set; }

        [StringLength(11, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 11)]
        [Display(Name = "RUC")]
        public int? RUC { get; set; }

        [StringLength(8, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 8)]
        [Display(Name = "DNI")]
        public int? DNI { get; set; }

        [StringLength(30, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 2)]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        [StringLength(11, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 11)]
        [Display(Name = "Nombres")]
        public string PrimerNombre { get; set; }

        [StringLength(11, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 11)]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [StringLength(11, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 11)]
        [Display(Name = "Razón Social")]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Fecha de Creacion")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaCreacion { get; set; }


    }
}