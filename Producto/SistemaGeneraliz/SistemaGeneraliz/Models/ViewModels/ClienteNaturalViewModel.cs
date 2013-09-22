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
    }
}