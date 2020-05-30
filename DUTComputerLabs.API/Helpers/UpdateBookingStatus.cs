using System;
using System.Linq;
using System.Threading.Tasks;
using DUTComputerLabs.API.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DUTComputerLabs.API.Helpers
{
    public class UpdateBookingStatus : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var service = context.HttpContext.RequestServices.GetService<IBookingService>();
            service.GetAll()
                .Where(b => b.BookingDate < DateTime.Today && !string.Equals(b.Status, "Đã hoàn thành"))
                .ToList()
                .ForEach(b => b.Status = "Đã hoàn thành");

            service.SaveAll();
        }

        // public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        // {
        //     var resultContext = await next();

        //     var service = resultContext.HttpContext.RequestServices.GetService<IBookingService>();
        //     service.GetAll()
        //         .Where(b => b.BookingDate < DateTime.Now && !string.Equals(b.Status, "Đã hoàn thành"))
        //         .ToList()
        //         .ForEach(b => b.Status = "Đã hoàn thành");
        // }
    }
}