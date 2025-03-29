using Ecom.Dto.VanHanh;

namespace Ecom.Interfaces
{
    public interface IPhieuNhapKhoService
    {
        public Task<PhieuNhapKhoDto> Create(PhieuNhapKhoDto request);
        public Task<PhieuNhapKhoDto> GetById(string id);
        public Task<PhieuNhapKhoDto> Edit(string id, PhieuNhapKhoDto request);
        public void XuLyPhieuNhap(string id);
        public byte[] ExportToExcel();

    }
}
