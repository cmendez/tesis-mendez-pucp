using System;
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
    }
}
