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
            return new Proveedor
            {
                LeadsDisponibles = _sgpFactory.GetLeadsGratisRegistro(),
                //Especialidad = proveedor.Especialidad, //setear esto desde la logica
                PuntuacionPromedio = _sgpFactory.GetPuntuacionPromedioInicial() * 1.0,
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
                LeadsDisponibles = _sgpFactory.GetLeadsGratisRegistro(),
                //Especialidad = proveedor.Especialidad, //setear esto desde la logica
                PuntuacionPromedio = _sgpFactory.GetPuntuacionPromedioInicial() * 1.0,
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
                    string documentoCliente = (trabajo.Trabajo.Cliente.Persona.DNI != null) ? ("DNI - " + trabajo.Trabajo.Cliente.Persona.DNI.ToString()) : ("RUC - " + trabajo.Proveedor.Persona.RUC.ToString());
                    string servicios = trabajo.TiposServicios.Aggregate("", (current, servicio) => current + (servicio.NombreServicio + " - "));
                    servicios = servicios.Substring(0, servicios.Length - 3);
                    string puntuacion = "-";
                    //if ((trabajo.EncuestaCliente != null) && (trabajo.EncuestaClienteId != null) && (trabajo.EncuestaClienteId > 0) && (trabajo.EncuestaCliente.PuntajeTotal != -1))
                    if (trabajo.EncuestaCliente.PuntajeTotal != -1)
                        puntuacion = trabajo.EncuestaCliente.PuntajeTotal.ToString();
                    string comentarios = "-";
                    //if ((trabajo.EncuestaCliente != null) && (trabajo.EncuestaClienteId != null) && (trabajo.EncuestaClienteId > 0) && (!String.IsNullOrEmpty(trabajo.EncuestaCliente.ComentariosCliente)))
                    if (!String.IsNullOrEmpty(trabajo.EncuestaCliente.ComentariosCliente))
                        comentarios = trabajo.EncuestaCliente.ComentariosCliente;
                    string rph_factura = "-";
                    if (trabajo.TipoRpH_Factura != null)
                        rph_factura += trabajo.TipoRpH_Factura + " ";
                    if (trabajo.NroRpH_Factura != null)
                        rph_factura += trabajo.NroRpH_Factura;
                    string montoCobrado = "";
                    if (trabajo.MontoCobrado != null)
                        montoCobrado = "S/. " + trabajo.MontoCobrado.ToString();

                    HistorialTrabajosViewModel his = new HistorialTrabajosViewModel
                    {
                        TrabajoProveedorId = trabajo.TrabajoProveedorId,
                        FechaTrabajo = fechaTrabajo.ToString("dd/MM/yyyy"),
                        Puntuacion = puntuacion, //PARA CUANDO ESTE LO DE ENCUESTAS
                        NombreCliente = nombreCliente,
                        DocumentoCliente = documentoCliente,
                        Servicios = servicios,
                        DescripcionCliente = trabajo.Trabajo.DescripcionCliente,
                        ReciboHonorarios_Factura = rph_factura,
                        MontoCobrado = montoCobrado,
                        LinkModificarDetalles = "", //AQUI IRA LINK PARA MODIFICAR DETALLES DE TRABAJO
                        EncuestaRespondida = trabajo.EncuestaCliente.IsCompletada,
                        Comentarios = comentarios
                    };
                    listaHistorialTrabajosViewModel.Add(his);
                }
                //listaHistorialTrabajosViewModel.Sort((x, y) => string.Compare(y.FechaTrabajo, x.FechaTrabajo));
            }
            return listaHistorialTrabajosViewModel;
        }

        public TrabajoProveedor GetTrabajoProveedor(int trabajoProveedorId)
        {
            return _sgpFactory.GetTrabajoProveedor(trabajoProveedorId);
        }

        public void ActualizarDetallesTrabajoProveedor(TrabajoProveedor trabajoProveedor)
        {
            TrabajoProveedor trabajo = GetTrabajoProveedor(trabajoProveedor.TrabajoProveedorId);
            trabajo.FechaReal = trabajoProveedor.FechaReal;
            trabajo.DescripcionProveedor = trabajoProveedor.DescripcionProveedor;
            trabajo.TipoRpH_Factura = trabajoProveedor.TipoRpH_Factura;
            trabajo.NroRpH_Factura = trabajoProveedor.NroRpH_Factura;
            trabajo.MontoCobrado = trabajoProveedor.MontoCobrado;
            trabajo.IsTerminado = trabajoProveedor.IsTerminado;
            _sgpFactory.ActualizarDetallesTrabajoProveedor(trabajo);
        }

        public void ActualizarComentarioProveedorEncuesta(int trabajoProveedorId, string comentariosProveedor, int visibilidad)
        {
            TrabajoProveedor trabajoProveedor = this.GetTrabajoProveedor(trabajoProveedorId);
            EncuestaCliente encuesta = trabajoProveedor.EncuestaCliente;
            encuesta.ComentariosProveedor = comentariosProveedor;
            encuesta.IsVisible = visibilidad;
            _sgpFactory.ActualizarEncuestaCompletada(encuesta);
        }
    }
}