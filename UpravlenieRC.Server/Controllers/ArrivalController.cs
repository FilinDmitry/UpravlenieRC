using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpravlenieRC.Server.DTO;
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
        public async Task<IActionResult> CreateArrivalOrder(List<GoodsNeededDTOGet> goods)
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
            ResponseWithMessage response = await _service.CreateArrivalOrder(userId, goods);
            switch ((int)response.Code)
            {
                case 400:
                    return BadRequest(response.message);
                case 401:
                    return Unauthorized(response.message);
                case 201:
                    return Created();
                case 500:
                    return Problem(response.message);
                default:
                    return Problem();
            }
        }
    }
}
