using Ecom.Entity.common;

namespace Ecom.Entity
{
    public class thong_bao : BaseModel
    {
        public Guid Id { get; set; }
        public Guid nguoi_dung_id { get; set; }
        public string? noi_dung { get; set; }
        public int nhom { get; set; }
        public bool trang_thai { get; set; } = false;
        public DateTime? thoi_gian_doc { get; set; }
        public virtual account? account { get; set; }
    }
}
