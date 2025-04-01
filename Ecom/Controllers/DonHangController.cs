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
    }
}
