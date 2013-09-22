using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGeneraliz.Models.BusinessLogic;
using SistemaGeneraliz.Models.Entities;

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
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarProveedorNatural(ProveedorNaturalViewModel proveedorNaturalViewModel)
        {
            if (ModelState.IsValid)
            {
                Persona persona = _logicaPersonas.CrearObjetoPersonaNatural(proveedorNaturalViewModel);
                Proveedor proveedor = _logicaProveedores.CrearObjetoProveedorNatural(proveedorNaturalViewModel);
                //setear la especialidad
                _logicaPersonas.AgregarPersona(persona);
                proveedor.PersonaId = persona.PersonaId;
                _logicaProveedores.AgregarProveedor(proveedor);
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
