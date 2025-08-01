using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HsR.Web.API.Controllers
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            var exceptionType = ex.GetType().FullName;
            var stackTrace = ex.StackTrace;

            _logger.LogError(ex, "Unhandled exception caught in CustomExceptionFilter. Exception Type: {ExceptionType}\nStack Trace: {StackTrace}",
                exceptionType, stackTrace);

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
    public class HsRControllerBase : ControllerBase { }
}