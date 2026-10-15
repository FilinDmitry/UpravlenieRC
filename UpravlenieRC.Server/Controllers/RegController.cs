using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.Controllers
{
    [Route("api/reg")]
    [ApiController]
    [AllowAnonymous]
    public class RegController : ControllerBase
    {
        [HttpPost]
        public IActionResult Reg(RC_SkladContext context, RegRequest request)
        {
            return Ok();
        }
    }
}
