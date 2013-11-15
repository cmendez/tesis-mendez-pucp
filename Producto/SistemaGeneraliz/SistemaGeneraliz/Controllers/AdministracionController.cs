using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SistemaGeneraliz.Models.BusinessLogic;
using SistemaGeneraliz.Models.ViewModels;

namespace SistemaGeneraliz.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministracionController : Controller
    {
        private readonly LogicaProveedores _logicaProveedores = new LogicaProveedores();
        private readonly LogicaSuministradores _logicaSuministradores = new LogicaSuministradores();
        //
        // GET: /Administracion/

        public ActionResult Usuarios()
        {
            return View();
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
    }
}
