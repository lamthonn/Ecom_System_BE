using AutoMapper;
using Ecom.Dto.KhachHang;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Authorize]
    [Route("api/khach-hang")]
    [ApiController]
    public class KhachHangController : GenericController<account, KhachHangDto>
    {
        public KhachHangController(IGenericRepository<account> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
