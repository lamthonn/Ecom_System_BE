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

    }
}
