using System.Net;

namespace DUTComputerLabs.API.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message) : base(HttpStatusCode.Forbidden, message)
        {
            
        }
    }
}