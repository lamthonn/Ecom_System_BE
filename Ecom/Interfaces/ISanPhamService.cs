using backend_v3.Models;
using Ecom.Dto.QuanLySanPham;

namespace Ecom.Interfaces
{
    public interface ISanPhamService
    {
        public Task<PaginatedList<SanPhamDto>> GetAll(SanPhamDto request);
        public Task<SanPhamDto> GetById(string id);
        public Task<SanPhamDto> create(SanPhamDto request);
        public void Edit(string id, SanPhamDto request);
        public void Delete(string id);
        public void DeleteAny(List<string> ids);
        public byte[] ExportToExcel();
    }
}
