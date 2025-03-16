using Ecom.Entity.common;

namespace Ecom.Entity
{
    public class chi_tiet_don_hang : BaseModel
    { 
        public Guid Id { get; set; }
        public Guid don_hang_id { get; set; }
        public Guid san_pham_id { get; set; }
        public decimal thanh_tien { get; set; }
        public int? so_luong { get; set; }
        public virtual don_hang? Don_Hang { get; set; }
        public virtual san_pham? San_pham { get; set; }
    }
}
