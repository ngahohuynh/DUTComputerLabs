using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUTComputerLabs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthsController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public UserToken Login(UserForLogin user) => _service.Login(user);
    }
}