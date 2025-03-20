using backend_v3.Models;
using Ecom.Dto.QuanLySanPham;

namespace Ecom.Interfaces
{
    public interface ISanPhamService
    {
        public Task<PaginatedList<SanPhamDto>> GetAll(SanPhamDto request);
        public Task<SanPhamDto> GetById(string id);
        public Task<List<SanPhamDto>> create(List<SanPhamDto> request);
        public void Edit(string id, SanPhamDto request);
        public void Delete(string id);
        public void DeleteAny(List<string> ids);
        public byte[] ExportToExcel();
        public Task<string> SaveImageFileCoverPhoto(IFormFile file);
        public Task<List<string>> SaveMutiImageFile(List<IFormFile> files);
        public void AddListImage(List<string> filePath, string ma);

    }
}
