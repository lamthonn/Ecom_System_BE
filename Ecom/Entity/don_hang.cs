using Ecom.Entity.common;
using Ecom.Interfaces;
namespace Ecom.Entity
{
    public class don_hang : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public Guid account_id { get; set; }
        public Guid dvvc_id { get; set; }
        public string ma_don_hang { get; set; }
        public int trang_thai { get; set; } = 1;
        public DateTime ngay_mua { get; set; }
        public decimal tong_tien { get; set; }
        public decimal thanh_tien { get; set; }
        public string? so_dien_thoai { get; set; }
        public string? dia_chi { get; set; }
        public virtual account? Account { get; set; }
        public virtual dvvc? Dvvc { get; set; }
        
    }
}
