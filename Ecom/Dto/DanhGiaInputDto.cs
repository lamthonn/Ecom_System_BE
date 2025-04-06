namespace Ecom.Dto
{
    public class DanhGiaInputDto
    {
        public Guid id { get; set; }
        public string? ma_san_pham {  get; set; }
        public int rating { get; set; }
        public string reviewText { get; set; }
        public Guid? don_hang_id { get; set; }
        public string ma_don_hang { get; set; }
    }
}
