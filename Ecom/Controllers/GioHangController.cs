using backend_v3.Models;
using Ecom.Dto.GioHang;
using Ecom.Dto.QuanLySanPham;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Route("api/gio-hang")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private readonly IGioHangService _service;
        public GioHangController(IGioHangService service) { 
            _service = service;
        }


        [HttpGet]
        [Authorize]
        [Route("get-all")]
        public Task<GioHangDto> GetAll([FromQuery] GioHangDto request)
        {
            try
            {
                return _service.GetAll(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        [HttpPost]
        [Authorize]
        [Route("created")]
        public Task Created(ChiTietGioHangDto request)
        {
            try
            {
                return _service.Add(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        [HttpPut]
        [Authorize]
        [Route("edit")]
        public Task Edit(ChiTietGioHangDto request)
        {
            try
            {
                return _service.Edit(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        [HttpDelete]
        [Authorize]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery]string id)
        {
            try
            {
                await _service.Delete(id);
                return NoContent(); // 204 No Content for successful deletion
            }
            catch (DirectoryNotFoundException) // Custom exception for not found
            {
                return NotFound(); // 404 Not Found
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error"); // 500 Internal Server Error
            }
        }

    }
}
