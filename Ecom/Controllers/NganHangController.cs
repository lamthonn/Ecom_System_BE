using AutoMapper;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Authorize]
    [Route("api/ngan-hang")]
    [ApiController]
    public class NganHangController : GenericController<ngan_hang, NganHangDto>
    {
        private readonly INganHangService _nganHangService;
        public NganHangController(IGenericRepository<ngan_hang> repository, IMapper mapper, INganHangService nganHangService)
    : base(repository, mapper)
        {
            _nganHangService = nganHangService;
        }

        [HttpPost("add-bank-account")]
        public async Task<IActionResult> AddBankAccount([FromBody] NganHangDto dto)
        {
            try
            {
                var result = await _nganHangService.AddBankAccountAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Thêm tài khoản ngân hàng thất bại.", error = ex.Message });
            }
        }
        [HttpPut("{id}/set-default")]
        public async Task<IActionResult> SetDefaultBankAccount(Guid id)
        {
            try
            {
                await _nganHangService.SetDefaultBankAccountAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner Exception: {ex.InnerException.Message}";
                }
                return BadRequest(new { message = "Đặt tài khoản ngân hàng mặc định thất bại.", error = errorMessage });
            }
        }
    }
}
