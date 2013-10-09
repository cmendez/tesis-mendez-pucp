using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;
using SistemaGeneraliz.Models.ViewModels;

namespace SistemaGeneraliz.Models.BusinessLogic
{
    public class LogicaClientes
    {
        private readonly ISGPFactory _sgpFactory;

        public LogicaClientes()
        {
            _sgpFactory = new SGPFactory();
        }

        public LogicaClientes(ISGPFactory sgpFactory)
        {
            _sgpFactory = sgpFactory;
        }

        internal void AgregarCliente(Cliente Cliente)
        {
            _sgpFactory.AgregarCliente(Cliente);
        }

        internal Cliente CrearObjetoClienteNatural(ClienteNaturalViewModel cliente)
        {
            return new Cliente
            {
            };
        }

        internal Cliente CrearObjetoClienteJuridico(ClienteJuridicoViewModel cliente)
        {
            return new Cliente
            {
            };
        }

        public Cliente GetClientePorPersonaId(int idPersona)
        {
            return _sgpFactory.GetClientePorPersonaId(idPersona);
        }

        public List<ProveedorBusquedaViewModel> EjecutarAlgoritmoTabu(string valueServicios, double latitud, double longitud)
        {
            try
            {
                string[] serviciosIds = valueServicios.Split(',');
                int[] servicios = new int[serviciosIds.Length];
                for (int i = 0; i < serviciosIds.Length; i++)
                {
                    servicios[i] = Int32.Parse(serviciosIds[i]);
                }

                AlgoritmoTabu algoritmo = new AlgoritmoTabu(servicios, latitud, longitud);
                return algoritmo.EjecutarAlgoritmoTabu();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Trabajo AgregarTrabajo(int clienteId, string proveedoresIds, string serviciosIds, string fecha, string ubicacion, string desc)
        {
            int i = 0;

            Trabajo trabajo = new Trabajo
            {
                ClienteId = clienteId,
                Fecha = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Direccion = ubicacion,
                DescripcionCliente = desc,
                IsTerminado = 0
            };

            _sgpFactory.AgregarTrabajo(trabajo);

            //Agregar TrabajoProveedor
            string[] provIds = proveedoresIds.Split(',');
            string[] servicios = serviciosIds.Split(',');
            foreach (var proveedor in provIds)
            {
                if (proveedor != "")
                {
                    TrabajoProveedor trabajoProveedor = new TrabajoProveedor
                    {
                        ProveedorId = Int32.Parse(proveedor),
                        TrabajoId = trabajo.TrabajoId,
                        TiposServicios = new List<TipoServicio>()
                    };

                    //Agregar TiposServiciosXTrabajosProveedores
                    string serviciosProveedor = servicios[i];
                    if (serviciosProveedor.IndexOf("-") > 0) //el proveedor ejecutará más de 1 servicio
                    {
                        string[] idsServicios = serviciosProveedor.Split('-');
                        foreach (var idServ in idsServicios)
                        {
                            if (idServ != "")
                            {
                                TipoServicio tipo = _sgpFactory.GetTipoServicioPorId(Convert.ToInt32(idServ));
                                trabajoProveedor.TiposServicios.Add(tipo);
                            }
                        }
                    }
                    else
                    {
                        if (serviciosProveedor != "")
                        {
                            TipoServicio tipo = _sgpFactory.GetTipoServicioPorId(Convert.ToInt32(serviciosProveedor));
                            trabajoProveedor.TiposServicios.Add(tipo);
                        }
                    }

                    _sgpFactory.AgregarTrabajoProveedor(trabajoProveedor);
                    EncuestaCliente encuesta = new EncuestaCliente
                    {
                        Fecha = trabajoProveedor.Trabajo.Fecha,
                        PuntajeTotal = -1,
                        IsVisible = 0,
                        IsCompletada = 0,
                        IsEliminado = 0,
                    };
                    _sgpFactory.AgregarEncuestaCliente(encuesta);
                    trabajoProveedor.EncuestaClienteId = encuesta.EncuestaClienteId;
                    _sgpFactory.ActualizarEncuestaIdTrabajoProveedor(trabajoProveedor);
                    _sgpFactory.ConsumirLeadsProveedor(Int32.Parse(proveedor), 1);
                }
                i++;
            }

            return trabajo;
        }

        public List<ProveedorBusquedaViewModel> BusquedaManualProveedores(string nombre, string valueServicios)
        {
            try
            {
                List<ProveedorBusquedaViewModel> lista = new List<ProveedorBusquedaViewModel>();

                string[] serviciosIds = valueServicios.Split(',');
                int[] servicios = new int[serviciosIds.Length];

                for (int t = 0; t < serviciosIds.Length; t++)
                {
                    servicios[t] = Int32.Parse(serviciosIds[t]);
                }

                List<Proveedor> listaProveedores = _sgpFactory.GetProveedoresServicios(servicios);
                List<Proveedor> listaFiltrada = new List<Proveedor>();
                if (!String.IsNullOrEmpty(nombre))
                {
                    foreach (var p in listaProveedores)
                    {
                        string nombreCompleto = "";
                        if (!String.IsNullOrEmpty(p.Persona.PrimerNombre))
                            nombreCompleto += p.Persona.PrimerNombre + " ";
                        if (!String.IsNullOrEmpty(p.Persona.ApellidoPaterno))
                            nombreCompleto += p.Persona.ApellidoPaterno + " ";
                        if (!String.IsNullOrEmpty(p.Persona.RazonSocial))
                            nombreCompleto += p.Persona.RazonSocial + " ";

                        if (nombreCompleto.ToUpper().Contains(nombre.Trim().ToUpper()))
                            listaFiltrada.Add(p);
                    }
                }

                if (listaFiltrada.Count == 0)
                    listaFiltrada = listaProveedores;

                foreach (var prov in listaFiltrada)
                {
                    string nombreProveedor = "";
                    string tipoDocumento = "";
                    string serviciosProveedor = "";
                    string serviciosIdsProveedor = "";

                    switch (prov.Persona.TipoPersona)
                    {
                        case "Natural":
                            nombreProveedor = prov.Persona.PrimerNombre + " " + prov.Persona.ApellidoPaterno;
                            tipoDocumento = "DNI";
                            break;

                        case "Juridica":
                            nombreProveedor = prov.Persona.RazonSocial;
                            tipoDocumento = "RUC";
                            break;
                    }

                    foreach (var serv in servicios)
                    {
                        var t = prov.TiposServicios.FirstOrDefault(s => s.TipoServicioId == serv);
                        if (t != null)
                        {
                            serviciosProveedor += t.NombreServicio + " - ";
                            serviciosIdsProveedor += t.TipoServicioId + "-";
                        }
                    }
                    serviciosIdsProveedor = serviciosIdsProveedor.Substring(0, serviciosIdsProveedor.Length - 3);

                    ProveedorBusquedaViewModel proveedorViewModel = new ProveedorBusquedaViewModel
                    {
                        ProveedorId = prov.ProveedorId,
                        Puntaje = Convert.ToInt32(prov.PuntuacionPromedio).ToString(),
                        RutaFoto = "", //AQUI IRA LA URL DE LA FOTO
                        NombreCompleto = nombreProveedor,
                        TipoDocumento = tipoDocumento,
                        Documento = prov.Persona.UserName,
                        Servicio = serviciosProveedor,
                        ServicioId = serviciosIdsProveedor,
                        Descripcion = prov.AcercaDeMi,
                        VerTrabajos = "", //AQUI IRA LINK DE TRABAJOS 
                        VerComentarios = "", //AQUI IRA LINK DE COMENTARIOS 
                        Telefono1 = prov.Persona.Telefono1 ?? "",
                        Telefono2 = prov.Persona.Telefono2 ?? "",
                        Telefono3 = prov.Persona.Telefono3 ?? "",
                        Email1 = prov.Persona.Email1 ?? "",
                        Email2 = prov.Persona.Email2 ?? "",
                    };
                    lista.Add(proveedorViewModel);
                }

                lista.Sort((x, y) => string.Compare(x.NombreCompleto, y.NombreCompleto));
                return lista;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<EncuestasClientesViewModel> GetEncuestasPendientes(int clienteId)
        {
            var listaEncuestasClientesViewModel = new List<EncuestasClientesViewModel>();
            var listaTrabajos = _sgpFactory.GetTrabajosConEncuestasPendientes(clienteId);

            if ((listaTrabajos != null) && (listaTrabajos.Count > 0))
            {
                foreach (var trabajo in listaTrabajos)
                {
                    DateTime fechaTrabajo = trabajo.FechaReal ?? trabajo.Trabajo.Fecha;
                    string nombreProveedor = trabajo.Proveedor.Persona.RazonSocial ??
                                           (trabajo.Proveedor.Persona.PrimerNombre + " " +
                                            trabajo.Proveedor.Persona.ApellidoPaterno);
                    string documentoProveedor = (trabajo.Proveedor.Persona.DNI != null) ? ("DNI - " + trabajo.Proveedor.Persona.DNI.ToString()) : ("RUC - " + trabajo.Proveedor.Persona.RUC.ToString());
                    string servicios = trabajo.TiposServicios.Aggregate("", (current, servicio) => current + (servicio.NombreServicio + " - "));
                    servicios = servicios.Substring(0, servicios.Length - 3);

                    EncuestasClientesViewModel his = new EncuestasClientesViewModel
                    {
                        TrabajoProveedorId = trabajo.TrabajoProveedorId,
                        EncuestaClienteId = (int)trabajo.EncuestaClienteId,
                        FechaTrabajo = fechaTrabajo.ToString("dd/MM/yyyy"),
                        FotoProveedor = trabajo.Proveedor.Persona.ImagenPrincipal,
                        NombreProveedor = nombreProveedor,
                        DocumentoProveedor = documentoProveedor,
                        Servicios = servicios,
                        DescripcionCliente = trabajo.Trabajo.DescripcionCliente,
                        DireccionCiente = trabajo.Trabajo.Cliente.Persona.DireccionCompleta
                    };
                    listaEncuestasClientesViewModel.Add(his);
                }
                listaEncuestasClientesViewModel.Sort((x, y) => string.Compare(y.FechaTrabajo, x.FechaTrabajo));
            }
            return listaEncuestasClientesViewModel;
        }

        public List<CriterioCalificacion> GetCriteriosEncuestas()
        {
            return _sgpFactory.GetCriteriosEncuestas();
        }

        public void EnviarEncuestaCliente(int encuestaId, int trabajoProveedorId, string respuestas, string comentarios)
        {
            List<RespuestaPorCriterio> listaRespuestas = new List<RespuestaPorCriterio>();
            string[] respuestasSplit = respuestas.Split(',');
            for (int t = 0; t < respuestasSplit.Length; t++)
            {
                if (respuestasSplit[t] != "")
                {
                    RespuestaPorCriterio respuesta = new RespuestaPorCriterio
                    {
                        EncuestaClienteId = encuestaId,
                        CriterioCalificacionId = t + 1, //asumimos un orden de criterios en la bd
                        PuntajeOtorgado = Int32.Parse(respuestasSplit[t])
                    };
                    listaRespuestas.Add(respuesta);
                }
            }
            _sgpFactory.AgregarRespuestasEncuesta(listaRespuestas);

            EncuestaCliente encuesta = _sgpFactory.GetEncuestaCliente(encuestaId);
            encuesta.Fecha = DateTime.Now;
            double puntuacion = (listaRespuestas.Sum(r => r.PuntajeOtorgado) * 20.0) / (5 * 5 + 2 * 1); //asumimos 27 total
            encuesta.PuntajeTotal = Convert.ToInt32(Math.Round(puntuacion, MidpointRounding.ToEven));
            encuesta.ComentariosCliente = comentarios;
            encuesta.IsCompletada = 1;
            encuesta.IsVisible = 1;
            _sgpFactory.ActualizarEncuestaCompletada(encuesta);
        }
    }
}