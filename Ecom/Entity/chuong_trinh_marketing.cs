using Ecom.Entity.common;
using Ecom.Interfaces;

namespace Ecom.Entity
{
    public class chuong_trinh_marketing : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public string ten { get; set; }
        /// <summary>
        /// 1: khuyến mãi
        /// 2: flash sale
        /// 3: mã giảm giá
        /// </summary>
        public int cong_cu { get; set; }
    }
}
