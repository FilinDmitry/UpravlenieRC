using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Services;

namespace UpravlenieRC.Server.Controllers
{
    [Route("api/Goods")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        GoodsService _service;
        public GoodsController(GoodsService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetGoods()
        {
            return Ok(await _service.GetGoods());
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
        [HttpGet]
        [Route("Needed")]
        public async Task<IActionResult> GetNeededGoods()
        {
            return Ok(await _service.GetNeededGoods());
        }
        [HttpPost]
        [Route("Needed/{id}")]
        public async Task<IActionResult> SetNeededGoods(int id, ChangeNeededRequest amounter)
        {
            return Ok(await _service.SetNeededGoods(id, amounter.amount, amounter.price));
        }
        [HttpGet]
        [Route("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _service.GetCategories());
        }
    }
}
