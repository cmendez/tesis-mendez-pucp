using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    [Authorize]
    public class SuministradoresController : Controller
    {
        private readonly LogicaSuministradores _logicaSuministradores = new LogicaSuministradores();
        private readonly LogicaPersonas _logicaPersonas = new LogicaPersonas();
        private readonly LogicaUbicaciones _logicaUbicaciones = new LogicaUbicaciones();
        private readonly LogicaProveedores _logicaProveedores = new LogicaProveedores();

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
                Session["Usuario"] = _logicaPersonas.GetNombrePersonaLoggeada(persona.PersonaId);
                Session["ImagenId"] = persona.ImagenId;

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax
            return View(suministradorJuridicoViewModel);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult RecargarLeads()
        {
            int idPersona = WebSecurity.CurrentUserId;
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(idPersona);

            if (suministrador == null)
            {
                suministrador = new Suministrador();
            }

            ViewBag.Suministrador = suministrador;

            return View();
        }

        // ReSharper disable InconsistentNaming
        public ActionResult RecargasLeads_Read([DataSourceRequest]DataSourceRequest request)
        {
            int idPersona = WebSecurity.CurrentUserId;
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(idPersona);
            List<RecargasLeadsViewModel> listaRecargasViewModel = new List<RecargasLeadsViewModel>();
            if (suministrador != null)
            {
                listaRecargasViewModel = _logicaSuministradores.GetListaRecargasSuministrador(suministrador.SuministradorId);
            }

            return Json(listaRecargasViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // ReSharper disable InconsistentNaming
        [Authorize(Roles = "Administrador, Suministrador")]
        [HttpGet]
        public ActionResult GetProveedorRecargaJSON(string documento = "", int opcionDocumento = 1)
        {
            long doc = Int64.Parse(documento);
            Proveedor proveedor = _logicaProveedores.GetProveedorPorDocumento(doc, opcionDocumento);
            string nombre = "";
            int id = -1;
            int leads = -1;
            int imagen = -1;
            if (proveedor != null)
            {
                id = proveedor.ProveedorId;
                leads = proveedor.LeadsDisponibles;
                if (proveedor.Persona.TipoPersona == "Natural")
                    nombre = proveedor.Persona.PrimerNombre + " " + proveedor.Persona.ApellidoPaterno;
                else if (proveedor.Persona.TipoPersona == "Juridica")
                    nombre = proveedor.Persona.RazonSocial;
                imagen = (int)proveedor.Persona.ImagenId;
            }
            
            var recargasJson = new List<Object>();
            Object o = new { ProveedorID = id, NombreProveedor = nombre, LeadsProveedor = leads, ImageProveedor = imagen };
            recargasJson.Add(o);
            return Json(recargasJson, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        [HttpGet]
        public ActionResult EjecutarRecarga(int idProveedor = 0, int idSuministrador = 0, int monto = 0)
        {
            //FALTARIAN VALIDACIONES DEL LADO SERVIDOR (ACÁ) - LAS MISMAS Q ESTAN DEL LADO DEL CLIENTE CON JQUERY

            if (Roles.IsUserInRole("Suministrador"))
            {
                _logicaSuministradores.AgregarRecarga(idProveedor, idSuministrador, monto);
                _logicaPersonas.HabilitarDeshabilitarUsuario("Proveedor", idProveedor, "Habilitar");
                _logicaSuministradores.ActualizarLeads(idSuministrador, monto);    
            }
            var recargasJson = new List<Object>();
            Object o = new { Msg = "ok" };
            recargasJson.Add(o);
            return Json(recargasJson, JsonRequestBehavior.AllowGet);
        }
    }
}
