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
        public account? tai_khoan { get; set; }
        public dvvc? dvvc { get; set; }
    }
}
