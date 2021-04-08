using Infraestructura.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;

namespace webApiMutant.Filter
{
    public class ValidacionesExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ValidationException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<ValidationException>(
                        new ValidationException(actionExecutedContext.Exception.Message),
                        new JsonMediaTypeFormatter()
                    )
                };
            }
        }
    }
}