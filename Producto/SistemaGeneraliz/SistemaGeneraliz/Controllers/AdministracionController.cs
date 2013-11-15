using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SistemaGeneraliz.Models.BusinessLogic;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.ViewModels;
using WebMatrix.WebData;

namespace SistemaGeneraliz.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministracionController : Controller
    {
        private  LogicaProveedores _logicaProveedores = new LogicaProveedores();
        private  LogicaSuministradores _logicaSuministradores = new LogicaSuministradores();
        private  LogicaPersonas _logicaPersonas = new LogicaPersonas();
        private  LogicaUbicaciones _logicaUbicaciones = new LogicaUbicaciones();
        //
        // GET: /Administracion/

        public ActionResult Usuarios()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult RegistrarSuministradorJuridico()
        {
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax
            SuministradorJuridicoViewModel suministrador = new SuministradorJuridicoViewModel();
            suministrador.Latitud = -12.08611459617003;
            suministrador.Longitud = -77.00229406356812;
            return View(suministrador);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult RegistrarSuministradorJuridico(SuministradorJuridicoViewModel suministradorJuridicoViewModel)
        {
            if (ModelState.IsValid)
            {
                bool existe = _logicaPersonas.ExisteDNIRUC(null, suministradorJuridicoViewModel.RUC);
                if (existe)
                {
                    ModelState.AddModelError("", "Error: el DNI y/o RUC ingresado ya existe.");
                    ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                    return View(suministradorJuridicoViewModel);
                }

                if ((suministradorJuridicoViewModel.File == null) || (suministradorJuridicoViewModel.File.ContentLength <= 0))
                {
                    ModelState.AddModelError("", "Error: es obligatorio subir una foto");
                    ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                    return View(suministradorJuridicoViewModel);
                }
                else
                {
                    var file = suministradorJuridicoViewModel.File;
                    string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                    string ext2 = file.FileName;

                    if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                    {
                        ModelState.AddModelError("", "Error: la extensión de la foto solo puede ser JPG, JPEG, y PNG");
                        ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                        return View(suministradorJuridicoViewModel);
                    }
                }

                Imagen foto = _logicaPersonas.AgregarFotoPersona(suministradorJuridicoViewModel.File);
                suministradorJuridicoViewModel.ImagenPrincipal = foto.ImagenId;
                Persona persona = _logicaPersonas.CrearObjetoPersonaJuridica(suministradorJuridicoViewModel, "Suministrador");
                Suministrador suministrador = _logicaSuministradores.CrearObjetoSuministradorJuridico(suministradorJuridicoViewModel);

                _logicaPersonas.AgregarPersona(persona);
                suministrador.PersonaId = persona.PersonaId;
                UbicacionPersona ubicacion = _logicaUbicaciones.CrearObjetoUbicacionPersonaJuridica(suministradorJuridicoViewModel, persona);
                _logicaUbicaciones.AgregarUbicacion(ubicacion);
                _logicaSuministradores.AgregarSuministrador(suministrador);

                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { "Suministrador" });
                WebSecurity.CreateAccount(persona.UserName, suministradorJuridicoViewModel.Password);
                bool loginSuccess = WebSecurity.Login(persona.UserName, suministradorJuridicoViewModel.Password);
                //Session["Usuario"] = _logicaPersonas.GetNombrePersonaLoggeada(persona.PersonaId);
                //Session["ImagenId"] = persona.ImagenId;

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax
            return View(suministradorJuridicoViewModel);
        }

        public ActionResult HistoricoTrabajos()
        {
            return View();
        }

        // ReSharper disable InconsistentNaming
        public ActionResult HistoricoTrabajos_Read([DataSourceRequest]DataSourceRequest request, string fechaInicio, string fechaFin)
        {
            List<HistorialTrabajosViewModel> listaHistorialTrabajosViewModel = _logicaProveedores.HistoricoTrabajos(fechaInicio, fechaFin);
            return Json(listaHistorialTrabajosViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConversionLeads()
        {
            return View();
        }

        // ReSharper disable InconsistentNaming
        public ActionResult ReporteConversionLeads_Read([DataSourceRequest]DataSourceRequest request, string fechaInicio, string fechaFin)
        {
            List<ConversionLeadsViewModel> suministradorJuridicoViewModels = _logicaSuministradores.ReporteConversionLeads(fechaInicio, fechaFin);
            return Json(suministradorJuridicoViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProveedoresDestacados()
        {
            return View();
        }

        // ReSharper disable InconsistentNaming
        public ActionResult ProveedoresDestacados_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<ProveedorDestacadoViewModel> proveedoresDestacadosViewModels = _logicaProveedores.ProveedoresDestacados();
            return Json(proveedoresDestacadosViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RecompensarProveedores()
        {
            _logicaProveedores.RecompensarProveedores();
            var recargasJson = new List<Object>();
            Object o = new { Msg = "ok" };
            recargasJson.Add(o);
            return Json(recargasJson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdministrarUsuarios()
        {
            return View();
        }

        public ActionResult HabilitarEliminarUsuarios()
        {
            return View();
        }

        // ReSharper disable InconsistentNaming
        public ActionResult Usuarios_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<UsuarioViewModel> usuarioViewModels = _logicaPersonas.ObtenerUsuarios();
            return Json(usuarioViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUsuarioJSON(string documento)
        {
            Persona persona = _logicaPersonas.GetPersonaPorUsername(documento);
            
            string nombre = "";
            int id = -1;
            int imagen = -1;
            if (persona != null)
            {
                id = persona.PersonaId;
                if (persona.TipoPersona == "Natural")
                    nombre = persona.PrimerNombre + " " + persona.ApellidoPaterno;
                else if (persona.TipoPersona == "Juridica")
                    nombre = persona.RazonSocial;
                imagen = (int)persona.ImagenId;
            }

            var personasJson = new List<Object>();
            Object o = new { UsuarioId = id, NombreUsuario = nombre, ImagenId = imagen };
            personasJson.Add(o);
            return Json(personasJson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CambiarEstadoUsuario(int idUsuario = -1, string estado = "")
        {
            _logicaPersonas.CambiarEstadoUsuario(idUsuario, estado);
            var recargasJson = new List<Object>();
            Object o = new { Msg = "ok" };
            recargasJson.Add(o);
            return Json(recargasJson, JsonRequestBehavior.AllowGet);
        }
    }
}
