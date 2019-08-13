using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLF.Service.Account;
using CLF.Service.DTO.Account;
using CLF.Service.DTO.Core;
using CLF.Web.Framework.Mvc.Filters;
using CLF.Web.Mvc.Models.Account;
using Microsoft.AspNetCore.Mvc;


namespace CLF.Web.Mvc.Controllers
{
    public class PermissionController : BaseController
    {
      private readonly  IAccountService _accountService;
        public PermissionController(IAccountService accountService)
        {
            this._accountService = accountService;
        }
        public ActionResult Index()
        {
            var model = new PermissionModel();
            return View(model);
        }
        public JsonResult LoadPermissions(int start, int length,string controllerName,string actionName)
        {
            var result = _accountService.FindPagenatedListWithCount(start, length, controllerName, actionName);
            return Json(result);
        }

        public PartialViewResult Form(int? id)
        {
            PermissionDTO model = new PermissionDTO();
            if (id.GetValueOrDefault() != 0)
            {
                model = _accountService.GetPermissionById(id.Value);
            }
            return PartialView("_Form",model);
        }

        [ThrowIfException]
        [AutoValidateAntiforgeryToken]
        public JsonResult CreateOrUpdate(PermissionDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = false;
                if (model.Id != 0)
                {
                    result = _accountService.ModifyPermission(model);
                }
                else
                {
                    result = _accountService.AddPermission(model);
                }
                return ThrowJsonMessage(result);
            }
            return ThrowJsonMessage(false, GetModelStateErrorMessage());
        }

        [ThrowIfException]
        public JsonResult Delete(List<int> ids)
        {
            var result = _accountService.DeletePermissions(ids);
            return ThrowJsonMessage(result);
        }
    }
}
