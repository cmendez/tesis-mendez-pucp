using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;
using SistemaGeneraliz.Models.ViewModels;

namespace SistemaGeneraliz.Models.BusinessLogic
{
    public class LogicaPersonas
    {
        private readonly ISGPFactory _sgpFactory;

        public LogicaPersonas()
        {
            _sgpFactory = new SGPFactory();
        }

        public LogicaPersonas(ISGPFactory sgpFactory)
        {
            _sgpFactory = sgpFactory;
        }

        internal void AgregarPersona(Persona persona)
        {
            _sgpFactory.AgregarPersona(persona);
        }

        internal Persona CrearObjetoPersonaNatural(PersonaNaturalViewModel persona, String tipoUsuario)
        {
            persona.Sexo = (persona.SexoId == 1) ? "Masculino" : "Femenino";
            //string tipoUsuario = "";
            //if (persona.GetType() == typeof(Proveedor))
            //    tipoUsuario = "Proveedor";
            //if (persona.GetType() == typeof(Cliente))
            //    tipoUsuario = "Cliente";
            //if (persona.GetType() == typeof(Suministrador))
            //    tipoUsuario = "Suministrador";

            return new Persona
            {
                UserName = persona.DNI,
                TipoPersona = "Natural",
                TipoUsuario = tipoUsuario,
                DNI = Int32.Parse(persona.DNI),
                RUC = (!String.IsNullOrEmpty(persona.RUC)) ? Int64.Parse(persona.RUC) : -1,
                PrimerNombre = persona.PrimerNombre,
                SegundoNombre = persona.SegundoNombre,
                ApellidoMaterno = persona.ApellidoMaterno,
                ApellidoPaterno = persona.ApellidoPaterno,
                FechaNacimiento = persona.FechaNacimiento,
                Sexo = persona.Sexo,
                //DireccionCompleta = persona.DireccionCompleta,
                Email1 = persona.Email1,
                Email2 = persona.Email2,
                Telefono1 = persona.Telefono1,
                Telefono2 = persona.Telefono2,
                Telefono3 = persona.Telefono3,
                //ImagenPrincipal = persona.ImagenPrincipal,
                UltimaActualizacionPersonal = DateTime.Now,
                IsHabilitado = 1, //true
                IsEliminado = 0 //false
            };
        }

        internal Persona CrearObjetoPersonaJuridica(PersonaJuridicaViewModel persona, String tipoUsuario)
        {
            //string tipoUsuario = "";
            //if (persona.GetType() == typeof(Proveedor))
            //    tipoUsuario = "Proveedor";
            //if (persona.GetType() == typeof(Cliente))
            //    tipoUsuario = "Cliente";
            //if (persona.GetType() == typeof(Suministrador))
            //    tipoUsuario = "Suministrador";

            return new Persona
            {
                UserName = persona.RUC,
                TipoPersona = "Juridica",
                TipoUsuario = tipoUsuario,
                RUC = Int64.Parse(persona.RUC),
                RazonSocial = persona.RazonSocial,
                FechaCreacion = persona.FechaCreacion,
                //DireccionCompleta = persona.DireccionCompleta,
                Email1 = persona.Email1,
                Email2 = persona.Email2,
                Telefono1 = persona.Telefono1,
                Telefono2 = persona.Telefono2,
                Telefono3 = persona.Telefono3,
                //ImagenPrincipal = persona.ImagenPrincipal,
                UltimaActualizacionPersonal = DateTime.Now,
                IsHabilitado = 1, //true
                IsEliminado = 0 //false
            };
        }

        public string GetNombrePersonaLoggeada(int currentUserId)
        {
            var nombre = "";
            var persona = _sgpFactory.GetPersonaLoggeada(currentUserId);
            if (persona.TipoPersona == "Natural")
                return (persona.PrimerNombre + " " + persona.ApellidoPaterno);
            else if (persona.TipoPersona == "Juridica")
                return (persona.RazonSocial);

            return nombre;
        }

        internal List<SelectListItem> GetDistritos()
        {
            List<SelectListItem> listDistritos = new List<SelectListItem>();
            List<Distrito> distritos = _sgpFactory.GetDistritos();

            foreach (Distrito distrito in distritos)
            {
                listDistritos.Add(new SelectListItem() { Text = distrito.NombreDistrito, Value = distrito.DistritoId.ToString() });
            }
            return listDistritos;
        }
    }
}