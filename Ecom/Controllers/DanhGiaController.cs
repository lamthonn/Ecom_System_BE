using AutoMapper;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Interfaces;
using Ecom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
