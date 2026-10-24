using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Services;

namespace UpravlenieRC.Server.Controllers
{
    [Route("api/Goods")]
    [ApiController]
    [Authorize]
    public class GoodsController : ControllerBase
    {
        GoodsService _service;
        public GoodsController(GoodsService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetGood(int id)
        {
            return Ok(await _service.GetGood(id));

        }
        [HttpPut]
        [Authorize(Roles = "Администратор")]
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
                case 409:
                    return Conflict(response.message);
                default:
                    return Problem();
            }
        }
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Администратор")]
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
        [Authorize(Roles = "Администратор")]
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
        [HttpGet]
        [Route("Units")]
        public async Task<IActionResult> GetUnits()
        {
            return Ok(await _service.GetUnits());
        }
        [HttpGet]
        [Route("Manufacturers")]
        public async Task<IActionResult> GetManufacturers()
        {
            return Ok(await _service.GetManufacturers());
        }
    }
}
