using CLF.Service.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CLF.Web.Framework.Mvc.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
       private readonly IHostingEnvironment _hostingEnvironment;
        public HttpExceptionFilter(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public void OnException(ExceptionContext context)
        {
            if (_hostingEnvironment.IsDevelopment())
                Log.Information(context.Exception.Message);
            else
                Log.Error(context.Exception.Message);
           
            context.Result = new JsonResult(new ServiceResult<bool>(ServiceResultType.Error, context.Exception.Message));
            context.ExceptionHandled = true;
        }
    }

    public class ThrowIfExceptionAttribute:TypeFilterAttribute
    {
        public ThrowIfExceptionAttribute() : base(typeof(HttpExceptionFilter))
        {
          
        }
    }
}
