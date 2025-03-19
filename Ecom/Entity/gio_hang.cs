using Ecom.Entity.common;
using Ecom.Interfaces;
namespace Ecom.Entity
{
    public class gio_hang : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public Guid account_id { get; set; }
        public virtual account? Account { get; set; }
    }
}
