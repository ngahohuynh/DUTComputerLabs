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
            var currentPeriod = ExchangeDate();

            service.GetAll()
                .Where(b => ( (b.BookingDate < DateTime.Today) 
                    || (b.BookingDate == DateTime.Today && b.EndAt < currentPeriod) )
                    && (!string.Equals(b.Status, "Đã hoàn thành") || !string.Equals(b.Status, "Đã hủy")) )
                .ToList()
                .ForEach(b => b.Status = "Đã hoàn thành");

            service.SaveAll();
        }

        private int ExchangeDate()
        {
            var currentHour = DateTime.Now.Hour;
            var currentMinute = DateTime.Now.Minute;
            var period = 0;

            switch (currentHour)
            {
                case 7:
                    period = 1;
                    break;
                case 8:
                    period = 2;
                    break;
                case 9:
                    period = 3;
                    break;
                case 10:
                    period = 4;
                    break;
                case 11:
                    period = 5;
                    break;
                case 12:
                    period = 6;
                    break;
                case 13:
                    if (currentMinute < 30) period = 6;
                    if (currentMinute >= 30) period = 7;
                    break;
                case 14:
                    if (currentMinute < 30) period = 7;
                    if (currentMinute >= 30) period = 8;
                    break;
                case 15:
                    if (currentMinute < 30) period = 8;
                    if (currentMinute >= 30) period = 9;
                    break;
                case 16:
                    if (currentMinute < 30) period = 9;
                    if (currentMinute >= 30) period = 10;
                    break;
                case 17:
                    if (currentMinute < 30) period = 10;
                    break;
                default:
                    if (currentHour < 7) period = 0;
                    if (currentHour > 17) period = 11;
                    break;
            }

            return period;
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