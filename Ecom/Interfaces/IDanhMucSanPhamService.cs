using backend_v3.Models;
using Ecom.Dto;
using Ecom.Dto.QuanLySanPham;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Interfaces
{
    public interface IDanhMucSanPhamService
    {
        public Task<PaginatedList<DanhMucDto>> GetAll(DanhMucDto request);
        public Task<DanhMucDto> GetById(string id);
        public Task<DanhMucDto> create(DanhMucDto request);
        public void Edit(string id ,DanhMucDto request);
        public void Delete(string id);
        public void DeleteAny(List<string> ids);
        public byte[] ExportToExcel();
    }
}
