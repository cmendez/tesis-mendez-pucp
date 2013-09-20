using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    public class Proveedor : PersonaNaturalViewModel
    {
		//VERIFICAR QUE SE HAGA BIEN LA HERENCIA:
		/*
			Que al crear la VIEW automáticamente, se cree con TODOS los campos
			de PersonaNaturalViewModel y de Proveedor. Sino los crea automáticamente,
			podemos probar con el RenderPartialView o por último crear 
			ProveedorNaturalViewModel y ProveedorJuridicoViewModel			
		*/
		public int ProveedorId { get; set; }
		public int PersonaId { get; set; }
        public virtual Persona Persona { get; set; }
		
		[Display(Name = "Leads Disponibles")]
		public int LeadsDisponibles { get; set; }		
		
		public int EspecialidadId { get; set; }
		
		[Display(Name = "Especialidad")]
		public string Especialidad { get; set; }
		
		[Display(Name = "Puntuacion Promedio")]
		public double PuntuacionPromedio { get; set; }
		
		[Display(Name = "Nro. de Trabajos Terminados")]
		public int NroTrabajosTerminados { get; set; }
		
		[Display(Name = "Nro. de Búsquedas")]
		public int NroBusquedas { get; set; }
		
		[Display(Name = "Nro. de Visitas")]
		public int NroClicksVisita { get; set; }
		
		[Display(Name = "Nro. de Comentarios")]
		public int NroComentarios { get; set; }
		
		[Display(Name = "Nro. de Calificaciones")]
		public int NroCalificaciones { get; set; }
		
		[Display(Name = "Nro. de Recomendaciones")]
		public int NroRecomendaciones { get; set; }
		
		[Display(Name = "Nro. de Volvería a Contratarlo")]
		public int NroVolveriaContratarlo { get; set; }
		
		[Url]
		[StringLength(50, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 3)]
		[Display(Name = "Página Web")]
		public string PaginaWeb { get; set; }

		[Url]
		[StringLength(50, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 3)]
		[Display(Name = "Facebook")]
		public string Facebook { get; set; }
		
		[StringLength(50, ErrorMessage = "El campo {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 3)]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[Display(Name = "Acerca de mí")]
		public string AcercaDeMi { get; set; }
		
		public int IsDestacado { get; set; }
    }
}