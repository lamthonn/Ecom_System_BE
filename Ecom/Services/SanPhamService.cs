using AutoMapper;
using Azure.Core;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;
using Ecom.Interfaces;
using Ecom.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly SaveFileCommon _fileService;
        private readonly IMapper _mapper;

        public SanPhamService(AppDbContext context, IWebHostEnvironment env, SaveFileCommon fileService, IMapper mapper)
        {
            _context = context;
            _env = env;
            _fileService = fileService;
            _mapper = mapper;
        }

        public void AddListImage(List<string> filePath, string ma)
        {
            var listData = new List<anh_san_pham>();
            foreach (var item in filePath)
            {
                var data = new anh_san_pham
                {
                    id = Guid.NewGuid(),
                    
                };
            }
        }

        public async Task<List<SanPhamDto>> create(List<SanPhamDto> request)
        {
            var newData = _mapper.Map<List<SanPhamDto>, List<san_pham>>(request);
            _context.san_pham.AddRange(newData);
            _context.SaveChanges();
            return request;
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
                IQueryable<san_pham> dataQuery = _context.san_pham.AsNoTracking();

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

                var dataQueryDto = dataQuery
                .GroupBy(x => x.ma_san_pham)
                .Select(g => new SanPhamDto
                {
                    Id = g.First().id,
                    ma_san_pham = g.Key,
                    ten_san_pham = g.First().ten_san_pham,
                    mo_ta = g.First().mo_ta,
                    danh_muc_id = g.First().danh_muc_id,
                    is_active = g.First().is_active,
                    xuat_xu = g.First().xuat_xu,
                    gia = g.First().gia,
                    khuyen_mai = g.First().khuyen_mai,
                    so_luong = g.Sum(y => y.so_luong)
                });

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

        public async Task<string> SaveImageFileCoverPhoto(IFormFile file)
        {
            string filePath = await _fileService.SaveImageFileCommon(file, "san_pham");
            return filePath;
        }

        public async Task<List<string>> SaveMutiImageFile(List<IFormFile> files)
        {
            var filePath = await _fileService.SaveMultipleImageFilesCommon(files, "san_pham");
            return filePath;
        }
    }
}
