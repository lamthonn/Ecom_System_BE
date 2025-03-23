using backend_v3.Dto.Common;

namespace Ecom.Dto.QuanLySanPham
{
    public class DanhMucDto : PaginParams
    {
        public Guid id { get; set; }
        public string? ma_danh_muc { get; set; }
        public string? ten_danh_muc { get; set; }
        public string? mo_ta { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}
