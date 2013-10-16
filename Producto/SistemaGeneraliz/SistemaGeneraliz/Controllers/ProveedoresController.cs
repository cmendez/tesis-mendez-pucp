using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    public class ProveedoresController : Controller
    {
        private readonly LogicaProveedores _logicaProveedores = new LogicaProveedores();
        private readonly LogicaPersonas _logicaPersonas = new LogicaPersonas();
        private readonly LogicaUbicaciones _logicaUbicaciones = new LogicaUbicaciones();
        private readonly LogicaClientes _logicaClientes = new LogicaClientes();

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
            //string ext3 = proveedorNaturalViewModel.File.ContentType;
            //string ext4 = proveedorNaturalViewModel.File.FileName;
            //return null;
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

                if ((proveedorNaturalViewModel.File == null) || (proveedorNaturalViewModel.File.ContentLength <= 0))
                {
                    ModelState.AddModelError("", "Error: es obligatorio subir una foto");
                    ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                    ViewBag.TiposServicios = ObtenerTiposServicios();
                    return View(proveedorNaturalViewModel);
                }
                else
                {
                    var file = proveedorNaturalViewModel.File;
                    string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                    string ext2 = file.FileName;

                    if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                    {
                        ModelState.AddModelError("", "Error: la extensión de la foto solo puede ser JPG, JPEG, y PNG");
                        ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                        ViewBag.TiposServicios = ObtenerTiposServicios();
                        return View(proveedorNaturalViewModel);
                    }
                }

                Imagen foto = _logicaPersonas.AgregarFotoPersona(proveedorNaturalViewModel.File);
                proveedorNaturalViewModel.ImagenPrincipal = foto.ImagenId;
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
                Session["Usuario"] = _logicaPersonas.GetNombrePersonaLoggeada(persona.PersonaId);
                Session["ImagenId"] = persona.ImagenId;
                return RedirectToAction("Index", "Home");
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

                if ((proveedorJuridicoViewModel.File == null) || (proveedorJuridicoViewModel.File.ContentLength <= 0))
                {
                    ModelState.AddModelError("", "Error: es obligatorio subir una foto");
                    ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                    ViewBag.TiposServicios = ObtenerTiposServicios();
                    return View(proveedorJuridicoViewModel);
                }
                else
                {
                    var file = proveedorJuridicoViewModel.File;
                    string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                    string ext2 = file.FileName;

                    if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                    {
                        ModelState.AddModelError("", "Error: la extensión de la foto solo puede ser JPG, JPEG, y PNG");
                        ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                        ViewBag.TiposServicios = ObtenerTiposServicios();
                        return View(proveedorJuridicoViewModel);
                    }
                }

                Imagen foto = _logicaPersonas.AgregarFotoPersona(proveedorJuridicoViewModel.File);
                proveedorJuridicoViewModel.ImagenPrincipal = foto.ImagenId;
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
                Session["Usuario"] = _logicaPersonas.GetNombrePersonaLoggeada(persona.PersonaId);
                Session["ImagenId"] = persona.ImagenId;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            ViewBag.TiposServicios = ObtenerTiposServicios();
            return View(proveedorJuridicoViewModel);
        }

        public List<TipoServicio> ObtenerTiposServicios()
        {
            List<TipoServicio> tipoServicios = _logicaProveedores.GetTipoServicios();
            return tipoServicios;
        }

        [Authorize(Roles = "Administrador, Proveedor")]
        public ActionResult HistorialTrabajos()
        {
            return View();
        }

        // ReSharper disable InconsistentNaming
        public ActionResult HistorialTrabajos_Read([DataSourceRequest]DataSourceRequest request)
        {
            int idPersona = WebSecurity.CurrentUserId;
            Proveedor proveedor = _logicaProveedores.GetProveedorPorPersonaId(idPersona);
            List<HistorialTrabajosViewModel> listaHistorialTrabajosViewModel = new List<HistorialTrabajosViewModel>();
            if (proveedor != null)
            {
                listaHistorialTrabajosViewModel = _logicaProveedores.GetHistorialTrabajos(proveedor.ProveedorId);
            }

            return Json(listaHistorialTrabajosViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarDetallesTrabajoProveedor(int trabajoProveedorId = 0)
        {
            TrabajoProveedor trabajoProveedor = _logicaProveedores.GetTrabajoProveedor(trabajoProveedorId);
            Proveedor proveedor = _logicaProveedores.GetProveedorPorPersonaId(WebSecurity.CurrentUserId);
            if ((proveedor == null) || (trabajoProveedor.ProveedorId != proveedor.ProveedorId))
                return null;

            if (trabajoProveedor.FechaReal != null)
                trabajoProveedor.FechaReal = DateTime.ParseExact(trabajoProveedor.FechaReal.Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            return View(trabajoProveedor);
        }

        [HttpPost]
        public ActionResult EditarDetallesTrabajoProveedor(TrabajoProveedor trabajoProveedor)
        {
            _logicaProveedores.ActualizarDetallesTrabajoProveedor(trabajoProveedor);
            //return HistorialTrabajos(); //ASI NO FUNCIONA
            return RedirectToAction("HistorialTrabajos");
        }

        public ActionResult EncuestaCliente_Read([DataSourceRequest]DataSourceRequest request)
        {
            int idPersona = WebSecurity.CurrentUserId;
            Proveedor proveedor = _logicaProveedores.GetProveedorPorPersonaId(idPersona);
            List<HistorialTrabajosViewModel> listaHistorialTrabajosViewModel = new List<HistorialTrabajosViewModel>();
            if (proveedor != null)
            {
                listaHistorialTrabajosViewModel = _logicaProveedores.GetHistorialTrabajos(proveedor.ProveedorId);
            }

            return Json(listaHistorialTrabajosViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetResultadosEncuestaCliente(int trabajoProveedorId)
        {
            TrabajoProveedor trabajoProveedor = _logicaProveedores.GetTrabajoProveedor(trabajoProveedorId);
            EncuestaCliente encuestaCliente = trabajoProveedor.EncuestaCliente;
            List<RespuestaPorCriterio> respuestas = encuestaCliente.RespuestasPorCriterio.ToList();//_logicaProveedores.GetResultadosEncuestaCliente(encuestaCliente.EncuestaClienteId);
            var resultadosEncuesta = new List<Object>();
            Object obj;

            if (encuestaCliente.IsCompletada == 1)
            {
                foreach (var respuesta in respuestas)
                {
                    obj = new { Pregunta = respuesta.CriterioCalificacion.NombreCriterio, Puntaje = respuesta.PuntajeOtorgado, Respuesta = "" };
                    resultadosEncuesta.Add(obj);
                }

                //Comentarios cliente
                obj = new { Pregunta = "", Puntaje = -1, Respuesta = encuestaCliente.ComentariosCliente };
                resultadosEncuesta.Add(obj);

                //Comentarios proveedor
                int proveedorRespondio = 0;
                string comentariosProveedor = "";
                if (encuestaCliente.ComentariosProveedor != null)
                {
                    proveedorRespondio = 1;
                    comentariosProveedor = encuestaCliente.ComentariosProveedor;
                }
                obj = new { Pregunta = "", Puntaje = proveedorRespondio, Respuesta = comentariosProveedor };
                resultadosEncuesta.Add(obj);

                //Visibilidad encuesta
                obj = new { Pregunta = "", Puntaje = -1, Respuesta = encuestaCliente.IsVisible };
                resultadosEncuesta.Add(obj);
            }
            else
            {
                //el cliente no contestó la encuesta
                obj = new { Pregunta = "", Puntaje = -2, Respuesta = "" };
                resultadosEncuesta.Add(obj);
            }
            return Json(resultadosEncuesta, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GuardarComentariosProveedores(int trabajoProveedorId, string comentariosProveedor, int visibilidad)
        {
            _logicaProveedores.ActualizarComentarioProveedorEncuesta(trabajoProveedorId, comentariosProveedor, visibilidad);
            var json = new List<Object>();
            Object o = new { Msg = "ok" };
            json.Add(o);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }

}
