using System.Net;

namespace DUTComputerLabs.API.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message)
        {
            
        }
    }
}