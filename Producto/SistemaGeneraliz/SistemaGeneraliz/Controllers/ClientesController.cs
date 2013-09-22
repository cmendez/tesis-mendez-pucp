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

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegistrarClienteNatural()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegistrarClienteNatural(ClienteNaturalViewModel clienteNaturalViewModel)
        {
            if (ModelState.IsValid)
            {
                Persona persona = _logicaPersonas.CrearObjetoPersonaNatural(clienteNaturalViewModel, "Cliente");
                Cliente cliente = _logicaClientes.CrearObjetoClienteNatural(clienteNaturalViewModel);
                //setear la especialidad
                _logicaPersonas.AgregarPersona(persona);
                cliente.PersonaId = persona.PersonaId;
                _logicaClientes.AgregarCliente(cliente);

                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Cliente" });
                WebSecurity.CreateAccount(persona.UserName, clienteNaturalViewModel.Password);
                bool loginSuccess = WebSecurity.Login(persona.UserName, clienteNaturalViewModel.Password);

                return RedirectToAction("Index");
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult RegistrarClienteJuridico()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegistrarClienteJuridico(ClienteJuridicoViewModel clienteNaturalViewModel)
        {
            if (ModelState.IsValid)
            {
                Persona persona = _logicaPersonas.CrearObjetoPersonaJuridica(clienteNaturalViewModel, "Cliente");
                Cliente cliente = _logicaClientes.CrearObjetoClienteJuridico(clienteNaturalViewModel);
                //setear la especialidad
                _logicaPersonas.AgregarPersona(persona);
                cliente.PersonaId = persona.PersonaId;
                _logicaClientes.AgregarCliente(cliente);

                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Cliente" });
                WebSecurity.CreateAccount(persona.UserName, clienteNaturalViewModel.Password);
                bool loginSuccess = WebSecurity.Login(persona.UserName, clienteNaturalViewModel.Password);

                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
