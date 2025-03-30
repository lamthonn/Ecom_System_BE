using backend_v3.Models;
using Ecom.Dto.QuanLySanPham;
using Ecom.Dto.VanHanh;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Route("api/don-hang")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangService _service;
        public DonHangController(IDonHangService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("get-all")]
        public Task<PaginatedList<DonHangDto>> GetAll([FromQuery] DonHangDto request)
        {
            try
            {
                return _service.GetAll(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        [HttpPut]
        [Route("xu-ly/{id}")]
        public IActionResult XuLyDonHang(string id, DonHangDto request)
        {
            try
            {
                _service.XuLyDonHang(id,request);
                return Ok();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        [HttpPut]
        [Route("xu-ly-nhieu")]
        public IActionResult XuLyDonHangs(List<DonHangDto> request)
        {
            try
            {
                _service.XuLyDonHangs(request);
                return Ok();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
