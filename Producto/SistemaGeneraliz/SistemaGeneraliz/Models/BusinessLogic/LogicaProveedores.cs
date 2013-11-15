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

        public List<HistorialTrabajosViewModel> GetHistorialTrabajos(int proveedorId, string filtro)
        {
            var listaHistorialTrabajosViewModel = new List<HistorialTrabajosViewModel>();
            var listaTrabajos = _sgpFactory.GetHistorialTrabajos(proveedorId);

            if ((listaTrabajos != null) && (listaTrabajos.Count > 0))
            {
                foreach (var trabajo in listaTrabajos)
                {
                    if ((filtro == "Todos") || ((filtro == "SoloVisibles") && (trabajo.EncuestaCliente.IsVisible == 1)))
                    {
                        DateTime fechaTrabajo = trabajo.FechaReal ?? trabajo.Trabajo.Fecha;
                        string nombreCliente = trabajo.Trabajo.Cliente.Persona.RazonSocial ??
                                               (trabajo.Trabajo.Cliente.Persona.PrimerNombre + " " +
                                                trabajo.Trabajo.Cliente.Persona.ApellidoPaterno);
                        string documentoCliente = (trabajo.Trabajo.Cliente.Persona.DNI != null)
                                                      ? ("DNI - " + trabajo.Trabajo.Cliente.Persona.DNI.ToString())
                                                      : ("RUC - " + trabajo.Trabajo.Cliente.Persona.RUC.ToString());
                        string servicios = trabajo.TiposServicios.Aggregate("",
                                                                            (current, servicio) =>
                                                                            current + (servicio.NombreServicio + " - "));
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
                        if ((trabajo.TipoRpH_Factura != null) && (trabajo.NroRpH_Factura != null))
                        {
                            rph_factura = trabajo.TipoRpH_Factura + " - " + trabajo.NroRpH_Factura;
                            ;
                        }
                        string montoCobrado = "-";
                        if (trabajo.MontoCobrado != null)
                        {
                            //if (trabajo.MontoCobrado.IndexOf("S/.") < 0)
                            //{
                            //    montoCobrado = "S/. " + trabajo.MontoCobrado;    
                            //}
                            //else
                            //{
                            montoCobrado = trabajo.MontoCobrado;
                            //}
                        }

                        HistorialTrabajosViewModel his = new HistorialTrabajosViewModel
                        {
                            TrabajoProveedorId = trabajo.TrabajoProveedorId,
                            FechaTrabajo = fechaTrabajo.ToString("dd/MM/yyyy"),
                            Puntuacion = puntuacion,
                            //PARA CUANDO ESTE LO DE ENCUESTAS
                            NombreCliente = nombreCliente,
                            DocumentoCliente = documentoCliente,
                            Servicios = servicios,
                            DescripcionCliente = trabajo.Trabajo.DescripcionCliente,
                            ReciboHonorarios_Factura = rph_factura,
                            MontoCobrado = montoCobrado,
                            LinkModificarDetalles = "",
                            //AQUI IRA LINK PARA MODIFICAR DETALLES DE TRABAJO
                            EncuestaRespondida =
                                trabajo.EncuestaCliente.IsCompletada,
                            Comentarios = comentarios
                        };
                        listaHistorialTrabajosViewModel.Add(his);
                    }
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

        public Object GetProveedorViewModel(Proveedor proveedor, string tipoPersona)
        {
            int[] listaServicios = new int[proveedor.TiposServicios.Count];
            int i = 0;
            foreach (var tipoServicio in proveedor.TiposServicios.ToList())
            {
                listaServicios[i] = tipoServicio.TipoServicioId;
                i++;
            }

            UbicacionPersona ubicacion = _sgpFactory.GetUbicacionesPersona(proveedor.PersonaId)[0]; //solo tienen 1 ubicacion
            if (tipoPersona == "Natural")
            {
                ProveedorNaturalViewModel proveedorNaturalViewModel = new ProveedorNaturalViewModel
                {
                    PersonaId = proveedor.PersonaId,
                    ProveedorId = proveedor.ProveedorId,
                    DNI = proveedor.Persona.DNI.ToString(),
                    RUC = proveedor.Persona.RUC.ToString(),
                    PrimerNombre = proveedor.Persona.PrimerNombre,
                    SegundoNombre = proveedor.Persona.SegundoNombre,
                    ApellidoMaterno = proveedor.Persona.ApellidoMaterno,
                    ApellidoPaterno = proveedor.Persona.ApellidoPaterno,
                    FechaNacimiento = (DateTime)proveedor.Persona.FechaNacimiento,
                    Sexo = proveedor.Persona.Sexo,
                    SexoId = (proveedor.Persona.Sexo == "Masculino") ? 1 : 2,
                    DireccionCompleta = proveedor.Persona.DireccionCompleta,
                    IdDistrito = ubicacion.DistritoId,
                    IdCiudad = ubicacion.Distrito.PaisCiudadId,
                    Direccion = ubicacion.Direccion,
                    Referencia = ubicacion.Referencia,
                    Latitud = ubicacion.Latitud,
                    Longitud = ubicacion.Longitud,
                    Email1 = proveedor.Persona.Email1,
                    Email2 = proveedor.Persona.Email2,
                    Telefono1 = proveedor.Persona.Telefono1,
                    Telefono2 = proveedor.Persona.Telefono2,
                    Telefono3 = proveedor.Persona.Telefono3,
                    ImagenPrincipal = (int)proveedor.Persona.ImagenId,
                    UltimaActualizacionPersonal = DateTime.Now,
                    //OldPassword = "asdasd",
                    Password = "password",
                    ConfirmPassword = "password",
                    IsHabilitado = 1, //true
                    IsEliminado = 0, //false
                    PaginaWeb = proveedor.PaginaWeb,
                    Facebook = proveedor.Facebook,
                    AcercaDeMi = proveedor.AcercaDeMi,
                    ListTiposServiciosIds = listaServicios,
                    TiposServicios = proveedor.TiposServicios.ToList()
                };
                return proveedorNaturalViewModel;
            }
            if (tipoPersona == "Jurídica")
            {
                ProveedorJuridicoViewModel proveedorJuridicoViewModel = new ProveedorJuridicoViewModel
                {
                    PersonaId = proveedor.PersonaId,
                    ProveedorId = proveedor.ProveedorId,
                    RUC = proveedor.Persona.RUC.ToString(),
                    RazonSocial = proveedor.Persona.RazonSocial,
                    FechaCreacion = (DateTime)proveedor.Persona.FechaCreacion,
                    DireccionCompleta = proveedor.Persona.DireccionCompleta,
                    IdDistrito = ubicacion.DistritoId,
                    IdCiudad = ubicacion.Distrito.PaisCiudadId,
                    Direccion = ubicacion.Direccion,
                    Referencia = ubicacion.Referencia,
                    Latitud = ubicacion.Latitud,
                    Longitud = ubicacion.Longitud,
                    Email1 = proveedor.Persona.Email1,
                    Email2 = proveedor.Persona.Email2,
                    Telefono1 = proveedor.Persona.Telefono1,
                    Telefono2 = proveedor.Persona.Telefono2,
                    Telefono3 = proveedor.Persona.Telefono3,
                    ImagenPrincipal = (int)proveedor.Persona.ImagenId,
                    UltimaActualizacionPersonal = DateTime.Now,
                    //OldPassword = "asdasdasd",
                    Password = "password",
                    ConfirmPassword = "password",
                    IsHabilitado = 1, //true
                    IsEliminado = 0, //false
                    ListTiposServiciosIds = listaServicios,
                    TiposServicios = proveedor.TiposServicios.ToList(),
                    Facebook = proveedor.Facebook,
                    PaginaWeb = proveedor.PaginaWeb,
                    AcercaDeMi = proveedor.AcercaDeMi
                };
                return proveedorJuridicoViewModel;
            }
            return null;
        }

        public Proveedor ModificarObjetoProveedorNatural(ProveedorNaturalViewModel proveedorNaturalViewModel)
        {
            Proveedor proveedor = _sgpFactory.GetProveedorPorDocumento(Int32.Parse(proveedorNaturalViewModel.DNI), 1);
            proveedor.Facebook = proveedorNaturalViewModel.Facebook;
            proveedor.PaginaWeb = proveedorNaturalViewModel.PaginaWeb;
            proveedor.AcercaDeMi = proveedorNaturalViewModel.AcercaDeMi;

            foreach (var servicio in proveedor.TiposServicios.ToList())
            {
                if (!proveedorNaturalViewModel.ListTiposServiciosIds.Contains(servicio.TipoServicioId))
                    proveedor.TiposServicios.Remove(servicio);
            }

            foreach (var servicioId in proveedorNaturalViewModel.ListTiposServiciosIds)
            {
                if (!proveedor.TiposServicios.Any(r => r.TipoServicioId == servicioId))
                {
                    TipoServicio tipo = this.GetTipoServicioPorId(servicioId);
                    proveedor.TiposServicios.Add(tipo);
                }
            }
            return proveedor;
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            _sgpFactory.ActualizarProveedor(proveedor);
        }

        public Proveedor ModificarObjetoProveedorJuridico(ProveedorJuridicoViewModel proveedorJuridicoViewModel)
        {
            Proveedor proveedor = _sgpFactory.GetProveedorPorDocumento(Int64.Parse(proveedorJuridicoViewModel.RUC), 2);
            proveedor.Facebook = proveedorJuridicoViewModel.Facebook;
            proveedor.PaginaWeb = proveedorJuridicoViewModel.PaginaWeb;
            proveedor.AcercaDeMi = proveedorJuridicoViewModel.AcercaDeMi;

            foreach (var servicio in proveedor.TiposServicios.ToList())
            {
                if (!proveedorJuridicoViewModel.ListTiposServiciosIds.Contains(servicio.TipoServicioId))
                    proveedor.TiposServicios.Remove(servicio);
            }

            foreach (var servicioId in proveedorJuridicoViewModel.ListTiposServiciosIds)
            {
                if (!proveedor.TiposServicios.Any(r => r.TipoServicioId == servicioId))
                {
                    TipoServicio tipo = this.GetTipoServicioPorId(servicioId);
                    proveedor.TiposServicios.Add(tipo);
                }
            }
            return proveedor;
        }

        public List<HistorialTrabajosViewModel> HistoricoTrabajos(string fechaInicio, string fechaFin)
        {
            var listaHistorialTrabajosViewModel = new List<HistorialTrabajosViewModel>();
            var listaTrabajos = _sgpFactory.GetHistoricoTrabajos(fechaInicio, fechaFin);

            if ((listaTrabajos != null) && (listaTrabajos.Count > 0))
            {
                if (!String.IsNullOrEmpty(fechaInicio))
                {
                    DateTime finicio = DateTime.Parse(fechaInicio);
                    listaTrabajos = listaTrabajos.Where(t => t.Trabajo.Fecha.Date.CompareTo(finicio) >= 0).ToList();
                }

                if (!String.IsNullOrEmpty(fechaFin))
                {
                    DateTime ffin = DateTime.Parse(fechaFin);
                    listaTrabajos = listaTrabajos.Where(t => t.Trabajo.Fecha.Date.CompareTo(ffin) <= 0).ToList();
                }

                foreach (var trabajo in listaTrabajos)
                {
                    DateTime fechaTrabajo = trabajo.FechaReal ?? trabajo.Trabajo.Fecha;
                    string nombreCliente = trabajo.Trabajo.Cliente.Persona.RazonSocial ??
                                           (trabajo.Trabajo.Cliente.Persona.PrimerNombre + " " +
                                            trabajo.Trabajo.Cliente.Persona.ApellidoPaterno);
                    string documentoCliente = (trabajo.Trabajo.Cliente.Persona.DNI != null)
                                                  ? ("DNI - " + trabajo.Trabajo.Cliente.Persona.DNI.ToString())
                                                  : ("RUC - " + trabajo.Trabajo.Cliente.Persona.RUC.ToString());
                    string servicios = trabajo.TiposServicios.Aggregate("", (current, servicio) =>
                                                                        current + (servicio.NombreServicio + " - "));
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
                    if ((trabajo.TipoRpH_Factura != null) && (trabajo.NroRpH_Factura != null))
                    {
                        rph_factura = trabajo.TipoRpH_Factura + " - " + trabajo.NroRpH_Factura;
                        ;
                    }
                    string montoCobrado = "-";
                    if (trabajo.MontoCobrado != null)
                    {
                        //if (trabajo.MontoCobrado.IndexOf("S/.") < 0)
                        //{
                        //    montoCobrado = "S/. " + trabajo.MontoCobrado;    
                        //}
                        //else
                        //{
                        montoCobrado = trabajo.MontoCobrado;
                        //}
                    }
                    string nombreProveedor = trabajo.Proveedor.Persona.RazonSocial ??
                                             (trabajo.Proveedor.Persona.PrimerNombre + " " +
                                              trabajo.Proveedor.Persona.ApellidoPaterno);
                    string documentoProveedor = (trabajo.Proveedor.Persona.DNI != null)
                                                    ? ("DNI - " + trabajo.Proveedor.Persona.DNI.ToString())
                                                    : ("RUC - " + trabajo.Proveedor.Persona.RUC.ToString());

                    HistorialTrabajosViewModel his = new HistorialTrabajosViewModel
                    {
                        TrabajoProveedorId = trabajo.TrabajoProveedorId,
                        FechaTrabajo = fechaTrabajo.ToString("dd/MM/yyyy"),
                        Puntuacion = puntuacion,
                        //PARA CUANDO ESTE LO DE ENCUESTAS
                        NombreCliente = nombreCliente,
                        DocumentoCliente = documentoCliente,
                        NombreProveedor = nombreProveedor,
                        DocumentoProveedor = documentoProveedor,
                        Servicios = servicios,
                        DescripcionCliente = trabajo.Trabajo.DescripcionCliente,
                        ReciboHonorarios_Factura = rph_factura,
                        MontoCobrado = montoCobrado,
                        LinkModificarDetalles = "",
                        EncuestaRespondida = trabajo.EncuestaCliente.IsCompletada,
                        Comentarios = comentarios
                    };
                    listaHistorialTrabajosViewModel.Add(his);

                }
                //listaHistorialTrabajosViewModel.Sort((x, y) => string.Compare(y.FechaTrabajo, x.FechaTrabajo));
            }
            return listaHistorialTrabajosViewModel;
        }

        public List<DemandaServiciosGeneralesViewModel> Demanda_ServiciosGenerales(string fechaInicio, string fechaFin)
        {
            List<DemandaServiciosGeneralesViewModel> demandaServiciosGeneralesViewModels = new List<DemandaServiciosGeneralesViewModel>();
            List<TipoServicio> tipoServicios = _sgpFactory.GetTipoServicios();
            List<Distrito> distritos = _sgpFactory.GetDistritos();

            if ((tipoServicios != null) && (tipoServicios.Count > 0))
            {
                DateTime finicio = DateTime.Now.AddMonths(-6);
                DateTime ffin = DateTime.Now.AddMonths(-1);
                if (!String.IsNullOrEmpty(fechaInicio))
                {
                    finicio = DateTime.Parse(fechaInicio);
                }

                if (!String.IsNullOrEmpty(fechaFin))
                {
                    ffin = DateTime.Parse(fechaFin);
                }
                int nroMeses = ((ffin.Year - finicio.Year) * 12) + ffin.Month - finicio.Month + 1;
                for (int i = 0; i < nroMeses; i++)
                {
                    DateTime d = finicio.AddMonths(i);
                    string fecha = d.Month + "/" + d.Year;

                    foreach (var tipoServicio in tipoServicios)
                    {
                        foreach (var distrito in distritos)
                        {
                            var trabajosProveedores = tipoServicio.TrabajosProveedores.Where(tp => (tp.Trabajo.Fecha.Year == d.Year) &&
                                                                                        (tp.Trabajo.Fecha.Month == d.Month) &&
                                                                                        (tp.Trabajo.NombreDistrito == distrito.NombreDistrito)).ToList();
                            double califProm = 0.0;
                            int nroTrabajos = trabajosProveedores.Count;
                            if (nroTrabajos > 0)
                            {
                                int sumaPuntajes = trabajosProveedores.Sum(s => s.EncuestaCliente.PuntajeTotal);
                                califProm = ((sumaPuntajes * 1.0) / (nroTrabajos * 1.0));
                            }

                            DemandaServiciosGeneralesViewModel demandaServiciosGeneralesViewModel = new DemandaServiciosGeneralesViewModel
                            {
                                Año = d.Year,
                                Mes = d.Month,
                                NombreServicio = tipoServicio.NombreServicio,
                                Distrito = distrito.NombreDistrito,
                                NroTrabajos = nroTrabajos,
                                CalificacionPromedio = califProm
                            };
                            demandaServiciosGeneralesViewModels.Add(demandaServiciosGeneralesViewModel);
                        }
                    }
                }
                demandaServiciosGeneralesViewModels = demandaServiciosGeneralesViewModels.OrderByDescending(r => r.Año).ThenByDescending(r => r.Mes).ThenByDescending(r => r.NroTrabajos).ThenByDescending(r => r.NombreServicio).ToList();
            }

            return demandaServiciosGeneralesViewModels;
        }

        public List<ProveedorDestacadoViewModel> ProveedoresDestacados(/*string fechaInicio, string fechaFin*/)
        {
            List<ProveedorDestacadoViewModel> proveedoresDestacadosViewModels = new List<ProveedorDestacadoViewModel>();
            List<Proveedor> proveedores = _sgpFactory.GetProveedores();

            if ((proveedores != null) && (proveedores.Count > 0))
            {
                //if (!String.IsNullOrEmpty(fechaInicio))
                //{
                //    DateTime finicio = DateTime.Parse(fechaInicio);
                //    proveedores = proveedores.Where(t => t.Trabajo.Fecha.Date.CompareTo(finicio) >= 0).ToList();
                //}

                //if (!String.IsNullOrEmpty(fechaFin))
                //{
                //    DateTime ffin = DateTime.Parse(fechaFin);
                //    proveedores = proveedores.Where(t => t.Trabajo.Fecha.Date.CompareTo(ffin) <= 0).ToList();
                //}
                foreach (var proveedor in proveedores)
                {
                    string nombreProveedor = "";
                    string documento = "";

                    switch (proveedor.Persona.TipoPersona)
                    {
                        case "Natural":
                            nombreProveedor = proveedor.Persona.PrimerNombre + " " + proveedor.Persona.ApellidoPaterno;
                            documento = "DNI - " + proveedor.Persona.DNI;
                            break;

                        case "Juridica":
                            nombreProveedor = proveedor.Persona.RazonSocial;
                            documento = "RUC - " + proveedor.Persona.RUC;
                            break;
                    }
                    string servicios = proveedor.TiposServicios.Aggregate("", (current, servicio) => current + (servicio.NombreServicio + " - "));
                    servicios = servicios.Substring(0, servicios.Length - 3);

                    var compras = proveedor.ComprasVirtuales.ToList();
                    int leadsCompras = 0;
                    if (compras.Count > 0 )
                    {
                        leadsCompras = compras.Sum(c => c.LeadsPagados);
                    }

                    ProveedorDestacadoViewModel proveedorDestacadoViewModel = new ProveedorDestacadoViewModel
                    {
                        ProveedorId = proveedor.ProveedorId,
                        NombreProveedor = nombreProveedor,
                        Imagen = (int)proveedor.Persona.ImagenId,
                        Documento = documento,
                        LeadsCompras = leadsCompras,
                        Servicios = servicios,
                        PuntuacionPromedio = proveedor.PuntuacionPromedio,
                        NroTrabajos = proveedor.TrabajosProveedores.Count,
                        NroRecomendaciones = proveedor.NroRecomendaciones + "/" + proveedor.TrabajosProveedores.Count,
                        NroVolveriaContratarlo = proveedor.NroVolveriaContratarlo + "/" + proveedor.TrabajosProveedores.Count
                    };
                    proveedoresDestacadosViewModels.Add(proveedorDestacadoViewModel);
                }
                proveedoresDestacadosViewModels = proveedoresDestacadosViewModels.OrderByDescending(p => p.PuntuacionPromedio).ThenByDescending(p => p.NroTrabajos).ThenByDescending(p => p.LeadsCompras).ThenByDescending(p => p.NombreProveedor).ToList();
            }

            return proveedoresDestacadosViewModels;
        }
    }
}