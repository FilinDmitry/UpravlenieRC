using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.Controllers
{   
    [Route("api/userUpdateProfile/")]
    [ApiController]
    [Authorize]
    public class ProfileChangeController : ControllerBase
    {
        private readonly PasswordHasher<User> hasher = new PasswordHasher<User>();
        [HttpPatch("password")]
        public async Task<IActionResult> ChangePassword(RC_SkladContext context, ChangePasswordRequest request)
        {
            int userId;

            try
            {
                userId = int.Parse(User.FindFirst("sub")?.Value);
            }
            catch (Exception)
            {
                return Unauthorized("Неверный токен");
            }
            User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            { return NotFound("Пользователь не найден"); }

            if (string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("OldPassword и NewPassword обязательны");
            }


            var CheckNew = hasher.VerifyHashedPassword(user, user.Password, request.OldPassword);
            if (CheckNew == PasswordVerificationResult.Success)
            {
                return BadRequest("Новый пароль не может совпадать со старым");
            }
            var verifyResult = hasher.VerifyHashedPassword(user, user.Password, request.OldPassword);
            if (verifyResult != PasswordVerificationResult.Success)
            {
                return Unauthorized("Текущий пароль неверен");
            }

            user.Password = hasher.HashPassword(user, request.NewPassword);

            await context.SaveChangesAsync();
            return Ok("Пароль успешно изменён");
        }

        [HttpPatch("email")]
        public async Task<IActionResult> UpdateEmail(RC_SkladContext context, ChangeEmailRequest request)
        {
            int userId;
            try
            {
                userId = int.Parse(User.FindFirst("sub")?.Value);
            }
            catch (Exception)
            {
                return Unauthorized("Неверный токен");
            }

            User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }
            var verifyResult = hasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (verifyResult != PasswordVerificationResult.Success)
            {
                return Unauthorized("Текущий пароль неверен");
            }


            if (string.IsNullOrEmpty(request.NewEmail))
            {
                return BadRequest("почта не может быть пустой");
            }    
                if (request.NewEmail == user.Email)
                {
                    return BadRequest("Новая почта должна отличаться");
                }
            if (await context.Users.FirstOrDefaultAsync(u => u.Email == request.NewEmail) != null)
            {
                return Conflict("Данный email уже зарегестрирован на другого пользователя");
            }
            user.Email = request.NewEmail;
            await context.SaveChangesAsync();
            return Ok("Контактные данные обновлены");
        }

        [HttpPatch("phone")]
        public async Task<IActionResult> UpdatePhone(RC_SkladContext context, ChangePhoneRequest request)
        {
            int userId;
            try
            {
                userId = int.Parse(User.FindFirst("sub")?.Value);
            }
            catch (Exception)
            {
                return Unauthorized("Неверный токен");
            }

            User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }
            


            if (string.IsNullOrEmpty(request.NewPhone))
            {
                return BadRequest("телефон не может быть пустым");
            }
            var verifyResult = hasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (verifyResult != PasswordVerificationResult.Success)
            {
                return Unauthorized("Текущий пароль неверен");
            }
            if (request.NewPhone == user.Phone)
                {
                    return BadRequest("Новый телефон должен отличаться");
                }
            if (await context.Users.FirstOrDefaultAsync(u => u.Phone == request.NewPhone) != null)
            {
                return Conflict("Данный телефон уже зарегестрирован на другого пользователя");
            }
            user.Phone = request.NewPhone;
            await context.SaveChangesAsync();
            return Ok("Контактные данные обновлены");
        }
    }
}