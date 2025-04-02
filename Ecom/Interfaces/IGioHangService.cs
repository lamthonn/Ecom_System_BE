using backend_v3.Models;
using Ecom.Dto.GioHang;
using Ecom.Dto.QuanLySanPham;

namespace Ecom.Interfaces
{
    public interface IGioHangService
    {
        public Task<GioHangDto> GetAll(GioHangDto request);
        public Task<ChiTietGioHangDto> Add(ChiTietGioHangDto request);
        public Task Edit(ChiTietGioHangDto request);
        public Task Delete(string id);
    }
}
