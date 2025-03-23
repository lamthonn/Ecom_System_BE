using AutoMapper;
using backend_v3.Models;
using Ecom.Dto.ProductTest;
using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Authorize]
    [Route("api/product")]
    [ApiController]
    public class ProductController : GenericController<san_pham, ProductTestDto>
    {
        private readonly ISanPhamService _service;
        public ProductController(IGenericRepository<san_pham> repository, IMapper mapper, ISanPhamService service)
            : base(repository, mapper) 
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
