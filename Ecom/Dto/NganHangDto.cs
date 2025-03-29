using Ecom.Entity;

namespace Ecom.Dto
{
    public class NganHangDto
    {
        public Guid id { get; set; }
        public string ten_tai_khoan { get; set; }
        public string so_tai_khoan { get; set; }
        public string ten_ngan_hang { get; set; }
        public string so_the { get; set; }
        public Guid account_id { get; set; }
        public bool is_default { get; set; }

    }
}
