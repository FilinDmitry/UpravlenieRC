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
        [HttpPost]
        [Route("{id}/accept")]
        public IActionResult RegAssept(int id, RegRequest request)
        {
            return Ok();
        }
        [HttpPost]
        [Route("{id}/decline")]
        public IActionResult RegDecline(int id, RegRequest request)
        {
            return Ok();
        }
    }
}
