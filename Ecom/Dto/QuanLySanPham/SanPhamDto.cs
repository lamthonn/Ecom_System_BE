using backend_v3.Dto.Common;

namespace Ecom.Dto.QuanLySanPham
{
    public class SanPhamDto : PaginParams
    {
        public Guid? Id { get; set; }
        public Guid? danh_muc_id { get; set; }
        public string? ma_san_pham { get; set; }
        public string? ten_san_pham { get; set; }
        public string? mo_ta { get; set; }
        public string? xuat_xu { get; set; }
        public int? luot_ban { get; set; }
        public int? so_luong { get; set; }
        public string? sku { get; set; }
        public string? duong_dan_anh_bia { get; set; }
        public string? mau_sac { get; set; }
        public string? size { get; set; }
        public decimal? gia { get; set; }
        public decimal? khuyen_mai { get; set; }
        public bool? is_active { get; set; } = true;
    }
}
