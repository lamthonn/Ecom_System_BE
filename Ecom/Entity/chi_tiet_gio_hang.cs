using Ecom.Entity.common;

namespace Ecom.Entity
{
    public class chi_tiet_gio_hang : BaseModel
    {
        public Guid Id { get; set; }
        public Guid gio_hang_id { get; set; }
        public Guid san_pham_id { get; set; }
        public int so_luong { get; set; }
        public virtual gio_hang? Gio_Hang { get; set; }
        public virtual san_pham? San_Pham { get; set; }
        
    }
}
