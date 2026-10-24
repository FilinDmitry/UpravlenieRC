using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;
using UpravlenieRC.Server.Services;

namespace UpravlenieRC.Server.Controllers
{
    [Route("api/reg")]
    [ApiController]
    [Authorize(Roles = "Администратор")]
    
    public class RegController : ControllerBase
    {
        private readonly AuthAndRegService _service;
        public RegController(AuthAndRegService service)
        {
            _service = service;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Reg(RegRequest request)
        {
            ResponseWithMessage response = await _service.Reg(request);
            switch ((int)response.Code)
            {
                case 400:
                    return BadRequest(response.message);
                case 409:
                    return Conflict(response.message);
                case 404:
                    return NotFound(response.message);
                case 201:
                    return Created();
                default:
                    return Problem();
            }
        }
        [HttpGet]
        public async Task<IActionResult> RegGet()
        {
            return Ok(await _service.GetReg());
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser(CreateUser request)
        {
            ResponseWithMessage response = await _service.CreateUser(request);
            switch ((int)response.Code)
            {
                case 400:
                    return BadRequest(response.message);
                case 409:
                    return Conflict(response.message);
                case 201:
                    return Created();
                default:
                    return Problem();
            }
        }
        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRolesToCreate()
        {
            return Ok(await _service.GetRoles());
        }
        [HttpPost]
        [Route("{id}/accept")]
        public async Task<IActionResult> RegAssept(int id)
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
            bool Res = await _service.AcceptReg(userId, id);
            if (Res)
            {
                return Created();
            }
            else
            {
                return Problem();
            }
        }
        [HttpPost]
        [Route("{id}/decline")]
        public async Task<IActionResult> RegDecline(int id)
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
            bool Res = await _service.DenyReg(userId, id);
            if (Res)
            {
                return Ok();
            }
            else
            {
                return Problem();
            }
        }

    }
}
