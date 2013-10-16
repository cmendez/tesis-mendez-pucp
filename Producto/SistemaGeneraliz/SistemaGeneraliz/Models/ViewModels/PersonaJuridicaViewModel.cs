using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class PersonaJuridicaViewModel
    {
        public int PersonaId { get; set; }

        // ******************* ATRIBUTOS DE PERSONA JURÍDICA ************************************//

        public int UserName { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Tipo Persona")]
        public int TipoPersona { get; set; }

        [StringLength(11, ErrorMessage = "El campo {0} debe tener {2} caracteres de longitud.", MinimumLength = 11)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [RegularExpression(@"[0-9]{1,11}", ErrorMessage = "El campo {0} debe contener solo dígitos.")]
		[Display(Name = "RUC")]
        public string RUC { get; set; }

        [StringLength(70, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 2)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

		[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Fecha de Creación de la Empresa")]        
        public DateTime? FechaCreacion { get; set; }

		[Display(Name = "Dirección")]
		public string DireccionCompleta { get; set; }

        //****************************************************************
        [Display(Name = "Ciudad")]
        public int IdCiudad { get; set; }

        [Display(Name = "Distrito")]
        public int IdDistrito { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Referencias")]
        public string Referencia { get; set; }

        public double Latitud { get; set; }
        public double Longitud { get; set; }
        //****************************************************************

		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[StringLength(40, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 8)]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
        [Display(Name = "Email 1")] 
		public string Email1 { get; set; }
		
		[StringLength(40, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 8)]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
        [Display(Name = "Email 2")]
		public string Email2 { get; set; }
		
		[StringLength(15, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 7)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[Display(Name = "Teléfono 1")]
		public string Telefono1 { get; set; }
		
		[StringLength(15, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 7)]
		[Display(Name = "Teléfono 2")]
		public string Telefono2 { get; set; }
		
		[StringLength(15, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 7)]
		[Display(Name = "Teléfono 3")]
		public string Telefono3 { get; set; }

        [Display(Name = "Foto")]
        public int ImagenPrincipal { get; set; }

        //[Required(ErrorMessage = "El campo Foto es obligatorio.")]
        //[FileExtensions(Extensions = "jpg", ErrorMessage = "La imagen debe estar en formato jpg")] PARECE QUE NO FUNCIONA
        public HttpPostedFileBase File { get; set; }
		
		[StringLength(15, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 7)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[DataType(DataType.Password)]
		[Display(Name = "Contraseña")]
		public string Password { get; set; }
		
		[StringLength(15, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 7)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
		[DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }
		
		[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]		
		public DateTime? UltimaActualizacionPersonal { get; set; } //seteo automático por servidor
		
		[Display(Name = "¿Está habilitado?")]
		public int IsHabilitado { get; set; }
		
		[Display(Name = "¿Está eliminado?")]
		public int IsEliminado { get; set; }
    }
}