using AutoMapper;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Authorize]
    [Route("api/chuong-trinh-marketing")]
    [ApiController]
    public class ChuongTrinhMarController : GenericController<chuong_trinh_marketing, ChuongTrinhMarDto>
    {
        public ChuongTrinhMarController(IGenericRepository<chuong_trinh_marketing> repository, IMapper mapper)
    : base(repository, mapper)
        {

        }
    }
}
