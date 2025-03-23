using System.ComponentModel;

namespace Ecom.Dto
{
    public class accountDto
    {
        public Guid Id { get; set; }
        public string tai_khoan { get; set; }
        public string mat_khau { get; set; }
        public string? ten { get; set; }
        public DateTime? ngay_sinh { get; set; } = new DateTime(1, 1, 0001);
        public string? dia_chi { get; set; }
        public bool? gioi_tinh { get; set; }
        public string? email { get; set; }
        public bool? trang_thai { get; set; }
        public string? so_dien_thoai { get; set; }
        public string? RefreshToken { get; set; } // Lưu Refresh Token
        public DateTime? RefreshTokenExpiryTime { get; set; } // Hạn sử dụng của Refresh Token
        public bool is_super_admin { get; set; }
    }

    public class UpdatePhoneDto
    {
        public string so_dien_thoai { get; set; } = string.Empty;
    }

    // DTO cho cập nhật email
    public class UpdateEmailDto
    {
        public string email { get; set; } = string.Empty;
    }

    // DTO cho cập nhật mật khẩu
    public class UpdatePasswordDto
    {
        public string oldPassword { get; set; } = string.Empty;
        public string newPassword { get; set; } = string.Empty;
    }
}
