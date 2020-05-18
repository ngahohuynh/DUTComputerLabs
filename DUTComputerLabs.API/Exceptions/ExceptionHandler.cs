using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DUTComputerLabs.API.Exceptions
{
    public class ExceptionHandler : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Exception is BaseException exception)
            {
                var errorResult = new 
                {
                    Message = exception.Message
                };

                context.Result = new JsonResult(errorResult)
                {
                    StatusCode = Convert.ToInt32(exception.StatusCode)
                };
                
                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}