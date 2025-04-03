using AutoMapper;
using backend_v3.Dto.Common;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Dto.DonHang;
using Ecom.Dto.QuanLySanPham;
using Ecom.Dto.VanHanh;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Ecom.Services
{
    public class DonHangService : IDonHangService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public DonHangService(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginatedList<DonHangDto>> GetAll(DonHangDto request)
        {
            try
            {
                // Thay vì ToList(), hãy để IQueryable
                var query = _context.don_hang.AsNoTracking();
                if(request.trang_thai != null)
                {
                    query = query.Where(x => x.trang_thai == request.trang_thai);
                }


                var dataDto = from x in query
                              let ChiTiet = _context.chi_tiet_don_hang.FirstOrDefault(y => y.don_hang_id == x.id)
                              let AnhDaiDien = _context.san_pham.FirstOrDefault(z => z.id == ChiTiet.san_pham_id)!.duong_dan_anh_bia
                              select new DonHangDto
                              {
                                  id = x.id,
                                  account_id = x.account_id,
                                  tong_tien = x.tong_tien,
                                  ma_don_hang = x.ma_don_hang,
                                  trang_thai = x.trang_thai,
                                  dvvc_id = x.dvvc_id,
                                  ngay_mua = x.ngay_mua,
                                  thanh_tien = x.thanh_tien,
                                  Created = x.Created,
                                  LastModified = x.LastModified,
                                  CreatedBy = x.CreatedBy,
                                  LastModifiedBy = x.LastModifiedBy,
                                  anh_dai_dien = AnhDaiDien,
                                  tai_khoan = _context.account.FirstOrDefault(y => y.id == x.account_id),
                                  ds_chi_tiet_don_hang = _context.chi_tiet_don_hang.Where(z => z.don_hang_id == x.id).Select(a => new ChiTietDonHangDto
                                  {
                                      don_hang_id = a.don_hang_id,
                                      don_gia = a.don_gia,
                                      san_pham_id = a.san_pham_id,
                                      so_luong = a.so_luong,
                                      id = a.id,
                                      thanh_tien = a.thanh_tien,
                                      ten_san_pham = _context.san_pham.FirstOrDefault(b => b.id == a.san_pham_id)!.ten_san_pham,
                                      mau_sac = _context.san_pham.FirstOrDefault(b => b.id == a.san_pham_id)!.mau_sac,
                                      kich_thuoc = _context.san_pham.FirstOrDefault(b => b.id == a.san_pham_id)!.size
                                  }).ToList()
                              };
                // Sắp xếp theo trạng thái tăng dần, sau đó ngày mua giảm dần
                dataDto = dataDto.OrderBy(x => x.trang_thai).ThenByDescending(x => x.ngay_mua);

                // Truyền IQueryable vào PaginatedList
                var result = await PaginatedList<DonHangDto>.Create(dataDto, request.pageNumber, request.pageSize);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task XuLyDonHang(string id, DonHangDto request)
        {
            var donHang = await _context.don_hang.FirstOrDefaultAsync(x => x.id.ToString() == id);
            if(request.trang_thai == null)
            {
                throw new Exception("Không có trạng thái thay đổi");
            }

            if(donHang != null && request.trang_thai != null)
            {
                var ChiTiet = await _context.chi_tiet_don_hang.Where(x => x.don_hang_id == donHang.id).ToListAsync();
                donHang.trang_thai = request.trang_thai ?? donHang.trang_thai;
                _context.don_hang.Update(donHang);
                await _context.SaveChangesAsync();
                //update lại số lượng sản phẩm còn lại + lượt bán sản phẩm
                if (request.trang_thai == 4)
                {
                    foreach (var chiTiet in ChiTiet)
                    {
                        var sanPham = await _context.san_pham.FirstOrDefaultAsync(x=>x.id == chiTiet.san_pham_id);
                        if (sanPham != null)
                        {
                            sanPham.so_luong = sanPham.so_luong - chiTiet.so_luong;
                            sanPham.luot_ban = sanPham.luot_ban == null ? (0 + chiTiet.so_luong) : (sanPham.luot_ban + chiTiet.so_luong);
                        }
                        _context.san_pham.Update(sanPham);
                    }
                }
                await _context.SaveChangesAsync();
            }

        }

        public async Task XuLyDonHangs(List<DonHangDto> request)
        {
            var DonHangIds = request.Select(x => x.id);
            var dataUpdate = await _context.don_hang.Where(x => DonHangIds.Contains(x.id)).ToListAsync();
            if(dataUpdate.Count() > 0)
            {
                //update trạng thái của đơn hàng
                foreach (var item in dataUpdate)
                {
                    var ChiTiet = await _context.chi_tiet_don_hang.Where(x => x.don_hang_id == item.id).ToListAsync();
                    item.trang_thai = request[0].trang_thai ?? 0;
                    if (request[0].trang_thai == 4)
                    {
                        foreach (var chiTiet in ChiTiet)
                        {
                            var sanPham = await _context.san_pham.FirstOrDefaultAsync(x=> x.id == chiTiet.san_pham_id);
                            if (sanPham != null)
                            {
                                sanPham.so_luong = sanPham.so_luong - chiTiet.so_luong;
                                sanPham.luot_ban = sanPham.luot_ban == null ? (0 + chiTiet.so_luong) : (sanPham.luot_ban + chiTiet.so_luong) ;
                            }
                            _context.san_pham.Update(sanPham);
                        }
                    }
                }
                _context.UpdateRange(dataUpdate);
                //update lại số lượng sản phẩm còn lại + lượt bán sản phẩm

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Không có đơn hàng nào được chọn");
            }

        }
        public async Task<PaginatedList<DonHangUserDto>> GetDonHangs(PaginParams param, int trang_thai)
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

                // Apply search filter if keySearch is provided
                var query = _context.don_hang
                    .Where(dh => dh.account_id == userId);

                if (!string.IsNullOrEmpty(param.keySearch))
                {
                    query = query.Where(dh => dh.ma_don_hang.Contains(param.keySearch) || dh.so_dien_thoai.Contains(param.keySearch) || dh.dia_chi.Contains(param.keySearch));
                }
                if(trang_thai != 0)
                {
                    query = query.Where(x => x.trang_thai == trang_thai);
                }

                // Get total item count
                var totalItems = await query.CountAsync();

                // Apply pagination before selecting data
                var paginatedQuery = query
                    .OrderBy(dh => dh.ngay_mua) // Sorting criteria
                    .Skip((param.pageNumber - 1) * param.pageSize)
                    .Take(param.pageSize);

                // Select and map the results into DonHangUserDto
                var donHangs = paginatedQuery
                    .Select(dh => new DonHangUserDto
                    {
                        id = dh.id,
                        ma_don_hang = dh.ma_don_hang,
                        trang_thai = dh.trang_thai,
                        ngay_mua = dh.ngay_mua,
                        tong_tien = dh.tong_tien,
                        thanh_tien = dh.thanh_tien,
                        so_dien_thoai = dh.so_dien_thoai,
                        dia_chi = dh.dia_chi,
                        is_danh_gia = dh.is_danh_gia,
                        ChiTietDonHangs = _context.chi_tiet_don_hang
                                .Where(ctdh => ctdh.don_hang_id == dh.id)
                                .Select(ct => new ChiTietDonHangUserDto
                                {
                                    id = ct.id,
                                    san_pham_id = ct.san_pham_id,
                                    ten_san_pham = _context.san_pham.FirstOrDefault(x => x.id == ct.san_pham_id).ten_san_pham,
                                    thanh_tien = ct.thanh_tien,
                                    don_gia = ct.don_gia,
                                    so_luong = ct.so_luong
                                })
                                .ToList()
                    });

                // Create the PaginatedList object with the result
                return await PaginatedList<DonHangUserDto>.Create(donHangs, param.pageNumber, param.pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi ghi get đơn hàng: " + ex.Message, ex);
            }
        }

        public async Task<DonHangUserDto> GetDonHangById(Guid id)
        {
            try
            {
                var dh = await _context.don_hang
                    .FirstOrDefaultAsync(dh => dh.id == id);
                if (dh == null) throw new Exception("Đơn hàng không tồn tại");
                var result = new DonHangUserDto
                {
                    id = dh.id,
                    ma_don_hang = dh.ma_don_hang,
                    trang_thai = dh.trang_thai,
                    ngay_mua = dh.ngay_mua,
                    tong_tien = dh.tong_tien,
                    thanh_tien = dh.thanh_tien,
                    so_dien_thoai = dh.so_dien_thoai,
                    dia_chi = dh.dia_chi,
                    dvvc_id = dh.dvvc_id,
                    is_danh_gia = dh.is_danh_gia,
                    ten_don_vi_cc = _context.dvvc.FirstOrDefault(dvvc => dvvc.id == dh.dvvc_id).Name,
                    ChiTietDonHangs = _context.chi_tiet_don_hang
                        .Where(ctdh => ctdh.don_hang_id == dh.id)
                        .Select(ct => new ChiTietDonHangUserDto
                        {
                            id = ct.id,
                            san_pham_id = ct.san_pham_id,
                            ten_san_pham = _context.san_pham.FirstOrDefault(x => x.id == ct.san_pham_id).ten_san_pham,
                            thanh_tien = ct.thanh_tien,
                            don_gia = ct.don_gia,
                            so_luong = ct.so_luong
                        })
                        .ToList()
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi ghi get đơn hàng" + ex.Message, ex);
            }
        }

        public async Task<bool> ChuyenTrangThaiDonHang(Guid id, int TrangThai)
        {
            try
            {
                var dh = await _context.don_hang
                    .FirstOrDefaultAsync(dh => dh.id == id);
                if (dh == null) throw new Exception("Đơn hàng không tồn tại");
                dh.trang_thai = TrangThai;
                _context.don_hang.Update(dh);
                await _context.SaveChangesAsync(new CancellationToken());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi hủy đơn hàng" + ex.Message, ex);
            }
        }
            
    }
}
