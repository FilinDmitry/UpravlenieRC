using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpravlenieRC.Server.Services;

namespace UpravlenieRC.Server.Controllers
{
    [Route("api/Arrival")]
    [ApiController]
    public class ArrivalController : ControllerBase
    {
        ArrivalService _service;
        public ArrivalController(ArrivalService service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("Order")]
        public async Task<IActionResult> CreateArrivalOrder()
        {
            await _service.CreateArrivalOrder();
            return Ok();
        }
    }
}
