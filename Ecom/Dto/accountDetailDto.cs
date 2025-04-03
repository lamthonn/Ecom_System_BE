namespace Ecom.Dto
{
    public class accountDetailDto
    {
        public Guid id { get; set; }
        public string? ten { get; set; }
        public string? ngay_sinh { get; set; }
        public string? dia_chi { get; set; }
        public bool? gioi_tinh { get; set; }
        public string? email { get; set; }
        public bool? trang_thai { get; set; }
        public string? so_dien_thoai { get; set; }
        public List<NganHangDto>? listNganHangs { get; set; }

    }
}
