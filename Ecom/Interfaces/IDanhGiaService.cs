using backend_v3.Dto.Common;
using backend_v3.Models;
using Ecom.Dto;
using static Ecom.Services.DanhGiaService;

namespace Ecom.Interfaces
{
    public interface IDanhGiaService
    {
        Task<bool> DanhGia(List<DanhGiaInputDto> listDanhGia, Guid donHangId);

        Task<PaginatedList<DanhGiaDto>> GetAllPaging(DanhGiaParams param);
    }
}
