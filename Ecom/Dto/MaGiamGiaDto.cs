namespace Ecom.Dto
{
    public class MaGiamGiaDto
    {
        public Guid id { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public decimal giam_gia { get; set; }
        public int so_luong { get; set; }
        public DateTime? bat_dau { get; set; }
        public DateTime? ket_thuc { get; set; }
        public List<DateTime>? thoi_gian { get; set; }
        public bool is_active { get; set; } = true;
    }
}
