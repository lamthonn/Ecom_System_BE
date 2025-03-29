using Ecom.Entity.common;
using Ecom.Interfaces;

namespace Ecom.Entity
{
    public class ngan_hang : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public string ten_tai_khoan { get; set; }
        public string so_tai_khoan { get; set; }
        public string ten_ngan_hang { get; set; }
        public string so_the {  get; set; }
        public bool is_default { get; set; } = false;
        public Guid account_id { get; set; }
        public account? account { get; set; }
    }
}
