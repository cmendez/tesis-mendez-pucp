﻿using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using SistemaGeneraliz.Models.BusinessLogic;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.ViewModels;
using WebMatrix.WebData;
using SistemaGeneraliz.Models.Helpers;

namespace SistemaGeneraliz.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class UsuariosController : Controller
    {
        private  LogicaPersonas _logicaPersonas = new LogicaPersonas();
        
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Area = "";
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            Persona persona = _logicaPersonas.GetPersonaPorUsername(model.UserName);

            if (ModelState.IsValid && (persona.IsEliminado == 0) && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                //Session["Usuario"] = _logicaPersonas.GetNombrePersonaLoggeada(persona.PersonaId);
                //Session["ImagenId"] = persona.ImagenId;
                return RedirectToLocal(returnUrl);
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        //[ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult RegistrarUsuario()
        {
            if (!WebSecurity.IsAuthenticated)
            {
                return View("RegistrarUsuario");
            }
            else
            {
                if (Roles.IsUserInRole("Administrador"))
                {
                    return View("RegistrarUsuario");
                }
                return RedirectToAction("Index", "Home");
            }
        }


        #region Aplicaciones auxiliares
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Vaya a http://go.microsoft.com/fwlink/?LinkID=177550 para
            // obtener una lista completa de códigos de estado.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "El nombre de usuario ya existe. Escriba un nombre de usuario diferente.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Ya existe un nombre de usuario para esa dirección de correo electrónico. Escriba una dirección de correo electrónico diferente.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La contraseña especificada no es válida. Escriba un valor de contraseña válido.";

                case MembershipCreateStatus.InvalidEmail:
                    return "La dirección de correo electrónico especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La respuesta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La pregunta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidUserName:
                    return "El nombre de usuario especificado no es válido. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.ProviderError:
                    return "El proveedor de autenticación devolvió un error. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

                case MembershipCreateStatus.UserRejected:
                    return "La solicitud de creación de usuario se ha cancelado. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

                default:
                    return "Error desconocido. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";
            }
        }
        #endregion

        [AllowAnonymous]
        public static string GetNombrePersonaLoggeada()
        {
            LogicaPersonas _logicaPersonas2 = new LogicaPersonas();
            return _logicaPersonas2.GetNombrePersonaLoggeada(WebSecurity.CurrentUserId);
        }

        [AllowAnonymous]
        public ActionResult GetImagen(int imagenId)
        {
            try
            {
                Imagen archivo = _logicaPersonas.GetImagenPorId(imagenId);
                if (archivo.Data != null)
                    return File(archivo.Data, archivo.Mime);
                return null;
                //var file = Server.MapPath("~/Images/unknown-person.jpg");
                //using (var stream = new FileStream(file, FileMode.Open))
                //{
                //    using (MemoryStream memoryStream = new MemoryStream())
                //    {
                //        stream.CopyTo(memoryStream);
                //        return File(memoryStream.ToArray(), "image/jpg");
                //    }
                //}
            }
            catch (Exception e)
            {
                Imagen archivo2 = _logicaPersonas.GetImagenPorId(imagenId);
                if (archivo2.Data != null)
                    return File(archivo2.Data, archivo2.Mime);
                return null;
                //var file = Server.MapPath("~/Images/unknown-person.jpg");
                //using (var stream = new FileStream(file, FileMode.Open))
                //{
                //    using (MemoryStream memoryStream = new MemoryStream())
                //    {
                //        stream.CopyTo(memoryStream);
                //        return File(memoryStream.ToArray(), "image/PNG");
                //    }
                //}
            }
        }

        [AllowAnonymous]
        public static int GetImagenIdPersonaLoggeada()
        {
            LogicaPersonas _logicaPersonas2 = new LogicaPersonas();
            Persona persona = _logicaPersonas2.GetPersonaLoggeada(WebSecurity.CurrentUserId);
            int imageId = -1;
            if (persona.ImagenId != null)
            {
                imageId = (int)persona.ImagenId;
            }
            return imageId;
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult EditarMiInformacion()
        {
            if (Roles.IsUserInRole("Proveedor"))
                return RedirectToAction("EditarMiInformacion", "Proveedores");
            if (Roles.IsUserInRole("Cliente"))
                return RedirectToAction("EditarMiInformacion", "Clientes");
            if (Roles.IsUserInRole("Suministrador"))
                return RedirectToAction("EditarMiInformacion", "Suministradores");

            return null;
        }

    }
}
