﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class ProveedorJuridicoViewModel : PersonaJuridicaViewModel
    {
        public int ProveedorId { get; set; }
        public virtual Persona Persona { get; set; }

        [Display(Name = "Leads Disponibles")]
        public int LeadsDisponibles { get; set; }

        public int EspecialidadId { get; set; }

        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; }

        [Display(Name = "Servicios que brinda")]
        public virtual ICollection<TipoServicio> TiposServicios { get; set; }

        public int[] ListTiposServiciosIds { get; set; }

        [Display(Name = "Puntuacion Promedio")]
        public double PuntuacionPromedio { get; set; }

        [Display(Name = "Nro. de Trabajos Terminados")]
        public int NroTrabajosTerminados { get; set; }

        [Display(Name = "Nro. de Búsquedas")]
        public int NroBusquedasCliente { get; set; }

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
        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Display(Name = "Página Web")]
        public string PaginaWeb { get; set; }

        [Url]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Display(Name = "Cuenta de Facebook (link)")]
        public string Facebook { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 3)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Acerca de mí")]
        public string AcercaDeMi { get; set; }

        public int IsDestacado { get; set; }
    }
}