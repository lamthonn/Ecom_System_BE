using backend_v3.Models;
using Ecom.Dto.QuanLySanPham;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Interfaces
{
    public interface ISanPhamService
    {
        public Task<PaginatedList<SanPhamDto>> GetAll(SanPhamDto request);
        public Task<List<SanPhamDto>> GetByMa([FromRoute]string ma);
        public Task<List<SanPhamDto>> create(List<SanPhamDto> request);
        public Task Edit(EditSanPhamRequest request);
        public void Delete(Guid id);
        public void DeleteAny(List<Guid> ids);
        public byte[] ExportToExcel();
        public Task<string> SaveImageFileCoverPhoto(IFormFile file);
        public Task<List<string>> SaveMutiImageFile(List<IFormFile> files);
        public void AddListImage(AddListImageProps req);

    }
}
