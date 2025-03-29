using AutoMapper;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Dto.QuanLySanPham;
using Ecom.Dto.VanHanh;
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
                else {
                    query = query.Where(x => x.trang_thai != 3 && x.trang_thai != 2);
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
                                  tai_khoan = _context.account.FirstOrDefault(y => y.id == x.account_id)
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

        public Task XuLyDonHang(string id, DonHangDto request)
        {
            var donHang = _context.don_hang.FirstOrDefault(x => x.id.ToString() == id);
            if(request.trang_thai == null)
            {
                throw new Exception("Không có trạng thái thay đổi");
            }

            if(donHang != null && request.trang_thai != null)
            {
                donHang.trang_thai = request.trang_thai ?? donHang.trang_thai;
                _context.don_hang.Update(donHang);
                _context.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
