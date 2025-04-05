using backend_v3.Models;
using Ecom.Dto;

namespace Ecom.Interfaces
{
    public interface IDanhGiaService
    {
        public Task<bool> DanhGia(List<DanhGiaInputDto> listDanhGia, Guid donHangId);
    }
}
