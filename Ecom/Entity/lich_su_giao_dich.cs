using Ecom.Entity.common;

namespace Ecom.Entity
{
    public class lich_su_giao_dich: BaseModel
    {
        public Guid Id { get; set; }
        public DateTime? ngay_giao_dich { get; set; }
        public int? phuong_thuc_giao_dich{ get; set; } //0-stripe; 1-zaloPay
        public decimal? giao_dich { get; set; }
        public int? loai_giao_dich { get; set; } //1-Doanh thu đơn hàng; 2-rút tiền; 3-đơn hoàn
        public decimal so_du {  get; set; }
    }
}
