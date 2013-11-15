using System;
using System.Collections.Generic;
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
    [Authorize]
    public class SuministradoresController : Controller
    {
        private  LogicaSuministradores _logicaSuministradores = new LogicaSuministradores();
        private  LogicaPersonas _logicaPersonas = new LogicaPersonas();
        private  LogicaUbicaciones _logicaUbicaciones = new LogicaUbicaciones();
        private  LogicaProveedores _logicaProveedores = new LogicaProveedores();

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
        
        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult EditarMiInformacion()
        {
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(WebSecurity.CurrentUserId);
            SuministradorJuridicoViewModel suministradorJuridicoViewModel = _logicaSuministradores.GetSuministradorViewModel(suministrador); 
            return View("EditarSuministradorJuridico", suministradorJuridicoViewModel);
        }

        [HttpPost]
        public ActionResult EditarMiInformacion_Juridico(SuministradorJuridicoViewModel suministradorJuridicoViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //habria que ver la manera de setear un objeto File dummy en el viewmodel por si el usuario no desea actualizar su foto,
                    //pero por ahora simplemente lo obligamos a que si lo haga
                    if ((suministradorJuridicoViewModel.File == null) || (suministradorJuridicoViewModel.File.ContentLength <= 0))
                    {
                        ModelState.AddModelError("", "Error: es obligatorio subir una foto");
                        ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                        return View("EditarSuministradorJuridico", suministradorJuridicoViewModel);
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
                            return View("EditarSuministradorJuridico", suministradorJuridicoViewModel);
                        }
                    }

                    if (suministradorJuridicoViewModel.Password != "password")
                    {
                        if (!WebSecurity.ChangePassword(suministradorJuridicoViewModel.RUC, suministradorJuridicoViewModel.OldPassword, suministradorJuridicoViewModel.Password))
                        {
                            ModelState.AddModelError("", "Error: ingrese bien las contraseñas");
                            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
                            return View("EditarSuministradorJuridico", suministradorJuridicoViewModel);
                        }
                    }

                    Imagen foto = _logicaPersonas.AgregarFotoPersona(suministradorJuridicoViewModel.File);
                    suministradorJuridicoViewModel.ImagenPrincipal = foto.ImagenId;
                    Persona persona = _logicaPersonas.ModificarObjetoPersonaJuridico(suministradorJuridicoViewModel);
                    Suministrador suministrador = _logicaSuministradores.ModificarObjetoSuministradorJuridico(suministradorJuridicoViewModel);
                    _logicaPersonas.ActualizarPersona(persona);
                    _logicaSuministradores.ActualizarSuministrador(suministrador);
                    UbicacionPersona ubicacion = _logicaUbicaciones.ModificarObjetoUbicacionPersonaJuridica(suministradorJuridicoViewModel, persona);
                    _logicaUbicaciones.ActualizarUbicacion(ubicacion);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            ViewBag.Distritos = _logicaPersonas.GetDistritos(); //solo para Lima, si uso otras ciudades, usar ajax en la vista
            return View("EditarSuministradorJuridico", suministradorJuridicoViewModel);
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
                listaRecargasViewModel = _logicaSuministradores.GetListaRecargasSuministrador(suministrador);
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

        [AllowAnonymous]
        public ActionResult BuscarProductos()
        {
            ViewBag.Categorias = _logicaSuministradores.GetCategoriasProducto();
            ViewBag.Distritos = _logicaSuministradores.GetDistritos();
            ViewBag.Suministradores = _logicaSuministradores.GetSuministradores();
            return View();
        }

        [AllowAnonymous]
        public ActionResult Productos_Read([DataSourceRequest] DataSourceRequest request, string nombreProducto = "", int categoriaId = -1, int distritoId = -1, int suministradorId = -1)
        {
            List<ProductosViewModel> listaProductosViewModel = _logicaSuministradores.GetProductosCatalogo(nombreProducto, categoriaId, distritoId, suministradorId);
            return Json(listaProductosViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetImagen(int imagenId)
        {
            try
            {
                Imagen archivo = _logicaPersonas.GetImagenPorId(imagenId);
                if (archivo.Data != null)
                    return File(archivo.Data, archivo.Mime);
                var file = Server.MapPath("~/Images/unknown-person.jpg");
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        return File(memoryStream.ToArray(), "image/jpg");
                    }
                }
            }
            catch (Exception e)
            {
                var file = Server.MapPath("~/Images/unknown-person.jpg");
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        return File(memoryStream.ToArray(), "image/PNG");
                    }
                }
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDetallesProductoJSON(int productoId, bool reporte)
        {
            var productoJson = new List<Object>();
            Producto producto = _logicaSuministradores.GetDetallesProducto(productoId, reporte);
            if (producto != null)
            {
                Object o = new
                {
                    NombreCompleto = producto.NombreCompleto,
                    Descripcion = producto.Descripcion,
                    ImagenId = producto.ImagenId,
                    Precio = producto.Precio,
                    Categoria = producto.CategoriaProducto.NombreCategoria,
                    Visible = producto.IsVisible == 1 ? "Sí" : "No",
                    Suministrador = producto.Suministrador.Persona.RazonSocial,
                    SuministradorId = producto.SuministradorId,
                    FechaRegistro = producto.FechaRegistro.ToString("dd/MM/yyyy HH:mm")
                };

                productoJson.Add(o);
            }
            return Json(productoJson, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDetallesSuministradorJSON(int suministradorId)
        {
            var suministradorJson = new List<Object>();
            Suministrador suministrador = _logicaSuministradores.GetSuministrador(suministradorId);
            if (suministrador != null)
            {
                Object o = new
                {
                    nombreSuministrador = suministrador.Persona.RazonSocial,
                    imagenSuministrador = suministrador.Persona.ImagenId,
                    rucSuministrador = suministrador.Persona.RUC,
                    telefonoSuministrador = suministrador.Persona.Telefono1,
                    telefonoSuministrador2 = suministrador.Persona.Telefono2 ?? "",
                    telefonoSuministrador3 = suministrador.Persona.Telefono3 ?? "",
                    emailSuministrador = suministrador.Persona.Email1 ?? "",
                    emailSuministrador2 = suministrador.Persona.Email2 ?? "",
                    webSuministrador = suministrador.PaginaWeb ?? "",
                    acercadeSuministrador = suministrador.AcercaDeMi ?? "",
                    facebookSuministrador = suministrador.Facebook ?? ""
                };

                suministradorJson.Add(o);
            }
            return Json(suministradorJson, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDetallesUbicacionesJSON(int suministradorId)
        {
            var ubicacionesJson = new List<Object>();
            List<UbicacionPersona> ubicaciones = _logicaSuministradores.GetUbicacionesSuministrador(suministradorId);
            foreach (var ubicacion in ubicaciones)
            {
                if (ubicacion != null)
                {
                    Object o = new
                    {
                        distrito = ubicacion.Distrito.NombreDistrito,
                        direccion = ubicacion.Direccion,
                        referencia = ubicacion.Referencia,
                        latitud = ubicacion.Latitud,
                        longitud = ubicacion.Longitud,
                    };

                    ubicacionesJson.Add(o);
                    //Object o2 = new
                    //{
                    //    distrito = ubicacion.Distrito.NombreDistrito,
                    //    direccion = ubicacion.Direccion,
                    //    referencia = ubicacion.Referencia,
                    //    latitud = ubicacion.Latitud + 0.00075,
                    //    longitud = ubicacion.Longitud + 0.00075,
                    //};
                    //ubicacionesJson.Add(o2);//para pruebas
                }
            }
            return Json(ubicacionesJson, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult EditarProductos()
        {
            int idPersona = WebSecurity.CurrentUserId;
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(idPersona);

            if (suministrador == null)
            {
                suministrador = new Suministrador();
            }

            ViewBag.Suministrador = suministrador;

            return View("MisProductos");
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult MisProductos_Read([DataSourceRequest] DataSourceRequest request, int suministradorId = -1)
        {
            List<ProductosViewModel> listaProductosViewModel = _logicaSuministradores.GetProductosSuministradorCatalogo(suministradorId);
            return Json(listaProductosViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult AgregarProducto()
        {
            int idPersona = WebSecurity.CurrentUserId;
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(idPersona);
            ViewBag.SuministradorId = suministrador.SuministradorId;
            //ViewBag.Categorias = _logicaSuministradores.GetCategoriasProducto();
            List<CategoriaProducto> categorias = _logicaSuministradores.GetCategoriasProducto();
            categorias.RemoveAt(0);
            ViewBag.Categorias = new SelectList(categorias, "CategoriaProductoId", "NombreCategoria", null);
            return View();
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        [HttpPost]
        public ActionResult AgregarProducto(ProductosViewModel productoViewModel)
        {
            int idPersona = WebSecurity.CurrentUserId;
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(idPersona);
            List<CategoriaProducto> categorias = _logicaSuministradores.GetCategoriasProducto();
            categorias.RemoveAt(0);
            ViewBag.SuministradorId = suministrador.SuministradorId;
            ViewBag.Categorias = new SelectList(categorias, "CategoriaProductoId", "NombreCategoria", null);

            if (ModelState.IsValid)
            {
                if ((productoViewModel.File == null) || (productoViewModel.File.ContentLength <= 0))
                {
                    ModelState.AddModelError("", "Error: es obligatorio subir una imagen");
                    return View(productoViewModel);
                }
                else
                {
                    var file = productoViewModel.File;
                    string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                    string ext2 = file.FileName;

                    if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                    {
                        ModelState.AddModelError("", "Error: la extensión de la imagen solo puede ser JPG, JPEG, y PNG");
                        return View(productoViewModel);
                    }
                }

                Imagen foto = _logicaSuministradores.AgregarImagenProducto(productoViewModel.File);
                productoViewModel.ImagenId = foto.ImagenId;
                _logicaSuministradores.AgregarProducto(productoViewModel);
                return RedirectToAction("EditarProductos");
            }

            return View(productoViewModel);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult ModificarProducto(int productoId = -1)
        {
            ProductosViewModel productoViewModel = _logicaSuministradores.GetProductoViewModel(productoId);
            List<CategoriaProducto> categorias = _logicaSuministradores.GetCategoriasProducto();
            categorias.RemoveAt(0);
            ViewBag.Categorias = new SelectList(categorias, "CategoriaProductoId", "NombreCategoria", productoViewModel.CategoriaProductoId);
            return View(productoViewModel);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        [HttpPost]
        public ActionResult ModificarProducto(ProductosViewModel productoViewModel)
        {
            List<CategoriaProducto> categorias = _logicaSuministradores.GetCategoriasProducto();
            categorias.RemoveAt(0);
            ViewBag.Categorias = new SelectList(categorias, "CategoriaProductoId", "NombreCategoria", productoViewModel.CategoriaProductoId);

            if (ModelState.IsValid)
            {
                if ((productoViewModel.File != null) && (productoViewModel.File.ContentLength > 0))
                {
                    var file = productoViewModel.File;
                    string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);

                    if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                    {
                        ModelState.AddModelError("", "Error: la extensión de la foto solo puede ser JPG, JPEG, y PNG");
                        return View(productoViewModel);
                    }

                    Imagen foto = _logicaSuministradores.AgregarImagenProducto(productoViewModel.File);
                    productoViewModel.ImagenId = foto.ImagenId;
                }

                _logicaSuministradores.ModificarProducto(productoViewModel);
                return RedirectToAction("EditarProductos");
            }

            return View(productoViewModel);
        }

        [AllowAnonymous]
        public ActionResult OfertasPromocionesDescuentos()
        {
            //ViewBag.Categorias = _logicaSuministradores.GetCategoriasProducto();
            //ViewBag.Distritos = _logicaSuministradores.GetDistritos();
            //ViewBag.Suministradores = _logicaSuministradores.GetSuministradores();
            if ((WebSecurity.IsAuthenticated) && (Roles.GetRolesForUser()[0] == "Proveedor"))
            {
                Proveedor proveedor = _logicaProveedores.GetProveedorPorPersonaId(WebSecurity.CurrentUserId);
                ViewBag.ProveedorId = proveedor.ProveedorId;
                ViewBag.EsProveedor = 1;
                ViewBag.leadsProveedor = proveedor.LeadsDisponibles;
            }
            else
            {
                ViewBag.ProveedorId = -1;
                ViewBag.EsProveedor = 0;
                ViewBag.LeadsProveedor = -1;
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult OfertasPromosDsctos_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<OfertasPromosDsctosViewModel> listaOfertasPromosDsctosViewModel = _logicaSuministradores.GetOfertasPromosDsctosCatalogo();
            return Json(listaOfertasPromosDsctosViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDetallesOfertaPromoDsctoJSON(int ofertaPromoDsctoId)
        {
            var productoJson = new List<Object>();
            OfertaPromoDscto ofertaPromoDscto = _logicaSuministradores.GetOfertaPromoDscto(ofertaPromoDsctoId);
            if (ofertaPromoDscto != null)
            {
                Object o = new
                {
                    OfertaPromoDsctoId = ofertaPromoDscto.OfertaPromoDsctoId,
                    Tipo = ofertaPromoDscto.Tipo,
                    NombreCorto = ofertaPromoDscto.NombreCorto.Length > 19 ? ofertaPromoDscto.NombreCorto.Substring(0, 19) + "..." : ofertaPromoDscto.NombreCorto,
                    NombreCompleto = ofertaPromoDscto.NombreCompleto,
                    Descripcion = ofertaPromoDscto.Descripcion,
                    ImagenPrincipalId = (int)ofertaPromoDscto.ImagenPrincipalId,
                    ImagenBannerId = (int)ofertaPromoDscto.ImagenBannerId,
                    CostoEnLeads = ofertaPromoDscto.CostoEnLeads,
                    Suministrador = ofertaPromoDscto.Suministrador.Persona.RazonSocial,
                    SuministradorId = ofertaPromoDscto.SuministradorId,
                    CantidadDisponible = ofertaPromoDscto.CantidadDisponible,
                    IsAdquiribleConLeads = ofertaPromoDscto.IsAdquiribleConLeads == 1 ? "Sí" : "No",
                    FechaRegistro = ofertaPromoDscto.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                    FechaInicioString = ofertaPromoDscto.FechaInicio.ToString("dd/MM/yyyy"),
                    FechaFinString = ofertaPromoDscto.FechaFin.ToString("dd/MM/yyyy"),
                    Visible = ofertaPromoDscto.IsVisible == 1 ? "Sí" : "No",
                    IsEliminado = ofertaPromoDscto.IsEliminado
                };

                productoJson.Add(o);
            }
            return Json(productoJson, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Proveedor")]
        [HttpGet]
        public ActionResult SepararOfertaPromoDscto(int proveedorId, int ofertaId, int costo)
        {
            _logicaSuministradores.SepararOfertaPromoDscto(proveedorId, ofertaId, costo);
            var json = new List<Object>();
            Object o = new { Msg = "ok" };
            json.Add(o);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult EditarOfertasPromosDsctos()
        {
            int idPersona = WebSecurity.CurrentUserId;
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(idPersona);

            if (suministrador == null)
            {
                suministrador = new Suministrador();
            }

            ViewBag.Suministrador = suministrador;

            return View("MisOfertasPromosDsctos");
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult MisOfertasPromosDsctos_Read([DataSourceRequest] DataSourceRequest request, int suministradorId = -1)
        {
            List<OfertasPromosDsctosViewModel> listaOfertasPromosDsctosViewModel = _logicaSuministradores.GetOfertasPromosDsctosSuministradorCatalogo(suministradorId);
            return Json(listaOfertasPromosDsctosViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult AgregarOfertaPromoDscto()
        {
            int idPersona = WebSecurity.CurrentUserId;
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(idPersona);
            ViewBag.SuministradorId = suministrador.SuministradorId;
            return View();
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        [HttpPost]
        public ActionResult AgregarOfertaPromoDscto(OfertasPromosDsctosViewModel ofertasPromoDsctoViewModel)
        {
            //return null;
            int idPersona = WebSecurity.CurrentUserId;
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(idPersona);
            ViewBag.SuministradorId = suministrador.SuministradorId;

            if (ModelState.IsValid)
            {
                if ((ofertasPromoDsctoViewModel.File1 == null) || (ofertasPromoDsctoViewModel.File1.ContentLength <= 0))
                {
                    ModelState.AddModelError("", "Error: es obligatorio subir una imagen");
                    return View(ofertasPromoDsctoViewModel);
                }
                else
                {
                    var file = ofertasPromoDsctoViewModel.File1;
                    string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                    if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                    {
                        ModelState.AddModelError("", "Error: la extensión de la imagen solo puede ser JPG, JPEG, y PNG");
                        return View(ofertasPromoDsctoViewModel);
                    }
                }

                if (ofertasPromoDsctoViewModel.FechaInicio.CompareTo(ofertasPromoDsctoViewModel.FechaFin) >= 0)
                {
                    ModelState.AddModelError("", "Error: la fecha fin tiene que ser mayor que la fecha de inicio");
                    return View(ofertasPromoDsctoViewModel);
                }

                Imagen foto = _logicaSuministradores.AgregarImagenProducto(ofertasPromoDsctoViewModel.File1);
                ofertasPromoDsctoViewModel.ImagenPrincipalId = foto.ImagenId;
                ofertasPromoDsctoViewModel.ImagenBannerId = foto.ImagenId;
                _logicaSuministradores.AgregarOfertaPromoDscto(ofertasPromoDsctoViewModel);
                return RedirectToAction("EditarOfertasPromosDsctos");
            }

            return View(ofertasPromoDsctoViewModel);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult ModificarOfertaPromoDscto(int ofertaPromoDsctoId = -1)
        {
            OfertasPromosDsctosViewModel ofertaPromoDsctoViewModel = _logicaSuministradores.GetOfertaPromoDsctoViewModel(ofertaPromoDsctoId);
            return View(ofertaPromoDsctoViewModel);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        [HttpPost]
        public ActionResult ModificarOfertaPromoDscto(OfertasPromosDsctosViewModel ofertasPromoDsctoViewModel)
        {
            int idPersona = WebSecurity.CurrentUserId;
            Suministrador suministrador = _logicaSuministradores.GetSuministradorPorPersonaId(idPersona);
            ViewBag.SuministradorId = suministrador.SuministradorId;

            if (ModelState.IsValid)
            {
                if ((ofertasPromoDsctoViewModel.File1 != null) && (ofertasPromoDsctoViewModel.File1.ContentLength > 0))
                {
                    var file = ofertasPromoDsctoViewModel.File1;
                    string ext = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
                    if ((ext != "jpg") && (ext != "jpeg") && (ext != "png"))
                    {
                        ModelState.AddModelError("", "Error: la extensión de la imagen solo puede ser JPG, JPEG, y PNG");
                        return View(ofertasPromoDsctoViewModel);
                    }

                    Imagen foto = _logicaSuministradores.AgregarImagenProducto(ofertasPromoDsctoViewModel.File1);
                    ofertasPromoDsctoViewModel.ImagenPrincipalId = foto.ImagenId;
                    ofertasPromoDsctoViewModel.ImagenBannerId = foto.ImagenId;
                }

                if (ofertasPromoDsctoViewModel.FechaInicio.CompareTo(ofertasPromoDsctoViewModel.FechaFin) >= 0)
                {
                    ModelState.AddModelError("", "Error: la fecha fin tiene que ser mayor que la fecha de inicio");
                    return View(ofertasPromoDsctoViewModel);
                }

                _logicaSuministradores.ModificarOfertaPromoDscto(ofertasPromoDsctoViewModel);
                return RedirectToAction("EditarOfertasPromosDsctos");
            }

            return View(ofertasPromoDsctoViewModel);
        }
        
        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult Demanda_OfertasPromosDsctos()
        {
            return View();
        }

        // ReSharper disable InconsistentNaming
        public ActionResult Demanda_OfertasPromosDsctos_Read([DataSourceRequest]DataSourceRequest request, string fechaInicio, string fechaFin)
        {
            List<OfertasPromosDsctosViewModel> ofertasPromosDsctosViewModels = new List<OfertasPromosDsctosViewModel>();
            ofertasPromosDsctosViewModels = _logicaSuministradores.Demanda_OfertasPromosDsctos(fechaInicio, fechaFin);
            return Json(ofertasPromosDsctosViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Suministrador")]
        public ActionResult Demanda_Productos()
        {
            return View();
        }

        // ReSharper disable InconsistentNaming
        public ActionResult Demanda_Productos_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<DemandaProductosViewModel> demandaProductosViewModels = new List<DemandaProductosViewModel>();
            demandaProductosViewModels = _logicaSuministradores.Demanda_Productos_Read();
            return Json(demandaProductosViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}

