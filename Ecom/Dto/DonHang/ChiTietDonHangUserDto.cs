namespace Ecom.Dto.DonHang
{
    public class ChiTietDonHangUserDto
    {
        public Guid id { get; set; }
        public Guid don_hang_id { get; set; }
        public Guid san_pham_id { get; set; }
        public string ten_san_pham {  get; set; }
        public decimal thanh_tien { get; set; }
        public decimal? don_gia { get; set; }
        public int? so_luong { get; set; }
    }
}
