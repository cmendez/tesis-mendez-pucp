using System;
using System.Collections.Generic;
using System.Linq;
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

        public int GetCantidadMaximaProveedoresConfiguracion()
        {
            return _sgpFactory.GetCantidadMaximaProveedoresConfiguracion();
        }

        public int GetPuntajeMinimoConfiguracion()
        {
            return _sgpFactory.GetPuntajeMinimoConfiguracion();
        }

        public Proveedor GetProveedorPorPersonaId(int idPersona)
        {
            return _sgpFactory.GetProveedorPorPersonaId(idPersona);
        }

        public List<HistorialTrabajosViewModel> GetHistorialTrabajos(int proveedorId)
        {
            var listaHistorialTrabajosViewModel = new List<HistorialTrabajosViewModel>();
            var listaTrabajos = _sgpFactory.GetHistorialTrabajos(proveedorId);

            if ((listaTrabajos != null) && (listaTrabajos.Count > 0))
            {
                foreach (var trabajo in listaTrabajos)
                {
                    DateTime fechaTrabajo = trabajo.FechaReal ?? trabajo.Trabajo.Fecha;
                    string nombreCliente = trabajo.Trabajo.Cliente.Persona.RazonSocial ??
                                           (trabajo.Trabajo.Cliente.Persona.PrimerNombre + " " +
                                            trabajo.Trabajo.Cliente.Persona.ApellidoPaterno);

                    string servicios = trabajo.TiposServicios.Aggregate("",
                                                                        (current, servicio) =>
                                                                        current + (servicio.NombreServicio + " - "));
                    servicios = servicios.Substring(0, servicios.Length - 3);

                    HistorialTrabajosViewModel his = new HistorialTrabajosViewModel
                    {
                        FechaTrabajo = fechaTrabajo.ToString("dd/MM/yyyy"),
                        //Puntuación = trabajo.EncuestaCliente.PuntuacionTotal.ToString(), //PARA CUANDO ESTE LO DE ENCUESTAS
                        NombreCliente = nombreCliente,
                        Servicios = servicios,
                        DescripcionCliente = trabajo.Trabajo.DescripcionCliente,
                        ReciboHonorarios_Factura = trabajo.TipoRpH_Factura + " " + trabajo.NroRpH_Factura,
                        MontoCobrado = "S/. " + trabajo.MontoCobrado.ToString(),
                        LinkModificarDetalles = "" //AQUI IRA LINK PARA MODIFICAR DETALLES DE TRABAJO
                    };
                    listaHistorialTrabajosViewModel.Add(his);
                }
                listaHistorialTrabajosViewModel.Sort((x, y) => string.Compare(y.FechaTrabajo, x.FechaTrabajo));
            }
            return listaHistorialTrabajosViewModel;
        }
    }
}