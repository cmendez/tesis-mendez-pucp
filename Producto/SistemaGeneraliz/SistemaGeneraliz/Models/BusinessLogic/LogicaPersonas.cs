﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
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
            Distrito distrito = _sgpFactory.GetDistritoPorId(persona.IdDistrito);
            persona.DireccionCompleta = persona.Direccion + " - " + distrito.NombreDistrito;

            Persona p = new Persona
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
                DireccionCompleta = persona.DireccionCompleta,
                Email1 = persona.Email1,
                Email2 = persona.Email2,
                Telefono1 = persona.Telefono1,
                Telefono2 = persona.Telefono2,
                Telefono3 = persona.Telefono3,
                UltimaActualizacionPersonal = DateTime.Now,
                IsHabilitado = 1, //true
                IsEliminado = 0 //false
            };

            if (persona.ImagenPrincipal != -1)
                p.ImagenId = persona.ImagenPrincipal;

            return p;
        }

        internal Persona CrearObjetoPersonaJuridica(PersonaJuridicaViewModel persona, String tipoUsuario)
        {
            Distrito distrito = _sgpFactory.GetDistritoPorId(persona.IdDistrito);
            persona.DireccionCompleta = persona.Direccion + " - " + distrito.NombreDistrito;

            Persona p = new Persona
            {
                UserName = persona.RUC,
                TipoPersona = "Juridica",
                TipoUsuario = tipoUsuario,
                RUC = Int64.Parse(persona.RUC),
                RazonSocial = persona.RazonSocial,
                FechaCreacion = persona.FechaCreacion,
                DireccionCompleta = persona.DireccionCompleta,
                Email1 = persona.Email1,
                Email2 = persona.Email2,
                Telefono1 = persona.Telefono1,
                Telefono2 = persona.Telefono2,
                Telefono3 = persona.Telefono3,
                UltimaActualizacionPersonal = DateTime.Now,
                IsHabilitado = 1, //true
                IsEliminado = 0 //false
            };

            if (persona.ImagenPrincipal != -1)
                p.ImagenId = persona.ImagenPrincipal;

            return p;
        }

        public Persona GetPersonaLoggeada(int currentUserId)
        {
            return _sgpFactory.GetPersonaLoggeada(currentUserId);
        }

        public string GetNombrePersonaLoggeada(int currentUserId)
        {
            var nombre = "";
            var persona = this.GetPersonaLoggeada(currentUserId);
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

        internal bool ExisteDNIRUC(string dni, string ruc)
        {
            return _sgpFactory.ExisteDNIRUC(dni, ruc);
        }

        //public Persona GetPersonaPorId(int idPersona)
        //{
        //    return _sgpFactory.GetPersonaPorId(idPersona);
        //}

        public void HabilitarDeshabilitarUsuario(string tipoUsuario, int idUsuario, string nuevoEstado)
        {
            _sgpFactory.HabilitarDeshabilitarUsuario(tipoUsuario, idUsuario, nuevoEstado);
        }

        public Imagen GetImagenPorId(int imagenId)
        {
            return _sgpFactory.GetImagenPorId(imagenId);
        }

        public void Dispose()
        {
            _sgpFactory.Dispose(true);
        }

        public Persona GetPersonaPorUsername(string userName)
        {
            return _sgpFactory.GetPersonaPorUsername(userName);
        }

        public Imagen AgregarFotoPersona(HttpPostedFileBase file)
        {
            Imagen imagen;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                imagen = new Imagen { Data = memoryStream.ToArray(), Nombre = file.FileName, Mime = file.ContentType };
            }
            _sgpFactory.AgregarImagen(imagen);
            return imagen;
        }

        public Persona ModificarObjetoPersonaNatural(PersonaNaturalViewModel persona)
        {
            persona.Sexo = (persona.SexoId == 1) ? "Masculino" : "Femenino";
            Distrito distrito = _sgpFactory.GetDistritoPorId(persona.IdDistrito);
            persona.DireccionCompleta = persona.Direccion + " - " + distrito.NombreDistrito;
            //Obtener persona de la BD
            Persona p = _sgpFactory.GetPersonaPorUsername(persona.DNI);
            p.PrimerNombre = persona.PrimerNombre;
            p.SegundoNombre = persona.SegundoNombre;
            p.ApellidoMaterno = persona.ApellidoMaterno;
            p.ApellidoPaterno = persona.ApellidoPaterno;
            p.FechaNacimiento = persona.FechaNacimiento;
            p.Sexo = persona.Sexo;
            p.DireccionCompleta = persona.DireccionCompleta;
            p.Email1 = persona.Email1;
            p.Email2 = persona.Email2;
            p.Telefono1 = persona.Telefono1;
            p.Telefono2 = persona.Telefono2;
            p.Telefono3 = persona.Telefono3;
            p.UltimaActualizacionPersonal = DateTime.Now;
            if (persona.ImagenPrincipal != -1)
                p.ImagenId = persona.ImagenPrincipal;

            return p;
        }

        public void ActualizarPersona(Persona persona)
        {
            _sgpFactory.ActualizarPersona(persona);
        }

        public Persona ModificarObjetoPersonaJuridico(PersonaJuridicaViewModel persona)
        {
            Distrito distrito = _sgpFactory.GetDistritoPorId(persona.IdDistrito);
            persona.DireccionCompleta = persona.Direccion + " - " + distrito.NombreDistrito;
            //Obtener persona de la BD
            Persona p = _sgpFactory.GetPersonaPorUsername(persona.RUC);
            p.RazonSocial = persona.RazonSocial;
            p.FechaCreacion = persona.FechaCreacion;
            p.DireccionCompleta = persona.DireccionCompleta;
            p.Email1 = persona.Email1;
            p.Email2 = persona.Email2;
            p.Telefono1 = persona.Telefono1;
            p.Telefono2 = persona.Telefono2;
            p.Telefono3 = persona.Telefono3;
            p.UltimaActualizacionPersonal = DateTime.Now;
            if (persona.ImagenPrincipal != -1)
                p.ImagenId = persona.ImagenPrincipal;

            return p;
        }
    }
}