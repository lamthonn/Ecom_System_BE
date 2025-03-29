using Ecom.Entity.common;
using Ecom.Interfaces;

namespace Ecom.Entity
{
    public class chi_tiet_don_hang : BaseModel, IEntity
    { 
        public Guid id { get; set; }
        public Guid don_hang_id { get; set; }
        public Guid san_pham_id { get; set; }
        public decimal thanh_tien { get; set; }
        public decimal? don_gia {  get; set; }
        public int? so_luong { get; set; }
        public virtual don_hang? Don_Hang { get; set; }
        public virtual san_pham? San_pham { get; set; }
    }
}
