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

        public Task<List<DanhGiaDto>> GetBySanPham(string ma)
        {
            try
            {
                var dsDanhGia = _context.danh_gia.Where(x=> x.ma_san_pham == ma).ToList();
                var result = dsDanhGia.Select(x => new DanhGiaDto
                {
                    id = x.id,
                    san_pham_id = x.san_pham_id,
                    account_id = x.account_id,
                    danh_gia_chat_luong = x.danh_gia_chat_luong,
                    noi_dung_danh_gia = x.noi_dung_danh_gia,
                    noi_dung_phan_hoi = x.noi_dung_phan_hoi,
                    san_pham = new Dto.QuanLySanPham.SanPhamDto
                    {
                        id = _context.san_pham.FirstOrDefault(a => a.ma_san_pham == ma)!.id,
                        ten_san_pham = _context.san_pham.FirstOrDefault(a => a.ma_san_pham == ma)!.ten_san_pham,
                        mo_ta = _context.san_pham.FirstOrDefault(a => a.ma_san_pham == ma)!.mo_ta,
                        duong_dan_anh_bia = _context.san_pham.FirstOrDefault(a => a.ma_san_pham == ma)!.duong_dan_anh_bia,
                    },
                    nguoi_dung = new accountDto
                    {
                        ten = _context.account.FirstOrDefault(a => a.id == x.account_id)!.ten,
                    }
                }).ToList();
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách đánh giá: " + ex.Message, ex);
            }
        }
    }
}
