using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SistemaGeneraliz.Models.BusinessLogic;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;
using SistemaGeneraliz.Models.ViewModels;
using WebMatrix.WebData;

namespace SistemaGeneraliz.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly LogicaClientes _logicaClientes = new LogicaClientes();
        private readonly LogicaPersonas _logicaPersonas = new LogicaPersonas();
        private readonly LogicaUbicaciones _logicaUbicaciones = new LogicaUbicaciones();
        private readonly LogicaProveedores _logicaProveedores = new LogicaProveedores();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegistrarClienteNatural()
        {
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            ClienteNaturalViewModel cliente = new ClienteNaturalViewModel();
            cliente.Latitud = -12.08611459617003;
            cliente.Longitud = -77.00229406356812;
            return View(cliente);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegistrarClienteNatural(ClienteNaturalViewModel clienteNaturalViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool existe = _logicaPersonas.ExisteDNIRUC(clienteNaturalViewModel.DNI, clienteNaturalViewModel.RUC);
                    if (existe)
                    {
                        ModelState.AddModelError("", "Error: el DNI y/o RUC ingresado ya existe.");
                        ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                        return View(clienteNaturalViewModel);
                    }

                    if ((clienteNaturalViewModel.File == null) || (clienteNaturalViewModel.File.ContentLength <= 0))
                    {
                        ModelState.AddModelError("", "Error: es obligatorio subir una foto");
                        ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                        return View(clienteNaturalViewModel);
                    }
                    else
                    {
                        var file = clienteNaturalViewModel.File;
                        string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                        string ext2 = file.FileName;

                        if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                        {
                            ModelState.AddModelError("", "Error: la extensión de la foto solo puede ser JPG, JPEG, y PNG");
                            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                            return View(clienteNaturalViewModel);
                        }
                    }
                    Imagen foto = _logicaPersonas.AgregarFotoPersona(clienteNaturalViewModel.File);
                    clienteNaturalViewModel.ImagenPrincipal = foto.ImagenId;

                    Persona persona = _logicaPersonas.CrearObjetoPersonaNatural(clienteNaturalViewModel, "Cliente");
                    Cliente cliente = _logicaClientes.CrearObjetoClienteNatural(clienteNaturalViewModel);
                    //setear la especialidad
                    _logicaPersonas.AgregarPersona(persona);
                    cliente.PersonaId = persona.PersonaId;
                    UbicacionPersona ubicacion = _logicaUbicaciones.CrearObjetoUbicacionPersonaNatural(clienteNaturalViewModel, persona);
                    _logicaUbicaciones.AgregarUbicacion(ubicacion);
                    _logicaClientes.AgregarCliente(cliente);

                    Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Cliente" });
                    WebSecurity.CreateAccount(persona.UserName, clienteNaturalViewModel.Password);
                    bool loginSuccess = WebSecurity.Login(persona.UserName, clienteNaturalViewModel.Password);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            return View(clienteNaturalViewModel);
        }

        [AllowAnonymous]
        public ActionResult RegistrarClienteJuridico()
        {
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            ClienteJuridicoViewModel cliente = new ClienteJuridicoViewModel();
            cliente.Latitud = -12.08611459617003;
            cliente.Longitud = -77.00229406356812;
            return View(cliente);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegistrarClienteJuridico(ClienteJuridicoViewModel clienteJuridicoViewModel)
        {
            if (ModelState.IsValid)
            {
                bool existe = _logicaPersonas.ExisteDNIRUC(null, clienteJuridicoViewModel.RUC);
                if (existe)
                {
                    ModelState.AddModelError("", "Error: el DNI y/o RUC ingresado ya existe.");
                    ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                    return View(clienteJuridicoViewModel);
                }

                if ((clienteJuridicoViewModel.File == null) || (clienteJuridicoViewModel.File.ContentLength <= 0))
                {
                    ModelState.AddModelError("", "Error: es obligatorio subir una foto");
                    ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                    return View(clienteJuridicoViewModel);
                }
                else
                {
                    var file = clienteJuridicoViewModel.File;
                    string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                    string ext2 = file.FileName;

                    if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                    {
                        ModelState.AddModelError("", "Error: la extensión de la foto solo puede ser JPG, JPEG, y PNG");
                        ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                        return View(clienteJuridicoViewModel);
                    }
                }
                Imagen foto = _logicaPersonas.AgregarFotoPersona(clienteJuridicoViewModel.File);
                clienteJuridicoViewModel.ImagenPrincipal = foto.ImagenId;

                Persona persona = _logicaPersonas.CrearObjetoPersonaJuridica(clienteJuridicoViewModel, "Cliente");
                Cliente cliente = _logicaClientes.CrearObjetoClienteJuridico(clienteJuridicoViewModel);
                //setear la especialidad
                _logicaPersonas.AgregarPersona(persona);
                cliente.PersonaId = persona.PersonaId;
                UbicacionPersona ubicacion = _logicaUbicaciones.CrearObjetoUbicacionPersonaJuridica(clienteJuridicoViewModel, persona);
                _logicaUbicaciones.AgregarUbicacion(ubicacion);
                _logicaClientes.AgregarCliente(cliente);

                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Cliente" });
                WebSecurity.CreateAccount(persona.UserName, clienteJuridicoViewModel.Password);
                bool loginSuccess = WebSecurity.Login(persona.UserName, clienteJuridicoViewModel.Password);

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            return View(clienteJuridicoViewModel);
        }

        [Authorize(Roles = "Administrador, Cliente")]
        public ActionResult EditarMiInformacion()
        {
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax
            Cliente cliente = _logicaClientes.GetClientePorPersonaId(WebSecurity.CurrentUserId);
            if (cliente.Persona.TipoPersona == "Natural")
            {
                ClienteNaturalViewModel clienteNaturalViewModel = (ClienteNaturalViewModel)_logicaClientes.GetClienteViewModel(cliente, "Natural");
                return View("EditarClienteNatural", clienteNaturalViewModel);
            }
            if (cliente.Persona.TipoPersona == "Juridica")
            {
                ClienteJuridicoViewModel clienteJuridicoViewModel = (ClienteJuridicoViewModel)_logicaClientes.GetClienteViewModel(cliente, "Jurídica");
                return View("EditarClienteJuridico", clienteJuridicoViewModel);
            }
            return null;
        }
        
        [HttpPost]
        public ActionResult EditarMiInformacion_Natural(ClienteNaturalViewModel clienteNaturalViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //habria que ver la manera de setear un objeto File dummy en el viewmodel por si el usuario no desea actualizar su foto,
                    //pero por ahora simplemente lo obligamos a que si lo haga
                    if ((clienteNaturalViewModel.File == null) || (clienteNaturalViewModel.File.ContentLength <= 0))
                    {
                        ModelState.AddModelError("", "Error: es obligatorio subir una foto");
                        ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                        return View("EditarClienteNatural", clienteNaturalViewModel);
                    }
                    else
                    {
                        var file = clienteNaturalViewModel.File;
                        string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                        string ext2 = file.FileName;

                        if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                        {
                            ModelState.AddModelError("", "Error: la extensión de la foto solo puede ser JPG, JPEG, y PNG");
                            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                            return View("EditarClienteNatural", clienteNaturalViewModel);
                        }
                    }

                    if (clienteNaturalViewModel.Password != "password")
                    {
                        if (!WebSecurity.ChangePassword(clienteNaturalViewModel.DNI, clienteNaturalViewModel.OldPassword, clienteNaturalViewModel.Password))
                        {
                            ModelState.AddModelError("", "Error: ingrese bien las contraseñas");
                            ViewBag.Distritos = _logicaPersonas.GetDistritos();
                            //solo para Lima, si uso otras ciudades, usar ajax en la vista
                            return View("EditarClienteNatural", clienteNaturalViewModel);
                        }
                    }

                    Imagen foto = _logicaPersonas.AgregarFotoPersona(clienteNaturalViewModel.File);
                    clienteNaturalViewModel.ImagenPrincipal = foto.ImagenId;

                    Persona persona = _logicaPersonas.ModificarObjetoPersonaNatural(clienteNaturalViewModel);
                    //Cliente cliente = _logicaClientes.ModificarObjetoClienteNatural(clienteNaturalViewModel);
                    _logicaPersonas.ActualizarPersona(persona);
                    UbicacionPersona ubicacion = _logicaUbicaciones.ModificarObjetoUbicacionPersonaNatural(clienteNaturalViewModel, persona);
                    _logicaUbicaciones.ActualizarUbicacion(ubicacion);
                    //_logicaClientes.ModificarCliente(cliente);
                    //WebSecurity.Logout();
                    //WebSecurity.Login(persona.UserName, clienteNaturalViewModel.Password);
                    string s = UsuariosController.GetNombrePersonaLoggeada();
                    //hace el update y todo pero no se refleja el nuevo nombre e imagen en el navigation bar
                    //por eso del static... ver si lo puedo arreglar luego
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            return View("EditarClienteNatural", clienteNaturalViewModel);
        }
        
        [HttpPost]
        public ActionResult EditarMiInformacion_Juridico(ClienteJuridicoViewModel clienteJuridicoViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //habria que ver la manera de setear un objeto File dummy en el viewmodel por si el usuario no desea actualizar su foto,
                    //pero por ahora simplemente lo obligamos a que si lo haga
                    if ((clienteJuridicoViewModel.File == null) || (clienteJuridicoViewModel.File.ContentLength <= 0))
                    {
                        ModelState.AddModelError("", "Error: es obligatorio subir una foto");
                        ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                        return View("EditarClienteJuridico", clienteJuridicoViewModel);
                    }
                    else
                    {
                        var file = clienteJuridicoViewModel.File;
                        string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                        string ext2 = file.FileName;

                        if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                        {
                            ModelState.AddModelError("", "Error: la extensión de la foto solo puede ser JPG, JPEG, y PNG");
                            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                            return View("EditarClienteJuridico", clienteJuridicoViewModel);
                        }
                    }

                    if (clienteJuridicoViewModel.Password != "password")
                    {
                        if (!WebSecurity.ChangePassword(clienteJuridicoViewModel.RUC, clienteJuridicoViewModel.OldPassword, clienteJuridicoViewModel.Password))
                        {
                            ModelState.AddModelError("", "Error: ingrese bien las contraseñas");
                            ViewBag.Distritos = _logicaPersonas.GetDistritos();
                            //solo para Lima, si uso otras ciudades, usar ajax en la vista
                            return View("EditarClienteJuridico", clienteJuridicoViewModel);
                        }
                    }

                    Imagen foto = _logicaPersonas.AgregarFotoPersona(clienteJuridicoViewModel.File);
                    clienteJuridicoViewModel.ImagenPrincipal = foto.ImagenId;

                    Persona persona = _logicaPersonas.ModificarObjetoPersonaJuridico(clienteJuridicoViewModel);
                    //Cliente cliente = _logicaClientes.ModificarObjetoClienteJuridico(clienteJuridicoViewModel);
                    _logicaPersonas.ActualizarPersona(persona);
                    UbicacionPersona ubicacion = _logicaUbicaciones.ModificarObjetoUbicacionPersonaJuridica(clienteJuridicoViewModel, persona);
                    _logicaUbicaciones.ActualizarUbicacion(ubicacion);
                    //_logicaClientes.ModificarCliente(cliente);
                    //WebSecurity.Logout();
                    //WebSecurity.Login(persona.UserName, clienteJuridicoViewModel.Password);
                    string s = UsuariosController.GetNombrePersonaLoggeada();
                    //hace el update y todo pero no se refleja el nuevo nombre e imagen en el navigation bar
                    //por eso del static... ver si lo puedo arreglar luego
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            return View("EditarClienteJuridico", clienteJuridicoViewModel);
        }

        public ActionResult MenuBuscarProveedores()
        {
            Cliente cliente = _logicaClientes.GetClientePorPersonaId(WebSecurity.CurrentUserId);
            if (cliente != null)
            {
                if (cliente.Persona.IsHabilitado == 0)
                {
                    int n = _logicaClientes.CantidadEncuestasPendientesCliente(cliente.ClienteId);

                    ViewBag.Motivo1 = "Tiene " + n + " encuesta(s) pendiente(s) por responder.";
                    ViewBag.LinkText = "Ir a Encuestas Pendientes";
                    ViewBag.Controlador = "Clientes";
                    ViewBag.Metodo = "CalificarProveedores";
                    return View("../Usuarios/UsuarioInhabilitado");
                }
            }

            return View();
        }

        [Authorize(Roles = "Administrador, Cliente")]
        public ActionResult BuscarProveedores(string tipoBusqueda)
        {
            int idPersona = WebSecurity.CurrentUserId;
            Cliente cliente = _logicaClientes.GetClientePorPersonaId(idPersona);
            if (cliente != null)
            {
                UbicacionPersona ubicacion = _logicaUbicaciones.GetPrimeraUbicacionPersona(idPersona);
                ViewBag.ClienteId = cliente.ClienteId;
                ViewBag.Latitud = ubicacion.Latitud;
                ViewBag.Longitud = ubicacion.Longitud;
                ViewBag.Ubicacion = cliente.Persona.DireccionCompleta;
            }
            else
            {
                ViewBag.ClienteId = -1;
                ViewBag.Latitud = -1;
                ViewBag.Longitud = -1;
                ViewBag.Ubicacion = "";
            }

            //Combobox Servicios
            ViewBag.TipoServicios = _logicaProveedores.GetTipoServicios();

            if (tipoBusqueda == "Automatizada")
                return View("BusquedaAutomatizadaProveedores");
            else if (tipoBusqueda == "Manual")
                return View("BusquedaManualProveedores");
            return null;
        }

        [HttpGet]
        public ActionResult GetProveedoresBusquedaTabuJSON(string valueServicios = "", double latitud = 1, double longitud = 1)
        {
            var proveedoresJson = new List<Object>();

            if (Roles.IsUserInRole("Cliente"))
            {
                List<ProveedorBusquedaViewModel> proveedores = _logicaClientes.EjecutarAlgoritmoTabu(valueServicios, latitud, longitud);
                //VER SI DEBERIAMOS ACTUALIZAR INDICE NroBusquedasCliente DEL PROVEEDOR LUEGO DE QUE SEA ELEGIDO POR EL ALGORITMO TABÚ
                if (proveedores != null)
                {
                    foreach (ProveedorBusquedaViewModel proveedor in proveedores)
                    {
                        Object o = new
                        {
                            ProveedorId = proveedor.ProveedorId,
                            Puntaje = proveedor.Puntaje,
                            FotoId = proveedor.FotoId,
                            NombreCompleto = proveedor.NombreCompleto,
                            TipoDocumento = proveedor.TipoDocumento,
                            Documento = proveedor.TipoDocumento + " - " + proveedor.Documento,
                            Servicio = proveedor.Servicio,
                            ServicioId = proveedor.ServicioId,
                            Descripcion = proveedor.Descripcion,
                            Telefono1 = proveedor.Telefono1,
                            Telefono2 = proveedor.Telefono2,
                            Telefono3 = proveedor.Telefono3,
                            Email1 = proveedor.Email1,
                            Email2 = proveedor.Email2
                        };

                        proveedoresJson.Add(o);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Solo los clientes pueden hacer búsquedas de proveedores.");
            }

            return Json(proveedoresJson, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet]
        public ActionResult ContratarProveedores(int clienteId, string proveedoresIds, string serviciosIds, string fecha, string ubicacion, double latitud, double longitud, string desc)
        {
            if (Roles.IsUserInRole("Cliente"))
            {
                Trabajo trabajo = _logicaClientes.AgregarTrabajo(clienteId, proveedoresIds, serviciosIds, fecha, ubicacion, latitud, longitud, desc);
            }

            var recargasJson = new List<Object>();
            Object o = new { Msg = "ok" };
            recargasJson.Add(o);
            return Json(recargasJson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProveedoresBusquedaManualJSON(string nombre, string valueServicios = "", double latitud = 1, double longitud = 1)
        {
            var proveedoresJson = new List<Object>();

            if (Roles.IsUserInRole("Cliente"))
            {
                List<ProveedorBusquedaViewModel> proveedores = _logicaClientes.BusquedaManualProveedores(nombre, valueServicios);
                if (proveedores != null)
                {
                    foreach (ProveedorBusquedaViewModel proveedor in proveedores)
                    {
                        Object o = new
                        {
                            ProveedorId = proveedor.ProveedorId,
                            Puntaje = proveedor.Puntaje,
                            FotoId = proveedor.FotoId,
                            NombreCompleto = proveedor.NombreCompleto,
                            TipoDocumento = proveedor.TipoDocumento,
                            Documento = proveedor.TipoDocumento + " - " + proveedor.Documento,
                            Servicio = proveedor.Servicio,
                            ServicioId = proveedor.ServicioId,
                            Descripcion = proveedor.Descripcion,
                            Telefono1 = proveedor.Telefono1,
                            Telefono2 = proveedor.Telefono2,
                            Telefono3 = proveedor.Telefono3,
                            Email1 = proveedor.Email1,
                            Email2 = proveedor.Email2
                        };

                        proveedoresJson.Add(o);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Solo los clientes pueden hacer búsquedas de proveedores.");
            }

            return Json(proveedoresJson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HistorialTrabajosProveedor(int proveedorId)
        {
            var trabajosJson = new List<Object>();

            List<HistorialTrabajosViewModel> listaHistorialTrabajosViewModel = _logicaProveedores.GetHistorialTrabajos(proveedorId, "SoloVisibles");
            if ((listaHistorialTrabajosViewModel != null) && (listaHistorialTrabajosViewModel.Count > 0))
            {
                foreach (HistorialTrabajosViewModel trabajo in listaHistorialTrabajosViewModel)
                {
                    Object o = new
                    {
                        FechaTrabajo = trabajo.FechaTrabajo,
                        Puntuacion = trabajo.Puntuacion,
                        NombreCliente = trabajo.NombreCliente,
                        Servicios = trabajo.Servicios,
                        DescripcionCliente = trabajo.DescripcionCliente,
                        ReciboHonorarios_Factura = trabajo.ReciboHonorarios_Factura,
                        Comentarios = trabajo.Comentarios
                    };

                    trabajosJson.Add(o);
                }
            }
            return Json(trabajosJson, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Cliente")]
        public ActionResult CalificarProveedores()
        {
            return View();
        }

        // ReSharper disable InconsistentNaming
        public ActionResult EncuestasPendientes_Read([DataSourceRequest]DataSourceRequest request)
        {
            Cliente cliente = _logicaClientes.GetClientePorPersonaId(WebSecurity.CurrentUserId);
            List<EncuestasClientesViewModel> listaEncuestasClientesViewModel = new List<EncuestasClientesViewModel>();
            if (cliente != null)
            {
                listaEncuestasClientesViewModel = _logicaClientes.GetEncuestasPendientes(cliente.ClienteId);
            }

            return Json(listaEncuestasClientesViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Cliente")]
        public ActionResult LlenarEncuestaCliente(int encuestaClienteId)
        {
            Cliente cliente = _logicaClientes.GetClientePorPersonaId(WebSecurity.CurrentUserId);
            List<EncuestasClientesViewModel> listaEncuestasClientesViewModel = new List<EncuestasClientesViewModel>();
            if (cliente != null)
            {
                listaEncuestasClientesViewModel = _logicaClientes.GetEncuestasPendientes(cliente.ClienteId);
            }
            EncuestasClientesViewModel encuestaViewModel = listaEncuestasClientesViewModel.Find(e => e.EncuestaClienteId == encuestaClienteId);
            ViewBag.EncuestaViewModel = encuestaViewModel;
            List<CriterioCalificacion> criteriosEncuesta = _logicaClientes.GetCriteriosEncuestas();
            List<PreguntasEncuestaViewModel> listaPreguntas = new List<PreguntasEncuestaViewModel>();
            int nroPreguntasEstrellas = 0;
            foreach (var criterio in criteriosEncuesta)
            {
                nroPreguntasEstrellas = (criterio.TipoPregunta == "Estrellas") ? ++nroPreguntasEstrellas : nroPreguntasEstrellas;
                PreguntasEncuestaViewModel preguntas = new PreguntasEncuestaViewModel
                {
                    CriterioId = criterio.CriterioCalificacionId,
                    TipoPregunta = criterio.TipoPregunta,
                    PreguntaAsociada = criterio.PreguntaAsociada,
                    PuntajeOtorgado = -1,
                    RespuestaPregunta = null,
                    NroOpciones = criterio.PuntajeMaximo
                };
                listaPreguntas.Add(preguntas);
            }

            ViewBag.LimitePreguntasLadoIzq = Convert.ToInt32(criteriosEncuesta.Count / 2) + 1;
            ViewBag.LimitePreguntasLadoDer = Convert.ToInt32(criteriosEncuesta.Count);
            ViewBag.NroPreguntasEstrellas = nroPreguntasEstrellas;

            return View(listaPreguntas);
        }

        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet]
        public ActionResult EnviarEncuestaCliente(int clienteId, int encuestaId, int trabajoProveedorId, string respuestas, string comentarios)
        {
            _logicaClientes.EnviarEncuestaCliente(clienteId, encuestaId, trabajoProveedorId, respuestas, comentarios);
            var json = new List<Object>();
            Object o = new { Msg = "ok" };
            json.Add(o);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
