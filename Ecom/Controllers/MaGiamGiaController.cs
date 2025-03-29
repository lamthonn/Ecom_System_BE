using AutoMapper;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Authorize]
    [Route("api/ma-giam-gia")]
    [ApiController]
    public class MaGiamGiaController : GenericController<ma_giam_gia, MaGiamGiaDto>
    {
        public MaGiamGiaController(IGenericRepository<ma_giam_gia> repository, IMapper mapper)
    : base(repository, mapper)
        {

        }
    }
}
