using Ecom.Entity.common;
using Ecom.Interfaces;

namespace Ecom.Entity
{
    public class ma_giam_gia : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public decimal giam_gia { get; set; }
        public int so_luong { get; set; }
        public DateTime? bat_dau { get; set; }
        public DateTime? ket_thuc { get; set; }
        public bool is_active { get; set; } = true;
    }
}
