using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaGeneraliz.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministracionController : Controller
    {
        //
        // GET: /Administracion/

        public ActionResult Usuarios()
        {
            return View();
        }

    }
}
