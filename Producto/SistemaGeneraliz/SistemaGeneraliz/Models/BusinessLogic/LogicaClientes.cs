using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;
using SistemaGeneraliz.Models.ViewModels;
using WebMatrix.WebData;

namespace SistemaGeneraliz.Models.BusinessLogic
{
    public class LogicaClientes
    {
        private ISGPFactory _sgpFactory;

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

        public Trabajo AgregarTrabajo(int clienteId, string proveedoresIds, string serviciosIds, string fecha, string ubicacion, double latitud, double longitud, string desc)
        {
            int i = 0;
            string nombreDistrito = this.GetDistritoCercanoLatLong(latitud, longitud).NombreDistrito;

            Trabajo trabajo = new Trabajo
            {
                ClienteId = clienteId,
                Fecha = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Direccion = ubicacion,
                DescripcionCliente = desc,
                NombreDistrito = nombreDistrito
            };
            _sgpFactory.AgregarTrabajo(trabajo);

            Persona cliente = this.GetClientePorId(clienteId).Persona;
            string mensajeEncabezado = "Estimado Proveedor:<br/><br/>El siguiente mensaje generado por el sistema es para informarle que un cliente acaba de contratar sus servicios. <br/>Favor de ponerse en contacto con su cliente.<br/><br/>";
            mensajeEncabezado += "Datos de Cliente:<br/>------------------------<br/>";
            if (cliente.TipoPersona == "Natural")
            {
                mensajeEncabezado += "Nombre:  " + cliente.PrimerNombre + " " + cliente.ApellidoPaterno + " " + cliente.ApellidoPaterno + "<br/>";
                mensajeEncabezado += "DNI:  " + cliente.DNI + "<br/>";
            }
            else
            {
                mensajeEncabezado += "Razón Social:  " + cliente.RazonSocial + "<br/>";
                mensajeEncabezado += "RUC:  " + cliente.RUC + "<br/>";
            }
            mensajeEncabezado += "Teléfonos:  " + cliente.Telefono1 + "  " + cliente.Telefono2 + "  " + cliente.Telefono3 + "<br/>";
            mensajeEncabezado += "Emails:  " + cliente.Email1 + "   " + cliente.Email2 + "<br/><br/>";
            mensajeEncabezado += "Datos del Trabajo<br/>-------------------------<br/>";
            mensajeEncabezado += "Dirección:  " + ubicacion + /*" - " + nombreDistrito +*/ "<br/>";
            mensajeEncabezado += "Descripción:  " + desc + "<br/>";
            mensajeEncabezado += "Fecha:  " + fecha + "<br/>";

            //Agregar TrabajoProveedor
            string[] provIds = proveedoresIds.Split(',');
            string[] servicios = serviciosIds.Split(',');
            foreach (var proveedor in provIds)
            {
                if (proveedor != "")
                {
                    string mensajeCorreo = mensajeEncabezado + "Servicios:  ";

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
                                mensajeCorreo += tipo.NombreServicio + " ";
                            }
                        }
                    }
                    else
                    {
                        if (serviciosProveedor != "")
                        {
                            TipoServicio tipo = _sgpFactory.GetTipoServicioPorId(Convert.ToInt32(serviciosProveedor));
                            trabajoProveedor.TiposServicios.Add(tipo);
                            mensajeCorreo += tipo.NombreServicio + " ";
                        }
                    }
                    mensajeCorreo += "<br/><br/>Saludos,<br/>Sistema Generaliz.";
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
                    LogicaPersonas.EnviarNotificacionXCorreo("c.mendez@pucp.pe", "c.mendez@pucp.pe", "Notificación de Nuevo Trabajo", mensajeCorreo);
                }
                i++;
            }

            //Inhabilitamos al cliente ya que tendrá encuestas pendientes
            _sgpFactory.HabilitarDeshabilitarUsuario("Cliente", trabajo.ClienteId, "Inhabilitado");

            return trabajo;
        }

        private Cliente GetClientePorId(int clienteId)
        {
            return _sgpFactory.GetClientePorId(clienteId);
        }

        private Distrito GetDistritoCercanoLatLong(double latitud, double longitud)
        {
            List<Distrito> distritos = _sgpFactory.GetDistritos();
            double distancia, d, c, a, dLat, dLon, lat1, lat2;
            int R = 6371; // radio en km
            double dMin = Double.MaxValue;
            Distrito distritoMin = null;

            foreach (var distrito in distritos)
            {
                dLat = (distrito.LatitudDefault - latitud) * (Math.PI / 180);
                dLon = (distrito.LongitudDefault - longitud) * (Math.PI / 180);
                lat1 = latitud * (Math.PI / 180);
                lat2 = distrito.LatitudDefault * (Math.PI / 180);

                a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                d = R * c;
                distancia = d;

                if (distancia < dMin)
                {
                    dMin = distancia;
                    distritoMin = distrito;
                }
            }
            return distritoMin;
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
                else
                {
                    listaFiltrada = listaProveedores;
                }

                if (listaFiltrada.Count != 0)
                {
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
                                serviciosIdsProveedor += t.TipoServicioId + " - ";
                            }
                        }
                        serviciosProveedor = serviciosProveedor.Substring(0, serviciosProveedor.Length - 3);
                        serviciosIdsProveedor = serviciosIdsProveedor.Substring(0, serviciosIdsProveedor.Length - 3);

                        ProveedorBusquedaViewModel proveedorViewModel = new ProveedorBusquedaViewModel
                        {
                            ProveedorId = prov.ProveedorId,
                            Puntaje = prov.PuntuacionPromedio,
                            FotoId = (int)prov.Persona.ImagenId,
                            NombreCompleto = nombreProveedor,
                            TipoDocumento = tipoDocumento,
                            Documento = prov.Persona.UserName,
                            Servicio = serviciosProveedor,
                            ServicioId = serviciosIdsProveedor,
                            Descripcion = prov.AcercaDeMi,
                            Telefono1 = prov.Persona.Telefono1 ?? "",
                            Telefono2 = prov.Persona.Telefono2 ?? "",
                            Telefono3 = prov.Persona.Telefono3 ?? "",
                            Email1 = prov.Persona.Email1 ?? "",
                            Email2 = prov.Persona.Email2 ?? "",
                            NroRecomendaciones = prov.NroRecomendaciones + "/" + prov.TrabajosProveedores.Count,
                            NroVolveriaContratarlo = prov.NroVolveriaContratarlo + "/" + prov.TrabajosProveedores.Count,
                        };
                        lista.Add(proveedorViewModel);
                    }

                    lista.Sort((x, y) => string.Compare(x.NombreCompleto, y.NombreCompleto));
                }
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
                        ClienteId = trabajo.Trabajo.ClienteId,
                        TrabajoProveedorId = trabajo.TrabajoProveedorId,
                        EncuestaClienteId = (int)trabajo.EncuestaClienteId,
                        FechaTrabajo = fechaTrabajo.ToString("dd/MM/yyyy"),
                        FotoProveedor = (int)trabajo.Proveedor.Persona.ImagenId,
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

        public void EnviarEncuestaCliente(int clienteId, int encuestaId, int trabajoProveedorId, string respuestas, string comentarios)
        {
            Persona cliente = this.GetClientePorId(clienteId).Persona;
            TrabajoProveedor trabajo = _sgpFactory.GetTrabajoProveedor(trabajoProveedorId);
            string mensajeCorreo = "Estimado Proveedor:<br/><br/>El siguiente mensaje generado por el sistema es para informarle que un cliente acaba de responder una encuesta de satisfacción por los servicios recibidos de usted. <br/><br/>";
            mensajeCorreo += "Datos de Cliente:<br/>------------------------<br/>";
            if (cliente.TipoPersona == "Natural")
            {
                mensajeCorreo += "Nombre:  " + cliente.PrimerNombre + " " + cliente.ApellidoPaterno + " " + cliente.ApellidoPaterno + "<br/>";
                mensajeCorreo += "DNI:  " + cliente.DNI + "<br/>";
            }
            else
            {
                mensajeCorreo += "Razón Social:  " + cliente.RazonSocial + "<br/>";
                mensajeCorreo += "RUC:  " + cliente.RUC + "<br/>";
            }
            mensajeCorreo += "Teléfonos:  " + cliente.Telefono1 + "  " + cliente.Telefono2 + "  " + cliente.Telefono3 + "<br/>";
            mensajeCorreo += "Emails:  " + cliente.Email1 + "   " + cliente.Email2 + "<br/><br/>";
            mensajeCorreo += "Datos del Trabajo<br/>---------------------------<br/>";
            mensajeCorreo += "Dirección:  " + trabajo.Trabajo.Direccion + /*" - " + nombreDistrito +*/ "<br/>";
            mensajeCorreo += "Descripción:  " + trabajo.Trabajo.DescripcionCliente + "<br/>";
            mensajeCorreo += "Fecha:  " + trabajo.Trabajo.Fecha.ToString("dd/MM/yyyy") + "<br/><br/>";
            mensajeCorreo += "Datos de la Encuesta<br/>---------------------------<br/>";

            List<RespuestaPorCriterio> listaRespuestas = new List<RespuestaPorCriterio>();
            string[] respuestasSplit = respuestas.Split(',');
            string[] criterios = { "Calidad", "Compromiso", "Trato y Cortesía", "Puntualidad", "Precio cobrado", "Volvería a contratarlo", "Recomendaría" };
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
                    string rpta = "";
                    if (t < 5)
                    {
                        rpta = respuestasSplit[t];
                    }
                    else
                    {
                        rpta = (respuestasSplit[t] == "1") ? "Sí" : "No";
                    }
                    mensajeCorreo += criterios[t] + ":  " + rpta + "<br/>";
                }
            }
            mensajeCorreo += "Comentarios:  " + comentarios + "<br/><br/>";
            _sgpFactory.AgregarRespuestasEncuesta(listaRespuestas);

            EncuestaCliente encuesta = _sgpFactory.GetEncuestaCliente(encuestaId);
            encuesta.Fecha = DateTime.Now;
            double puntuacion = (listaRespuestas.Sum(r => r.PuntajeOtorgado) * 20.0) / (5 * 5 + 2 * 1); //asumimos 27 total
            encuesta.PuntajeTotal = Convert.ToInt32(Math.Round(puntuacion, MidpointRounding.ToEven));
            encuesta.ComentariosCliente = comentarios;
            encuesta.IsCompletada = 1;
            encuesta.IsVisible = 1;
            _sgpFactory.ActualizarEncuestaCompletada(encuesta);
            Proveedor proveedor = trabajo.Proveedor;
            int nroEncuestasCompletadas = proveedor.TrabajosProveedores.Count(t => t.EncuestaCliente.IsCompletada == 1);
            double nuevaPuntuacion = (proveedor.PuntuacionPromedio * nroEncuestasCompletadas + puntuacion) / (nroEncuestasCompletadas + 1);
            proveedor.PuntuacionPromedio = nuevaPuntuacion; //Convert.ToInt32(Math.Round(nuevaPuntuacion, MidpointRounding.ToEven)); <-- si usaramos enteros
            proveedor.NroRecomendaciones += Int32.Parse(respuestasSplit[6]);
            proveedor.NroVolveriaContratarlo += Int32.Parse(respuestasSplit[5]);
            proveedor.NroComentarios += 1;
            //proveedor.NroTrabajosterminados++
            //proveedor.nrocalifaciones++
            _sgpFactory.ActualizarProveedor(proveedor);
            mensajeCorreo += "Su nueva puntuación promedio es:  " + nuevaPuntuacion.ToString("F") + "<br/>";
            mensajeCorreo += "Si desea puede responder a los comentarios hechos por el cliente, así como modificar la visibilidad de la encuesta." + "<br/><br/>";
            mensajeCorreo += "Saludos,<br/>Sistema Generaliz.";
            //var c = trabajo.Trabajo.ClienteId;
            //Cliente cliente = _sgpFactory.GetClientePorId(clienteId);

            //Si el cliente responde una encuesta, entonces verificamos si ya no tiene más pendientes
            int cantidad = CantidadEncuestasPendientesCliente(clienteId);
            if (cantidad == 0)
            {
                _sgpFactory.HabilitarDeshabilitarUsuario("Cliente", clienteId, "Habilitado");
            }
            LogicaPersonas.EnviarNotificacionXCorreo("c.mendez@pucp.pe", "c.mendez@pucp.pe", "Notificación de Nueva Encuesta Respondida", mensajeCorreo);
        }

        public int CantidadEncuestasPendientesCliente(int clienteId)
        {
            return _sgpFactory.CantidadEncuestasPendientesCliente(clienteId);
        }

        public Object GetClienteViewModel(Cliente cliente, string tipoPersona)
        {
            UbicacionPersona ubicacion = _sgpFactory.GetUbicacionesPersona(cliente.PersonaId)[0]; //solo tienen 1 ubicacion
            if (tipoPersona == "Natural")
            {
                ClienteNaturalViewModel clienteNaturalViewModel = new ClienteNaturalViewModel
                {
                    ClienteId = cliente.ClienteId,
                    DNI = cliente.Persona.DNI.ToString(),
                    RUC = cliente.Persona.RUC.ToString(),
                    PrimerNombre = cliente.Persona.PrimerNombre,
                    SegundoNombre = cliente.Persona.SegundoNombre,
                    ApellidoMaterno = cliente.Persona.ApellidoMaterno,
                    ApellidoPaterno = cliente.Persona.ApellidoPaterno,
                    FechaNacimiento = (DateTime)cliente.Persona.FechaNacimiento,
                    Sexo = cliente.Persona.Sexo,
                    SexoId = (cliente.Persona.Sexo == "Masculino") ? 1 : 2,
                    DireccionCompleta = cliente.Persona.DireccionCompleta,
                    IdDistrito = ubicacion.DistritoId,
                    IdCiudad = ubicacion.Distrito.PaisCiudadId,
                    Direccion = ubicacion.Direccion,
                    Referencia = ubicacion.Referencia,
                    Latitud = ubicacion.Latitud,
                    Longitud = ubicacion.Longitud,
                    Email1 = cliente.Persona.Email1,
                    Email2 = cliente.Persona.Email2,
                    Telefono1 = cliente.Persona.Telefono1,
                    Telefono2 = cliente.Persona.Telefono2,
                    Telefono3 = cliente.Persona.Telefono3,
                    ImagenPrincipal = (int)cliente.Persona.ImagenId,
                    UltimaActualizacionPersonal = DateTime.Now,
                    //OldPassword = "asdasd",
                    Password = "password",
                    ConfirmPassword = "password",
                    IsHabilitado = 1, //true
                    IsEliminado = 0 //false
                };
                return clienteNaturalViewModel;
            }
            if (tipoPersona == "Jurídica")
            {
                ClienteJuridicoViewModel clienteJuridicoViewModel = new ClienteJuridicoViewModel
                {
                    ClienteId = cliente.ClienteId,
                    RUC = cliente.Persona.RUC.ToString(),
                    RazonSocial = cliente.Persona.RazonSocial,
                    FechaCreacion = (DateTime)cliente.Persona.FechaCreacion,
                    DireccionCompleta = cliente.Persona.DireccionCompleta,
                    IdDistrito = ubicacion.DistritoId,
                    IdCiudad = ubicacion.Distrito.PaisCiudadId,
                    Direccion = ubicacion.Direccion,
                    Referencia = ubicacion.Referencia,
                    Latitud = ubicacion.Latitud,
                    Longitud = ubicacion.Longitud,
                    Email1 = cliente.Persona.Email1,
                    Email2 = cliente.Persona.Email2,
                    Telefono1 = cliente.Persona.Telefono1,
                    Telefono2 = cliente.Persona.Telefono2,
                    Telefono3 = cliente.Persona.Telefono3,
                    ImagenPrincipal = (int)cliente.Persona.ImagenId,
                    UltimaActualizacionPersonal = DateTime.Now,
                    //OldPassword = "asdasdasd",
                    Password = "password",
                    ConfirmPassword = "password",
                    IsHabilitado = 1, //true
                    IsEliminado = 0 //false
                };
                return clienteJuridicoViewModel;
            }
            return null;
        }
    }
}