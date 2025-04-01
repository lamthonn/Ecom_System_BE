using backend_v3.Dto.Common;
using Ecom.Entity;

namespace Ecom.Dto.VanHanh
{
    public class DonHangDto : PaginParams
    {
        public Guid? id { get; set; }
        public Guid? account_id { get; set; }
        public Guid? dvvc_id { get; set; }
        public string? ma_don_hang { get; set; }
        public int? trang_thai { get; set; }
        public DateTime? ngay_mua { get; set; }
        public decimal? tong_tien { get; set; }
        public decimal? thanh_tien { get; set; }
        public string? anh_dai_dien { get; set; }
        public List<ChiTietDonHangDto>? ds_chi_tiet_don_hang { get; set; }
        public account? tai_khoan { get; set; }
        public dvvc? dvvc { get; set; }
    }

    public class ChiTietDonHangDto {
        public Guid? id { get; set; }
        public Guid? don_hang_id { get; set; }
        public Guid? san_pham_id { get; set; }
        public string? ten_san_pham { get; set; }
        public string? kich_thuoc { get; set; }
        public string? mau_sac { get; set; }
        public decimal? thanh_tien { get; set; }
        public decimal? don_gia { get; set; }
        public int? so_luong { get; set; }
    }
}
