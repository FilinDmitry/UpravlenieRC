using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;
using UpravlenieRC.Server.Services;

namespace UpravlenieRC.Server.Controllers
{   
    [Route("api/userUpdateProfile/")]
    [ApiController]
    [Authorize]
    public class ProfileChangeController : ControllerBase
    {
        private readonly PasswordHasher<User> hasher = new PasswordHasher<User>();
        AuthAndRegService _service;
        public ProfileChangeController(AuthAndRegService service)
        {
            _service = service;
        }

        [HttpPatch("password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
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

            ResponseWithMessage response = await _service.ChangePassword(userId, request);
            switch ((int)response.Code)
            {
                case 404:
                    return NotFound(response.message);
                case 400:
                    return BadRequest(response.message);
                case 401:
                    return Unauthorized(response.message);
                case 200:
                    return Ok(response.message);
                default:
                    return Problem();
            }
        }

        [HttpPatch("email")]
        public async Task<IActionResult> UpdateEmail(ChangeEmailRequest request)
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
            ResponseWithMessage response = await _service.ChangeEmail(userId, request);
            switch ((int)response.Code)
            {
                case 404:
                    return NotFound(response.message);
                case 400:
                    return BadRequest(response.message);
                case 401:
                    return Unauthorized(response.message);
                case 409:
                    return Conflict(response.message);
                case 200:
                    return Ok(response.message);
                default:
                    return Problem();
            }
        }

        [HttpPatch("phone")]
        public async Task<IActionResult> UpdatePhone(ChangePhoneRequest request)
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
            ResponseWithMessage response = await _service.ChangePhone(userId, request);
            switch ((int)response.Code)
            {
                case 404:
                    return NotFound(response.message);
                case 400:
                    return BadRequest(response.message);
                case 401:
                    return Unauthorized(response.message);
                case 409:
                    return Conflict(response.message);
                case 200:
                    return Ok(response.message);
                default:
                    return Problem();
            }

        }
    }
}