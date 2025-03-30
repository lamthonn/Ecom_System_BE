using backend_v3.Models;
using Ecom.Dto.VanHanh;

namespace Ecom.Interfaces
{
    public interface IDonHangService
    {
        public Task<PaginatedList<DonHangDto>> GetAll(DonHangDto request);
        public Task XuLyDonHang(string id, DonHangDto request);
        public Task XuLyDonHangs(List<DonHangDto> request);
    }
}
