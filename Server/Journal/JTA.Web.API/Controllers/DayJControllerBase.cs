using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace JTA.Web.API.Controllers
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
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
    public class DayJControllerBase : ControllerBase { }   
}
