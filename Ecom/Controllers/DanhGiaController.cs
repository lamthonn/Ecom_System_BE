using AutoMapper;
using backend_v3.Models;
using Ecom.Dto;
using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;
using Ecom.Interfaces;
using Ecom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Ecom.Services.DanhGiaService;

namespace Ecom.Controllers
{
    [Authorize]
    [Route("api/danh-gia")]
    [ApiController]
    public class DanhGiaController : GenericController<danh_gia, DanhGiaDto>
    {
        private readonly IDanhGiaService _danhGiaService;
        public DanhGiaController(IGenericRepository<danh_gia> repository, IMapper mapper, IDanhGiaService danhGiaService)
    : base(repository, mapper)
        {
            _danhGiaService = danhGiaService;
        }
        [Authorize]
        [HttpPost("danh-gia-don-hang")]
        public async Task<bool> DanhGia([FromBody] List<DanhGiaInputDto> listDanhGia,[FromQuery] Guid id)
        {
            try
            {
                return await _danhGiaService.DanhGia(listDanhGia, id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Authorize]
        [HttpGet("get-danh-gia/{ma}")]
        public async Task<List<DanhGiaDto>> GetDanhGia([FromRoute]string ma)
        {
            try
            {
                return await _danhGiaService.GetBySanPham(ma);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [Authorize]
        [HttpGet("get-all-paging")]
        public async Task<PaginatedList<DanhGiaDto>> GetAllPaging([FromQuery] DanhGiaParams request)
        {
            try
            {
                return await _danhGiaService.GetAllPaging(request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
