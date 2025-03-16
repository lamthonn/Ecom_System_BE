using Ecom.Entity.common;

namespace Ecom.Entity
{
    public class ho_tro : BaseModel
    {
        public Guid id { get; set; }
        public Guid account_id { get; set; }
        public string noi_dung { get; set; }
        public bool trang_thai { get; set; }
        public virtual account? account { get; set; }
    }
}
