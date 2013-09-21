using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaGeneraliz.Models.Entities
{
    public class PersonaNaturalViewModel
    {
        public int PersonaId { get; set; }
	
		// ******************* ATRIBUTOS DE PERSONA ************************************//
		
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Tipo Persona")]
        public int TipoPersona { get; set; }

        [StringLength(8, ErrorMessage = "El campo {0} debe tener {2} caracteres de longitud.", MinimumLength = 8)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "DNI")]
        public int DNI { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 3)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Nombres")]
        public string PrimerNombre { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 3)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 3)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Razón Social")]
        public string ApellidoMaterno { get; set; }

		[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Fecha de Nacimiento")]        
        public DateTime? FechaNacimiento { get; set; }

		public int SexoId { get; set; }	
		
		[Display(Name = "Sexo")] 
		public string Sexo { get; set; }	
		
		[Display(Name = "Dirección")] 
		public string DireccionCompleta { get; set; }

		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[StringLength(40, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 8)]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
        [Display(Name = "Email 1")] 
		public string Email1 { get; set; }
		
		[StringLength(40, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 8)]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
        [Display(Name = "Email 2")]
		public string Email2 { get; set; }
		
		[StringLength(15, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 7)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[Display(Name = "Teléfono 2")]
		public string Telefono1 { get; set; }
		
		[StringLength(15, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 7)]
		[Display(Name = "Teléfono 2")]
		public string Telefono2 { get; set; }
		
		[StringLength(15, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 7)]
		[Display(Name = "Teléfono 3")]
		public string Telefono3 { get; set; }
				
		public string ImagenPrincipal { get; set; }
		
		[StringLength(15, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 7)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[DataType(DataType.Password)]
		[Display(Name = "Contraseña")]
		public string Password { get; set; }
			
        [StringLength(15, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 7)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }
		
		[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]		
		public DateTime? UltimaActualizacionPersonal { get; set; } //seteo automático por servidor
		
		[Display(Name = "¿Está habilitado?")]
		public int IsHabilitado { get; set; }
		
		[Display(Name = "¿Está eliminado?")]
		public int IsEliminado { get; set; }
    }
}