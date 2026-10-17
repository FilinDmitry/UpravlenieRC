using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.Controllers
{
    [Route("api/reg")]
    [ApiController]
    [Authorize(Roles = "Администратор")]
    
    public class RegController : ControllerBase
    {
        private readonly PasswordHasher<User> hasher = new PasswordHasher<User>();
        private readonly PasswordHasher<RegestrationRequest> regHasher = new PasswordHasher<RegestrationRequest>();
        private int minPasswordLength = 4;
        private int minLoginLength = 4;
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Reg(RC_SkladContext context, RegRequest request)
        {
            if (string.IsNullOrEmpty(request.INN) || string.IsNullOrEmpty(request.message))
            {
                return BadRequest("Сообщение и ИНН не могут быть пустыми");
            }
            if (request.Password.Length < minPasswordLength)
            {
                return BadRequest($"Пароль должен быть длинее {minPasswordLength} символов");
            }
            if (request.Login.Length < minLoginLength)
            {
                return BadRequest($"Логин должен быть длинее {minLoginLength} символов");
            }
            if ((await context.Users.FirstOrDefaultAsync(u => u.Login == request.Login)) != null)
            {
                return Conflict("Пользователь с таким логином уже существует");
            }
            Partner? partner = await context.Partners.FirstOrDefaultAsync(p => p.Iin == request.INN);
            if (partner == null)
            {
                return NotFound("Организация с указанным ИНН не обнаружена");
            }


            RegestrationRequest regestration = new RegestrationRequest()
            {
                Description = request.message,
                Login = request.Login,
                PasswordHash = request.Password,
                Partherid = partner.Id
            };
            regestration.PasswordHash = regHasher.HashPassword(regestration, request.Password);
            await context.SaveChangesAsync();
            return Created();
        }
        [HttpPost]
        [Route("{id}/accept")]
        public IActionResult RegAssept(RC_SkladContext context, int id, RegRequest request)
        {
            return Ok();
        }
        [HttpPost]
        [Route("{id}/decline")]
        public IActionResult RegDecline(RC_SkladContext context, int id, RegRequest request)
        {
            return Ok();
        }
    }
}
