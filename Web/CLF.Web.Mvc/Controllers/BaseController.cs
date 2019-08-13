using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLF.Service.Core;
using Microsoft.AspNetCore.Mvc;

namespace CLF.Web.Mvc.Controllers
{
    public class BaseController : Controller
    {
        protected JsonResult GetServiceJsonResult<T>(Func<T> func)
        {
            var result = ServiceWrapper.Invoke(
                "CLF.Web.Mvc.Controllers.BaseController",
                "GetServiceJsonResult",
                func);

            return Json(result);
        }

       public string GetModelStateErrorMessage()
        {
            string message = string.Empty;
            foreach (var value in ModelState.Values.Where(o => o.Errors.Any()))
            {
                if (!string.IsNullOrEmpty(value.Errors.FirstOrDefault().ErrorMessage))
                {
                    message = value.Errors.FirstOrDefault().ErrorMessage;
                    break;
                }
            }
            return message;
        }

        public virtual JsonResult ThrowJsonMessage(bool success, string message=null)
        {
            return Json(new ServiceResult<bool>(success ? ServiceResultType.Success : ServiceResultType.Error, message));
        }
    }
}
