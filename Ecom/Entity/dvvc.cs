using Ecom.Entity.common;
using Ecom.Interfaces;
namespace Ecom.Entity
{
    public class dvvc : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public string Name { get; set; }
    }
}
