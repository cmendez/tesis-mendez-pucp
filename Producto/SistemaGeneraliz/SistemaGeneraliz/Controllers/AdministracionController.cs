using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}
