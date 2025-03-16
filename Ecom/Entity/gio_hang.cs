using Ecom.Entity.common;

namespace Ecom.Entity
{
    public class gio_hang :BaseModel
    {
        public Guid id { get; set; }
        public Guid account_id { get; set; }
        public virtual account? Account { get; set; }
    }
}
