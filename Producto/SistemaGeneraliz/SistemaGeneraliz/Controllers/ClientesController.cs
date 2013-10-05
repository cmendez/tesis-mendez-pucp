using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using SistemaGeneraliz.Models.BusinessLogic;
using SistemaGeneraliz.Models.Entities;
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

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw;
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

                return RedirectToAction("Index");
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            return View(clienteJuridicoViewModel);
        }

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
                if (proveedores != null)
                {
                    foreach (ProveedorBusquedaViewModel proveedor in proveedores)
                    {
                        Object o = new
                        {
                            ProveedorId = proveedor.ProveedorId,
                            Puntaje = proveedor.Puntaje,
                            RutaFoto = proveedor.RutaFoto,
                            NombreCompleto = proveedor.NombreCompleto,
                            TipoDocumento = proveedor.TipoDocumento,
                            Documento = proveedor.TipoDocumento + " - " + proveedor.Documento,
                            Servicio = proveedor.Servicio,
                            ServicioId = proveedor.ServicioId,
                            Descripcion = proveedor.Descripcion,
                            VerTrabajos = proveedor.VerTrabajos,
                            VerComentarios = proveedor.VerComentarios,
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
        public ActionResult ContratarProveedores(int clienteId, string proveedoresIds, string serviciosIds, string fecha, string ubicacion, string desc)
        {
            if (Roles.IsUserInRole("Administrador"))
                return null;
            
            Trabajo trabajo = _logicaClientes.AgregarTrabajo(clienteId, proveedoresIds, serviciosIds, fecha, ubicacion, desc);

            var recargasJson = new List<Object>();
            Object o = new { Msg = "ok" };
            recargasJson.Add(o);
            return Json(recargasJson, JsonRequestBehavior.AllowGet);

            //return RedirectToAction("DatosContactoProveedores", trabajo);
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
                            RutaFoto = proveedor.RutaFoto,
                            NombreCompleto = proveedor.NombreCompleto,
                            TipoDocumento = proveedor.TipoDocumento,
                            Documento = proveedor.TipoDocumento + " - " + proveedor.Documento,
                            Servicio = proveedor.Servicio,
                            ServicioId = proveedor.ServicioId,
                            Descripcion = proveedor.Descripcion,
                            VerTrabajos = proveedor.VerTrabajos,
                            VerComentarios = proveedor.VerComentarios,
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
    }
}
