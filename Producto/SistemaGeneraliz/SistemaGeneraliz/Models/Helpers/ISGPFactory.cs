﻿using System;
using System.Collections.Generic;
using System.Linq;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.Helpers
{
    public interface ISGPFactory
    {
        void Dispose(bool disposing);
        void SetEntryState(Object res, System.Data.EntityState entityState);

        #region Personas
        void AgregarPersona(Persona persona);
        Persona GetPersonaLoggeada(int currentUserId);
        List<Distrito> GetDistritos();
        void AgregarUbicacion(UbicacionPersona ubicacion);
        bool ExisteDNIRUC(string dni, string ruc);
        Distrito GetDistritoPorId(int distritoId);
        UbicacionPersona GetPrimeraUbicacionPersona(int idPersona);
        #endregion

        #region Proveedores
        void AgregarProveedor(Proveedor proveedor);
        List<TipoServicio> GetTipoServicios();
        TipoServicio GetTipoServicioPorId(int tipoServicioId);
        Proveedor GetProveedorPorDocumento(long documento, int opcionDocumento);
        List<TipoServicio> GetServiciosPorIds(int[] serviciosIds);
        List<Proveedor> GetProveedoresPorServicio(TipoServicio servicio, int cantidadMaxima, int puntajeMinimo);
        int GetCantidadMaximaProveedoresConfiguracion();
        int GetPuntajeMinimoConfiguracion();
        #endregion

        #region Clientes
        void AgregarCliente(Cliente cliente);
        Cliente GetClientePorPersonaId(int idPersona);
        #endregion

        #region Suministradores
        void AgregarSuministrador(Suministrador suministrador);
        Suministrador GetSuministradorPorPersonaId(int personaId);
        List<RecargaLeads> GetListaRecargasSuministrador(int suministradorId);
        void AgregarRecarga(RecargaLeads recarga);
        Suministrador GetSuministrador(int idSuministrador);
        void ActualizarSuministrador(Suministrador suministrador);
        #endregion

        
    }
}
