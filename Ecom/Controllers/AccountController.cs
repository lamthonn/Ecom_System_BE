using AutoMapper;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : GenericController<account, accountDetailDto>
    {
        public AccountController(IGenericRepository<account> repository, IMapper mapper)
    : base(repository, mapper)
        {
            
        }
    }
}
