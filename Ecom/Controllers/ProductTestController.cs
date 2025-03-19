using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : GenericController<san_pham>
    {
        public ProductController(IGenericRepository<san_pham> repository) : base(repository) { }
    }
}
