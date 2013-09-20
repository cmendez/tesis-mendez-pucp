using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ExtensionMethods
{
    public abstract class WSController : Controller
    {
        internal object FormatSuccess(object data = null)
        {
            return new { success = true, data = data };
        }

        internal object FormatError(object message = null)
        {
            if (message == null) message = "";
            return new { success = false, message = message };
        }

        internal object Format(object dataOrMessage, bool success = true)
        {
            if (success)
                return FormatSuccess(dataOrMessage = null);
            else
                return FormatError(dataOrMessage);
        }

        internal JsonResult JsonSuccessGet(object data = null)
        {
            return Json(FormatSuccess(data), JsonRequestBehavior.AllowGet);
        }

        internal JsonResult JsonErrorGet(object message = null)
        {
            return Json(FormatError(message), JsonRequestBehavior.AllowGet);
        }

        internal JsonResult JsonSuccessPost(object data = null)
        {
            return Json(FormatSuccess(data));
        }

        internal JsonResult JsonErrorPost(object message = null)
        {
            return Json(FormatError(message));
        }

    }
}