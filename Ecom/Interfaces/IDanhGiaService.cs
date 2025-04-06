using backend_v3.Dto.Common;
using backend_v3.Models;
using Ecom.Dto;
using Ecom.Services;
using static Ecom.Services.DanhGiaService;

namespace Ecom.Interfaces
{
    public interface IDanhGiaService
    {
        public Task<bool> DanhGia(List<DanhGiaInputDto> listDanhGia, Guid donHangId);
        public Task<List<DanhGiaDto>> GetBySanPham(string ma);
        Task<PaginatedList<DanhGiaDto>> GetAllPaging(DanhGiaParams param);
    }
}
