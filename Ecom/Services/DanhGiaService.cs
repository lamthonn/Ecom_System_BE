using backend_v3.Dto.Common;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Dto;
using Ecom.Dto.QuanLySanPham;
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

        public async Task<PaginatedList<DanhGiaDto>> GetAllPaging(DanhGiaParams request)
        {
            try
            {
                var dataQuery = _context.danh_gia.AsNoTracking();

                if (!string.IsNullOrEmpty(request.ma_san_pham))
                {
                    dataQuery = dataQuery.Where(x => x.ma_san_pham.Contains(request.ma_san_pham));
                }

                if (request.muc_danh_gia.HasValue)
                {
                    dataQuery = dataQuery.Where(x => x.danh_gia_chat_luong == request.muc_danh_gia.Value);
                }

                var dataQueryDto = dataQuery
                    .OrderByDescending(x => x.Created)
                    .Select(x => new DanhGiaDto
                    {
                        id = x.id,
                        account_id = x.account_id,
                        ten_khach_hang = _context.account.FirstOrDefault(ac => ac.id == x.account_id).ten,
                        danh_gia_chat_luong = x.danh_gia_chat_luong,
                        ma_san_pham = x.ma_san_pham,
                        noi_dung_danh_gia = x.noi_dung_danh_gia,
                        ngay_danh_gia = x.Created.ToString("dd/MM/yyyy HH") + "h",
                        noi_dung_phan_hoi = x.noi_dung_phan_hoi,
                        san_pham_id = x.san_pham_id,
                        ten_san_pham = _context.san_pham.FirstOrDefault(sp => sp.ma_san_pham == x.ma_san_pham).ten_san_pham,
                    });

                var result = await PaginatedList<DanhGiaDto>.Create(dataQueryDto, request.pageNumber, request.pageSize);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public class DanhGiaParams : PaginParams
        {
            public string? ma_san_pham { get; set; }
            public int? muc_danh_gia { get; set; }
        }
    }
}
