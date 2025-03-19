using Ecom.Entity.common;
using Ecom.Interfaces;

namespace Ecom.Entity
{
    public class danh_muc : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public string ma_danh_muc { get; set; }
        public string ten_danh_muc { get; set; }
        public string? mo_ta { get; set; }
    }
}
