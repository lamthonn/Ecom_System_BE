using AutoMapper;
using Ecom.Context;
using Ecom.Dto.GioHang;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services
{
    public class GioHangService : IGioHangService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public GioHangService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            try
            {
                var gioHang = _context.gio_hang.FirstOrDefault(x => x.account_id == request.nguoi_dung_id);
                if(gioHang == null)
                {
                    var newGioHang = new gio_hang
                    {
                        id = Guid.NewGuid(),
                        account_id = request.nguoi_dung_id ?? Guid.NewGuid(),
                    };
                    _context.gio_hang.Add(newGioHang);

                    var chiTiet = new chi_tiet_gio_hang
                    {
                        id = Guid.NewGuid(),
                        gio_hang_id = newGioHang.id,
                        so_luong = request.so_luong ?? 1,
                        Created = DateTime.Now,
                        san_pham_id = request.san_pham_id ?? Guid.NewGuid(),
                        
                    };
                    _context.chi_tiet_gio_hang.Add(chiTiet);
                    await _context.SaveChangesAsync();
                    return new ChiTietGioHangDto
                    {
                        id = chiTiet.id,
                        gio_hang_id = chiTiet.gio_hang_id,
                        san_pham_id = chiTiet.san_pham_id,
                    };
                }
                else
                {
                    var duplicateSanPham = _context.chi_tiet_gio_hang.FirstOrDefault(x => x.san_pham_id == request.san_pham_id);
                    if(duplicateSanPham == null)
                    {
                        var chiTiet = new chi_tiet_gio_hang
                        {
                            id = Guid.NewGuid(),
                            gio_hang_id = gioHang.id,
                            so_luong = request.so_luong ?? 1,
                            Created = DateTime.Now,
                            san_pham_id = request.san_pham_id ?? Guid.NewGuid(),

                        };
                        _context.chi_tiet_gio_hang.Add(chiTiet);
                        await _context.SaveChangesAsync();
                        return new ChiTietGioHangDto
                        {
                            id = chiTiet.id,
                            gio_hang_id = chiTiet.gio_hang_id,
                            san_pham_id = chiTiet.san_pham_id,
                        };
                    }
                    else
                    {
                        duplicateSanPham.so_luong += (request.so_luong ?? 0);
                        _context.chi_tiet_gio_hang.Update(duplicateSanPham);
                        await _context.SaveChangesAsync();
                        return new ChiTietGioHangDto
                        {
                            id = duplicateSanPham.id,
                            gio_hang_id = duplicateSanPham.gio_hang_id,
                            san_pham_id = duplicateSanPham.san_pham_id,
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task Edit(ChiTietGioHangDto request)
        {
            try
            {
                var dataUpdate = _context.chi_tiet_gio_hang.FirstOrDefault(x => x.id == request.id);
                if (dataUpdate != null) { 
                    dataUpdate.so_luong = request.so_luong ?? 1;
                    _context.chi_tiet_gio_hang.Update(dataUpdate);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Không tìm thấy sản phẩm này");
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task Delete(string id)
        {
            try
            {
                var dataUpdate = _context.chi_tiet_gio_hang.FirstOrDefault(x => x.id.ToString() == id);
                if (dataUpdate != null)
                {
                    _context.chi_tiet_gio_hang.Remove(dataUpdate);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Không tìm thấy sản phẩm này trong giỏ hàng");
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
    