using backend_v3.Dto.Common;
using backend_v3.Models;
using Ecom.Dto.DonHang;
using Ecom.Dto.QuanLySanPham;
using Ecom.Dto.VanHanh;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> XuLyDonHang(string id, DonHangDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("ID đơn hàng không hợp lệ.");
                }

                if (request == null)
                {
                    return BadRequest("Dữ liệu yêu cầu không hợp lệ.");
                }

                await _service.XuLyDonHang(id, request);
                return NoContent(); // Trả về 204 khi thành công, không có nội dung trả về.
            }
            catch (Exception ex)
            {
                // Trả về BadRequest hoặc NotFound với thông báo lỗi cụ thể
                return BadRequest(ex.Message); // hoặc NotFound(ex.Message) nếu phù hợp
            }
        }

        [HttpPut]
        [Route("xu-ly-nhieu")]
        public async Task<IActionResult> XuLyDonHangs(List<DonHangDto> request)
        {
            try
            {
                await _service.XuLyDonHangs(request);
                return NoContent();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        [Authorize]
        [HttpGet]
        [Route("get-don-hang-by-id")]
        public async Task<DonHangUserDto> GetDonHangById([FromQuery] Guid id)
        {
            try
            {
                return await _service.GetDonHangById(id);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        [Authorize]
        [HttpGet]
        [Route("get-don-hang-by-user")]
        public async Task<PaginatedList<DonHangUserDto>> GetDonHangs([FromQuery] PaginParams param, [FromQuery] int trang_thai)
        {
            try
            {
                return await _service.GetDonHangs(param, trang_thai);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        [Authorize]
        [HttpPut]
        [Route("chuyen-trang-thai-don-hang")]
        public async Task<bool> ChuyenTrangThaiDonHang([FromQuery] Guid id, [FromQuery] int TrangThai)
        {
            try
            {
                return await _service.ChuyenTrangThaiDonHang(id, TrangThai);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
