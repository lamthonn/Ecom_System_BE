using backend_v3.Models;
using Ecom.Context;
using Ecom.Dto.QuanLySanPham;
using Ecom.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly AppDbContext _context;
        public SanPhamService(AppDbContext context)
        {
            _context = context;
        }

        public Task<SanPhamDto> create(SanPhamDto request)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteAny(List<string> ids)
        {
            throw new NotImplementedException();
        }

        public void Edit(string id, SanPhamDto request)
        {
            throw new NotImplementedException();
        }

        public byte[] ExportToExcel()
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedList<SanPhamDto>> GetAll(SanPhamDto request)
        {
            try
            {
                var dataQuery = _context.san_pham.AsNoTracking();

                //if (!string.IsNullOrEmpty(request.ma_danh_muc))
                //{
                //    dataQuery = dataQuery.Where(x => x.ma_danh_muc.Contains(request.ma_danh_muc));
                //}

                //if (!string.IsNullOrEmpty(request.ten_danh_muc))
                //{
                //    dataQuery = dataQuery.Where(x => x.ten_danh_muc.Contains(request.ten_danh_muc));
                //}

                //if (request.Created != null)
                //{
                //    dataQuery = dataQuery.Where(x => x.Created == request.Created);
                //}

                var dataQueryDto = dataQuery.Select(x => new SanPhamDto
                {
                    Id = x.id,
                    ma_san_pham = x.ma_san_pham,
                    ten_san_pham = x.ten_san_pham,
                    mo_ta = x.mo_ta,
                    danh_muc_id = x.danh_muc_id,
                    is_active = x.is_active,
                    xuat_xu = x.xuat_xu,
                    gia = x.gia,
                    khuyen_mai = x.khuyen_mai,
                    so_luong = dataQuery.Where(y => y.ma_san_pham == x.ma_san_pham).Sum(y => y.so_luong)
                }).AsEnumerable().DistinctBy(x => x.ma_san_pham).AsQueryable();

                var result = await PaginatedList<SanPhamDto>.Create(dataQueryDto, request.pageNumber, request.pageSize);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<SanPhamDto> GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
