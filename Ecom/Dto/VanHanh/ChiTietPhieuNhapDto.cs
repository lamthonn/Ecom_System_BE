using backend_v3.Dto.Common;
using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;

namespace Ecom.Dto.VanHanh
{
    public class ChiTietPhieuNhapDto : PaginParams
    {
        public Guid? id { get; set; }
        public Guid? phieu_nhap_kho_id { get; set; }
        public Guid? san_pham_id { get; set; }
        public string? ma_san_pham { get; set; }
        public int? so_luong { get; set; }
        public decimal? don_gia { get; set; } // => Giá/1 biến thể sản phẩm
        public string? sku { get; set; }
        public virtual SanPhamDto? san_pham_dto { get; set; }
        public virtual phieu_nhap_kho? phieu_nhap_kho { get; set; }
        public virtual san_pham? san_pham { get; set; }
    }
}
