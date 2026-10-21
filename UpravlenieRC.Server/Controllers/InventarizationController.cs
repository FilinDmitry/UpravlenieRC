using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Services;
namespace UpravlenieRC.Server.Controllers
{
    [Route("api/Inventarization")]
    [ApiController]
    [AllowAnonymous]
    public class InventarizationController : ControllerBase
    {
        InvantarizationService _service;
        public InventarizationController(InvantarizationService service)
        {
            _service = service;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetExcel()
        {
             FileData fd = await _service.GetInventarizationExcel();  
             return File(fd.content, fd.type, fd.filename);
            

        }
    }
}
