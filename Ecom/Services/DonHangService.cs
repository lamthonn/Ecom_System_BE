using AutoMapper;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Dto.QuanLySanPham;
using Ecom.Dto.VanHanh;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services
{
    public class DonHangService : IDonHangService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public DonHangService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
    }
}
