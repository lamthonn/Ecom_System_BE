using Ecom.Entity.common;
using Ecom.Interfaces;
using System.ComponentModel;

namespace Ecom.Entity
{
    public class account : BaseModel, IEntity
    {
        public Guid id { get; set; }
        public string tai_khoan { get; set; }
        public string mat_khau { get; set; }
        public string? salt { get; set; }
        public string? ten { get; set; }
        public DateTime? ngay_sinh { get; set; } = new DateTime(1, 1, 0001);
        public string? dia_chi { get; set; }
        public bool? gioi_tinh { get; set; }
        public string? email { get; set; }
        [DefaultValue(true)]
        /// <summary>
        /// 0: Đang khóa
        /// 1: Đang hoạt động
        /// </summary>
        public bool? trang_thai { get; set; }
        public string? so_dien_thoai { get; set; }
        public string? RefreshToken { get; set; } // Lưu Refresh Token
        public DateTime? RefreshTokenExpiryTime { get; set; } // Hạn sử dụng của Refresh Token
        [DefaultValue(false)]
        public bool? is_super_admin { get; set; }
        public Guid? dvvc_id { get; set; }
        public virtual dvvc? dvvc { get; set; }

    }
}
