using System;
using System.Collections.Generic;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;
using SistemaGeneraliz.Models.ViewModels;

namespace SistemaGeneraliz.Models.BusinessLogic
{
    public class LogicaProveedores
    {
        private readonly ISGPFactory _sgpFactory;

        public LogicaProveedores()
        {
            _sgpFactory = new SGPFactory();
        }

        public LogicaProveedores(ISGPFactory sgpFactory)
        {
            _sgpFactory = sgpFactory;
        }

        internal void AgregarProveedor(Proveedor proveedor)
        {
            _sgpFactory.AgregarProveedor(proveedor);
        }

        internal Proveedor CrearObjetoProveedorNatural(ProveedorNaturalViewModel proveedor)
        {
            //proveedor.Sexo = (proveedor.SexoId == 1) ? "Masculino" : "Femenino";

            return new Proveedor
            {
                LeadsDisponibles = 2, //Configuracion.LeadsGratisRegistro
                //Especialidad = proveedor.Especialidad, //setear esto desde la logica
                PuntuacionPromedio = 0,
                NroTrabajosTerminados = 0,
                NroBusquedasCliente = 0,
                NroClicksVisita = 0,
                NroComentarios = 0,
                NroCalificaciones = 0,
                NroRecomendaciones = 0,
                NroVolveriaContratarlo = 0,
                PaginaWeb = proveedor.PaginaWeb,
                Facebook = proveedor.Facebook,
                AcercaDeMi = proveedor.AcercaDeMi,
                IsDestacado = 0 //false
            };
        }

        internal Proveedor CrearObjetoProveedorJuridico(ProveedorJuridicoViewModel proveedor)
        {
            return new Proveedor
            {
                LeadsDisponibles = 2, //Configuracion.LeadsGratisRegistro
                //Especialidad = proveedor.Especialidad, //setear esto desde la logica
                PuntuacionPromedio = 0,
                NroTrabajosTerminados = 0,
                NroBusquedasCliente = 0,
                NroClicksVisita = 0,
                NroComentarios = 0,
                NroCalificaciones = 0,
                NroRecomendaciones = 0,
                NroVolveriaContratarlo = 0,
                PaginaWeb = proveedor.PaginaWeb,
                Facebook = proveedor.Facebook,
                AcercaDeMi = proveedor.AcercaDeMi,
                IsDestacado = 0 //false
            };
        }

        internal List<TipoServicio> GetTipoServicios()
        {
            return _sgpFactory.GetTipoServicios();
        }

        internal TipoServicio GetTipoServicioPorId(int tipoServicioId)
        {
            return _sgpFactory.GetTipoServicioPorId(tipoServicioId);
        }

        public Proveedor GetProveedorPorDocumento(long documento, int opcionDocumento)
        {
            return _sgpFactory.GetProveedorPorDocumento(documento, opcionDocumento);
        }

        public List<TipoServicio> GetServiciosPorIds(int[] serviciosIds)
        {
            return _sgpFactory.GetServiciosPorIds(serviciosIds);
        }

        public List<Proveedor> GetProveedoresPorServicio(TipoServicio servicio, int cantidadMaxima, int puntajeMinimo)
        {
            return _sgpFactory.GetProveedoresPorServicio(servicio, cantidadMaxima, puntajeMinimo);
        }
    }
}