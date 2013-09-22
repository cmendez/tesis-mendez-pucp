using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SistemaGeneraliz.Models.BusinessLogic;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.ViewModels;
using WebMatrix.WebData;

namespace SistemaGeneraliz.Controllers
{
    [Authorize]
    public class SuministradoresController : Controller
    {
        private readonly LogicaSuministradores _logicaSuministradores = new LogicaSuministradores();
        private readonly LogicaPersonas _logicaPersonas = new LogicaPersonas();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contacto()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult RegistrarSuministradorJuridico()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult RegistrarSuministradorJuridico(SuministradorJuridicoViewModel suministradorJuridicoViewModel)
        {
            if (ModelState.IsValid)
            {
                Persona persona = _logicaPersonas.CrearObjetoPersonaJuridica(suministradorJuridicoViewModel, "Suministrador");
                Suministrador suministrador = _logicaSuministradores.CrearObjetoSuministradorJuridico(suministradorJuridicoViewModel);
                
                _logicaPersonas.AgregarPersona(persona);
                suministrador.PersonaId = persona.PersonaId;
                _logicaSuministradores.AgregarSuministrador(suministrador);

                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Suministrador" });
                WebSecurity.CreateAccount(persona.UserName, suministradorJuridicoViewModel.Password);
                bool loginSuccess = WebSecurity.Login(persona.UserName, suministradorJuridicoViewModel.Password);

                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
