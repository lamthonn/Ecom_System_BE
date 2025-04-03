using Ecom.Context;
using Ecom.Dto.GioHang;
using Ecom.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services
{
    public class GioHangService : IGioHangService
    {
        private readonly AppDbContext _context;
        public GioHangService(AppDbContext context)
        {
            _context = context;
        }


        public Task<GioHangDto> GetAll(GioHangDto request)
        {
            try
            {
                var data = _context.gio_hang.FirstOrDefault(x=> x.account_id == request.account_id);
                var result = new GioHangDto
                {
                    id = data!.id,
                    account_id = data!.account_id,
                    ds_chi_tiet_gio_hang = _context.chi_tiet_gio_hang.Where(x => x.gio_hang_id == data.id).Select(a => new ChiTietGioHangDto
                    {
                        id = a.id,
                        gio_hang_id = a.gio_hang_id,
                        san_pham_id = a.san_pham_id,
                        so_luong = a.so_luong,
                        Created = a.Created,
                        san_pham = _context.san_pham.FirstOrDefault(b => b.id == a.san_pham_id),
                    }).ToList(),
                };

                return Task.FromResult(result);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ChiTietGioHangDto> Add(ChiTietGioHangDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<ChiTietGioHangDto> Edit(ChiTietGioHangDto request)
        {
            throw new NotImplementedException();
        }

    }
}
    