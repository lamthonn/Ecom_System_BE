using Ecom.Entity.common;
using Ecom.Interfaces;

namespace Ecom.Entity
{
    public class anh_san_pham : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public string duong_dan { get; set; }
        public string? ma_san_pham { get; set; }
        public Guid? san_pham_id { get; set; }
        public virtual san_pham? san_Pham { get; set; }
    }
}
