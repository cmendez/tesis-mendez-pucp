using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;
using SistemaGeneraliz.Models.ViewModels;

namespace SistemaGeneraliz.Models.BusinessLogic
{
    public class LogicaUbicaciones
    {
        private readonly ISGPFactory _sgpFactory;

        public LogicaUbicaciones()
        {
            _sgpFactory = new SGPFactory();
        }

        public LogicaUbicaciones(ISGPFactory sgpFactory)
        {
            _sgpFactory = sgpFactory;
        }

        internal void AgregarUbicacion(UbicacionPersona ubicacion)
        {
            _sgpFactory.AgregarUbicacion(ubicacion);
        }

        internal UbicacionPersona CrearObjetoUbicacionPersonaNatural(PersonaNaturalViewModel proveedor, Persona persona)
        {
            return new UbicacionPersona
            {
                PersonaId = persona.PersonaId,
                DistritoId = proveedor.IdDistrito,
                Direccion = proveedor.Direccion,
                Referencia = proveedor.Referencia,
                Latitud = proveedor.Latitud,
                Longitud = proveedor.Longitud,
                IsVisible = 1,
                IsEliminado = 0
            };
        }

        internal UbicacionPersona CrearObjetoUbicacionPersonaJuridica(PersonaJuridicaViewModel proveedor, Persona persona)
        {
            return new UbicacionPersona
            {
                PersonaId = persona.PersonaId,
                DistritoId = proveedor.IdDistrito,
                Direccion = proveedor.Direccion,
                Referencia = proveedor.Referencia,
                Latitud = proveedor.Latitud,
                Longitud = proveedor.Longitud,
                IsVisible = 1,
                IsEliminado = 0
            };
        }

        public UbicacionPersona GetPrimeraUbicacionPersona(int idPersona)
        {
            return _sgpFactory.GetPrimeraUbicacionPersona(idPersona);
        }

        public void ActualizarUbicacion(UbicacionPersona ubicacion)
        {
            _sgpFactory.ActualizarUbicacion(ubicacion);
        }

        public UbicacionPersona ModificarObjetoUbicacionPersonaNatural(PersonaNaturalViewModel personaNaturalViewModel, Persona persona)
        {
            UbicacionPersona ubicacion = _sgpFactory.GetUbicacionesPersona(persona.PersonaId)[0];
            ubicacion.DistritoId = personaNaturalViewModel.IdDistrito;
            ubicacion.Direccion = personaNaturalViewModel.Direccion;
            ubicacion.Referencia = personaNaturalViewModel.Referencia;
            ubicacion.Latitud = personaNaturalViewModel.Latitud;
            ubicacion.Longitud = personaNaturalViewModel.Longitud;
            return ubicacion;
        }

        public UbicacionPersona ModificarObjetoUbicacionPersonaJuridica(PersonaJuridicaViewModel personaJuridicoViewModel, Persona persona)
        {
            UbicacionPersona ubicacion = _sgpFactory.GetUbicacionesPersona(persona.PersonaId)[0];
            ubicacion.DistritoId = personaJuridicoViewModel.IdDistrito;
            ubicacion.Direccion = personaJuridicoViewModel.Direccion;
            ubicacion.Referencia = personaJuridicoViewModel.Referencia;
            ubicacion.Latitud = personaJuridicoViewModel.Latitud;
            ubicacion.Longitud = personaJuridicoViewModel.Longitud;
            return ubicacion;
        }
    }
}