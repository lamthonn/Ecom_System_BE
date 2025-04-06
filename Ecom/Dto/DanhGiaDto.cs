using Ecom.Dto.QuanLySanPham;

using Ecom.Entity;

namespace Ecom.Dto
{
    public class DanhGiaDto
    {
        public Guid id { get; set; }
        public Guid? san_pham_id { get; set; }
        public string? ten_san_pham { get; set; }
        public Guid account_id { get; set; }
        public string? ten_khach_hang { get; set; }
        public string? ma_san_pham { get; set; }
        public int danh_gia_chat_luong { get; set; }
        public string? noi_dung_danh_gia { get; set; }
        public string? noi_dung_phan_hoi { get; set; }
        public SanPhamDto? san_pham { get; set; }
        public accountDto? nguoi_dung { get; set; }
        public string? ngay_danh_gia { get; set; }
        public string? ma_don_hang { get; set; }
       
    }
}
