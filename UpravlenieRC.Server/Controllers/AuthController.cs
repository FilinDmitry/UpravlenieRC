using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;
using UpravlenieRC.Server.Services;
namespace UpravlenieRC.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        PasswordHasher<User> hasher = new PasswordHasher<User>();
        private readonly AuthAndRegService _service;
        public AuthController(AuthAndRegService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Auth(AuthRequest request)
        {
            AuthServiceController response = await _service.Auth(request);
            switch ((int)response.Code)
            {
                case 200:
                    return Ok(new
                    {
                        access_token = response.token,
                        token_type = "Bearer"
                    });
                case 401:
                    return Unauthorized(response.message);
                default:
                    return Problem();
            }
        }

        

        
    }
}
