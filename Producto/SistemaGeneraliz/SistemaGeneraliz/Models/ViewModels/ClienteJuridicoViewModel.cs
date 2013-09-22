using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.ViewModels  
{
    public class ClienteJuridicoViewModel : PersonaJuridicaViewModel
    {
        public int ClienteId { get; set; }
        public virtual Persona Persona { get; set; }
    }
}