using backend_v3.Models;
using Ecom.Dto.QuanLySanPham;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhSachSanPhamController : ControllerBase
    {
        private readonly ISanPhamService _service;
        public DanhSachSanPhamController(ISanPhamService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-all")]
        public Task<PaginatedList<SanPhamDto>> GetAll([FromQuery] SanPhamDto request)
        {
            try
            {
                return _service.GetAll(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
