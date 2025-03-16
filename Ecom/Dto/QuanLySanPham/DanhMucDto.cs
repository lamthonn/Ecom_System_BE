using backend_v3.Dto.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecom.Dto.QuanLySanPham
{
    public class DanhMucDto : PaginParams
    {
        public Guid id { get; set; }
        public string? ma_danh_muc { get; set; }
        public string? ten_danh_muc { get; set; }
        public string? mo_ta { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
