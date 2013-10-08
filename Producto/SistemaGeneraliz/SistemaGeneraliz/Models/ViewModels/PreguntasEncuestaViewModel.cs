using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.ViewModels
{
    public class PreguntasEncuestaViewModel
    {
        public int CriterioId { get; set; }
        public string TipoPregunta { get; set; }
        public string PreguntaAsociada { get; set; }
        public int PuntajeOtorgado  { get; set; }
        public string RespuestaPregunta { get; set; }
        public int NroOpciones { get; set; }
    }
}