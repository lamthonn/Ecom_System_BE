using Ecom.Context;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services
{
    public class DanhGiaService : IDanhGiaService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        public DanhGiaService(IHttpContextAccessor httpContextAccessor, AppDbContext context) 
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<bool> DanhGia(List<DanhGiaInputDto> listDanhGia, Guid donHangId)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new Exception("Không thể truy xuất HttpContext");

                var userIdClaim = httpContext.User.FindFirst("id");
                if (userIdClaim == null)
                    throw new Exception("Không tìm thấy ID người dùng trong token");

                var userId = Guid.Parse(userIdClaim.Value);
                var user = await _context.account.FirstOrDefaultAsync(x => x.id == userId);

                if (user == null)
                    throw new Exception("Không tìm thấy tài khoản");
                foreach (var item in listDanhGia)
                {
                    var newItem = new danh_gia
                    {
                        id = Guid.NewGuid(),
                        san_pham_id = item.id,
                        account_id = userId,
                        danh_gia_chat_luong = item.rating,
                        noi_dung_danh_gia = item.reviewText,
                    };
                    _context.danh_gia.Add(newItem);
                }
                var donHang = await _context.don_hang.FirstOrDefaultAsync(x => x.id == donHangId);
                if (donHang != null)
                {
                    donHang.is_danh_gia = true;
                    _context.don_hang.Update(donHang);
                }
                await _context.SaveChangesAsync(new CancellationToken());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi đánh giá: " + ex.Message, ex);
            }
        }
    }
}
