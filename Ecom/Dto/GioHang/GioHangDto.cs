using backend_v3.Dto.Common;

namespace Ecom.Dto.GioHang
{
    public class GioHangDto : PaginParams
    {
        public Guid? id { get; set; }
        public Guid? account_id { get; set; }
        public List<ChiTietGioHangDto>? ds_chi_tiet_gio_hang { get; set; }
    }
}
