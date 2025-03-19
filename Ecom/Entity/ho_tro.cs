using Ecom.Entity.common;
using Ecom.Interfaces;
namespace Ecom.Entity
{
    public class ho_tro : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public Guid account_id { get; set; }
        public string noi_dung { get; set; }
        public bool trang_thai { get; set; }
        public virtual account? account { get; set; }
    }
}
