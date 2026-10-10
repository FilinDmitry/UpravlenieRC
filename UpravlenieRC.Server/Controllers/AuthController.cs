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
        [HttpPost]
        public IActionResult Auth(RC_SkladContext context, AuthRequest request)
        {
            User? find_user = context.Users.FirstOrDefault(
        i => i.Login == request.Login);

            if (find_user == null || string.IsNullOrWhiteSpace(request.Password))
                return Unauthorized("Введен неверный логин или пароль");

            var result = hasher.VerifyHashedPassword(
                find_user, find_user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Введен неверный логин или пароль");

            return Ok(new
            {
                access_token = JWT.CreateToken(find_user),
                token_type = "Bearer"
            });
        }

        
    }
}
