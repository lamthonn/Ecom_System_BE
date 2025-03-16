using Ecom.Entity.common;

namespace Ecom.Entity
{
    public class danh_muc : BaseModel
    {
        public Guid id { get; set; }
        public string ma_danh_muc { get; set; }
        public string ten_danh_muc { get; set; }
        public string? mo_ta { get; set; }
    }
}
