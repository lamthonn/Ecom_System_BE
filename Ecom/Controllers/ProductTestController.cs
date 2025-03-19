using AutoMapper;
using Ecom.Dto.ProductTest;
using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : GenericController<san_pham, ProductTestDto>
    {
        public ProductController(IGenericRepository<san_pham> repository, IMapper mapper)
            : base(repository, mapper) { }
    }

}
