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
        [HttpPut]
        public async Task<IActionResult> CreateGood(GoodsDTO goods)
        {
            ResponseWithMessage response = await _service.ChangeGood(0, goods);
            switch ((int)response.Code)
            {
                case 201:
                    return Created();
                case 400:
                    return BadRequest(response.message);
                case 404:
                    return NotFound(response.message);
                case 200:
                    return Ok(response.message);
                default:
                    return Problem();
            }
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> ChangeGood(int id, GoodsDTO goods)
        {
            ResponseWithMessage response = await _service.ChangeGood(id, goods);
            switch ((int)response.Code)
            {
                case 201:
                    return Created();
                case 400:
                    return BadRequest(response.message);
                case 404:
                    return NotFound(response.message);
                case 200:
                    return Ok(response.message);
                default:
                    return Problem();
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> StopGood(int id)
        {
            ResponseWithMessage response = await _service.StopGood(id);
            switch ((int)response.Code)
            {
                case 204:
                    return NoContent();
                case 404:
                    return NotFound();
                default:
                    return Problem();
            }
        }
        [HttpGet]
        [Route("Needed")]
        public async Task<IActionResult> GetNeededGoods()
        {
            return Ok(await _service.GetNeededGoods());
        }
        [HttpPatch]
        [Route("Needed")]
        public async Task<IActionResult> SetNeededGoodList(Dictionary<int, ChangeNeededRequest> dict)
        {
            var notFoundIds = new List<int>();
            var successCount = 0;

            foreach (var item in dict)
            {
                var response = await _service.SetNeededGoods(item.Key, item.Value.amount, item.Value.price);
                if (response.Code == System.Net.HttpStatusCode.NotFound)
                {
                    notFoundIds.Add(item.Key);
                }
                else
                {
                    successCount++;
                }
            }
            if (notFoundIds.Count == 0)
            {
                return Ok("Данные обновлены в полном объёме");
            }
            return Ok($"Обновлено {successCount} позиций, не найдены: {string.Join(", ", notFoundIds)}");
            
        }
        [HttpPatch]
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
