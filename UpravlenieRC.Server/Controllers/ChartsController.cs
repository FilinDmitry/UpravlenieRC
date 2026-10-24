using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Services;

namespace UpravlenieRC.Server.Controllers
{
    [Route("api/charts")]
    [ApiController]
    [Authorize(Roles = "Администратор, Менеджер закупок")]
    public class ChartsController : ControllerBase
    {
        private readonly ChartsService _service;
        public ChartsController(ChartsService service)
        {
            _service = service;
        }
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersChart()
        {
            return Ok(await _service.GetOrdersArrivalData());
        }
    }
}
