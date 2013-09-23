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
    public class ProveedoresController : Controller
    {
        private readonly LogicaProveedores _logicaProveedores = new LogicaProveedores();
        private readonly LogicaPersonas _logicaPersonas = new LogicaPersonas();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistrarProveedorNatural()
        {
            ViewBag.TiposServicios = ObtenerTiposServicios();

            return View();
        }

        [HttpPost]
        public ActionResult RegistrarProveedorNatural(ProveedorNaturalViewModel proveedorNaturalViewModel)
        {
            if (ModelState.IsValid)
            {
                Persona persona = _logicaPersonas.CrearObjetoPersonaNatural(proveedorNaturalViewModel, "Proveedor");
                Proveedor proveedor = _logicaProveedores.CrearObjetoProveedorNatural(proveedorNaturalViewModel);
                //setear la especialidad
                _logicaPersonas.AgregarPersona(persona);
                proveedor.PersonaId = persona.PersonaId;
                
                proveedor.TiposServicios = new List<TipoServicio>();
                foreach (var tipoServicioId in proveedorNaturalViewModel.ListTiposServiciosIds)
                {
                    TipoServicio tipo = _logicaProveedores.GetTipoServicioPorId(tipoServicioId);
                    proveedor.TiposServicios.Add(tipo);
                }

                _logicaProveedores.AgregarProveedor(proveedor);

                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Proveedor" });
                var s = WebSecurity.CreateAccount(persona.UserName, proveedorNaturalViewModel.Password);
                bool loginSuccess = WebSecurity.Login(persona.UserName, proveedorNaturalViewModel.Password);

                return RedirectToAction("Index");
            }
            ViewBag.TiposServicios = ObtenerTiposServicios();
            return View();
        }

        public ActionResult RegistrarProveedorJuridico()
        {
            ViewBag.TiposServicios = ObtenerTiposServicios();
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarProveedorJuridico(ProveedorJuridicoViewModel proveedorJuridicoViewModel)
        {
            if (ModelState.IsValid)
            {
                Persona persona = _logicaPersonas.CrearObjetoPersonaJuridica(proveedorJuridicoViewModel, "Proveedor");
                Proveedor proveedor = _logicaProveedores.CrearObjetoProveedorJuridico(proveedorJuridicoViewModel);
                //setear la especialidad
                _logicaPersonas.AgregarPersona(persona);
                proveedor.PersonaId = persona.PersonaId;

                proveedor.TiposServicios = new List<TipoServicio>();
                foreach (var tipoServicioId in proveedorJuridicoViewModel.ListTiposServiciosIds)
                {
                    TipoServicio tipo = _logicaProveedores.GetTipoServicioPorId(tipoServicioId);
                    proveedor.TiposServicios.Add(tipo);
                }

                _logicaProveedores.AgregarProveedor(proveedor);

                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Proveedor" });
                WebSecurity.CreateAccount(persona.UserName, proveedorJuridicoViewModel.Password);
                bool loginSuccess = WebSecurity.Login(persona.UserName, proveedorJuridicoViewModel.Password);

                return RedirectToAction("Index");
            }
            ViewBag.TiposServicios = ObtenerTiposServicios();
            return View();
        }

        public List<TipoServicio> ObtenerTiposServicios()
        {
            /*List<SelectListItem> listTipos = new List<SelectListItem>();
            List<TipoServicio> tipoServicios = _logicaProveedores.GetTipoServicios();

            foreach (TipoServicio tipo in tipoServicios)
            {
                listTipos.Add(new SelectListItem() { Text = tipo.NombreServicio, Value = tipo.TipoServicioId.ToString() });
            }

            return listTipos;*/
            List<TipoServicio> tipoServicios = _logicaProveedores.GetTipoServicios();
            return tipoServicios;
        }
    }
}
