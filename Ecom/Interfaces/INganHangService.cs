using Ecom.Dto;

namespace Ecom.Interfaces
{
    public interface INganHangService
    {
        public Task<NganHangDto> AddBankAccountAsync(NganHangDto dto);
        public Task SetDefaultBankAccountAsync(Guid id);
    }
}
