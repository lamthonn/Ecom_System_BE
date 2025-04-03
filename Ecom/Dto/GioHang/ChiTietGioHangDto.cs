using backend_v3.Dto.Common;
using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;

namespace Ecom.Dto.GioHang
{
    public class ChiTietGioHangDto : PaginParams
    {
        public Guid? id { get; set; }
        public Guid? gio_hang_id { get; set; }
        public Guid? san_pham_id { get; set; }
        public int? so_luong { get; set; }
        public san_pham? san_pham { get; set; }
    }
}
