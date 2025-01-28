using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace HsR.Web.API.Controllers
{
    internal class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new JsonResult(new
            {
                error = "An error occurred while processing your request."
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            base.OnException(context);
        }
    }

    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    internal class DayJControllerBase : ControllerBase { }   
}
