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
        private readonly LogicaUbicaciones _logicaUbicaciones = new LogicaUbicaciones();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistrarProveedorNatural()
        {
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax
            ViewBag.TiposServicios = ObtenerTiposServicios();
            ProveedorNaturalViewModel proveedor = new ProveedorNaturalViewModel();
            proveedor.Latitud = -12.08611459617003;
            proveedor.Longitud = -77.00229406356812;
            return View(proveedor);
        }

        [HttpPost]
        public ActionResult RegistrarProveedorNatural(ProveedorNaturalViewModel proveedorNaturalViewModel)
        {
            if (ModelState.IsValid)
            {
                bool existe = _logicaPersonas.ExisteDNIRUC(proveedorNaturalViewModel.DNI, proveedorNaturalViewModel.RUC);
                if (existe)
                {
                    ModelState.AddModelError("", "Error: el DNI y/o RUC ingresado ya existe.");
                    ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                    ViewBag.TiposServicios = ObtenerTiposServicios();
                    return View(proveedorNaturalViewModel);
                }

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

                UbicacionPersona ubicacion = _logicaUbicaciones.CrearObjetoUbicacionPersonaNatural(proveedorNaturalViewModel, persona);
                _logicaUbicaciones.AgregarUbicacion(ubicacion);
                _logicaProveedores.AgregarProveedor(proveedor);

                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Proveedor" });
                var s = WebSecurity.CreateAccount(persona.UserName, proveedorNaturalViewModel.Password);
                bool loginSuccess = WebSecurity.Login(persona.UserName, proveedorNaturalViewModel.Password);

                return RedirectToAction("Index");
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            ViewBag.TiposServicios = ObtenerTiposServicios();
            return View(proveedorNaturalViewModel);
        }

        public ActionResult RegistrarProveedorJuridico()
        {
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            ViewBag.TiposServicios = ObtenerTiposServicios();
            ProveedorJuridicoViewModel proveedor = new ProveedorJuridicoViewModel();
            proveedor.Latitud = -12.08611459617003;
            proveedor.Longitud = -77.00229406356812;
            return View(proveedor);
        }

        [HttpPost]
        public ActionResult RegistrarProveedorJuridico(ProveedorJuridicoViewModel proveedorJuridicoViewModel)
        {
            if (ModelState.IsValid)
            {
                bool existe = _logicaPersonas.ExisteDNIRUC(null, proveedorJuridicoViewModel.RUC);
                if (existe)
                {
                    ModelState.AddModelError("", "Error: el DNI y/o RUC ingresado ya existe.");
                    ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                    ViewBag.TiposServicios = ObtenerTiposServicios();
                    return View(proveedorJuridicoViewModel);
                }

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

                UbicacionPersona ubicacion = _logicaUbicaciones.CrearObjetoUbicacionPersonaJuridica(proveedorJuridicoViewModel, persona);
                _logicaUbicaciones.AgregarUbicacion(ubicacion);
                _logicaProveedores.AgregarProveedor(proveedor);

                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Proveedor" });
                WebSecurity.CreateAccount(persona.UserName, proveedorJuridicoViewModel.Password);
                bool loginSuccess = WebSecurity.Login(persona.UserName, proveedorJuridicoViewModel.Password);

                return RedirectToAction("Index");
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            ViewBag.TiposServicios = ObtenerTiposServicios();
            return View(proveedorJuridicoViewModel);
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
