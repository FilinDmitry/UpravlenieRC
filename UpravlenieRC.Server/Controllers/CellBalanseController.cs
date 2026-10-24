using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;
using UpravlenieRC.Server.Services;

namespace UpravlenieRC.Server.Controllers
{
    [Route("api/CellBalanse")]
    [ApiController]
    [Authorize]
    public class CellBalanseController : ControllerBase
    {
        CellBalanceServices _service;
        public CellBalanseController(CellBalanceServices service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetCellBalanse()
        {
            return Ok(await _service.GetCellBalanse());
            /*AuthServiceController response = await _service.Auth(request);
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
            }*/
        }
    }
}
