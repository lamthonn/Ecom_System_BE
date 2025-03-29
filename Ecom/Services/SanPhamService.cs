using AutoMapper;
using Azure.Core;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;
using Ecom.Interfaces;
using Ecom.Services.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;

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

        public async void AddListImage(AddListImageProps request)
        {
            var listData = new List<anh_san_pham>();
            foreach (var item in request.filePath)
            {
                var data = new anh_san_pham
                {
                    id = Guid.NewGuid(),
                    duong_dan = item,
                    ma_san_pham = request.ma,
                    Created = DateTime.Now,
                };
                listData.Add(data);
            }

            _context.anh_san_pham.AddRange(listData);
            _context.SaveChanges();
        }

        public async Task<List<SanPhamDto>> create(List<SanPhamDto> request)
        {
            var newData = _mapper.Map<List<san_pham>>(request);
            foreach (var data in newData)
            {
                data.Created = DateTime.Now;
            }
            _context.san_pham.AddRange(newData);
            _context.SaveChanges();
            return request;
        }

        public void Delete(Guid id)
        {
            var data = _context.san_pham.FirstOrDefault(x => x.id == id);
            if (data != null)
            {
                var dataRemove = _context.san_pham.Where(x => x.ma_san_pham == data.ma_san_pham);
                _context.san_pham.RemoveRange(dataRemove);
                _context.SaveChanges();
            }
        }

        public void DeleteAny(List<Guid> ids)
        {
            var data = _context.san_pham.Where(x => ids.Contains(x.id)).GroupBy(sp=> sp.ma_san_pham).Select(y => y.Key);
            var dataRemove = _context.san_pham.Where(x => data.Contains(x.ma_san_pham));
            _context.san_pham.RemoveRange(dataRemove);
            _context.SaveChanges(); 
        }

        public async Task Edit(EditSanPhamRequest request)
        {
            // Parse chuỗi JSON thành danh sách SanPhamDto
            var dataSanPham = JsonConvert.DeserializeObject<List<SanPhamDto>>(request.data_san_pham);
            var ls_sku = dataSanPham.Select(x => x.sku);
            //check trùng ảnh bìa => nếu không thay đổi thì lấy đường dẫn cũ || nếu có thay đổi thì add ảnh vào thư mực và trả ra 1 đường dẫn
            var isDuplicateFile = _context.san_pham.Any(f => f.duong_dan_anh_bia == $"san_pham/{request.anh_bia!.FileName}"); // true - có trùng ; false - không trùng
            var folderPathAnhBia = "";
            if(isDuplicateFile == false)
            {
                folderPathAnhBia = await _fileService.SaveImageFileCommon(request.anh_bia!, "san_pham");
            }
            //lọc những bản ghi có ma_san_pham == request.ma
            var dataFilter = _context.san_pham.Where(x => x.ma_san_pham == request.ma && ls_sku.Contains(x.sku));
            var newdata = new List<san_pham>();
            foreach (var item in dataFilter)
            {
                var new_sp = dataSanPham.FirstOrDefault(x => x.sku == item.sku);
                if (new_sp != null)
                {
                    // Cập nhật trực tiếp đối tượng đã lấy từ database
                    item.ma_san_pham = new_sp.ma_san_pham;
                    item.ten_san_pham = new_sp.ten_san_pham;
                    item.mo_ta = new_sp.mo_ta;
                    item.gia = new_sp.gia;
                    item.khuyen_mai = new_sp.khuyen_mai;
                    item.mau_sac = new_sp.mau_sac;
                    item.size = new_sp.size;
                    item.is_active = new_sp.is_active;
                    item.so_luong = new_sp.so_luong;
                    item.xuat_xu = new_sp.xuat_xu;
                    item.duong_dan_anh_bia = isDuplicateFile ? item.duong_dan_anh_bia : folderPathAnhBia;
                }
            }
            _context.san_pham.UpdateRange(dataFilter);
            await _context.SaveChangesAsync();
            //xử lý update ảnh sản phẩm
        }

        public byte[] ExportToExcel()
        {
            var danhMucs = _context.san_pham.ToList();
            var dataQueryDto = danhMucs
                .GroupBy(x => x.ma_san_pham)
                .Select(g => new SanPhamDto
                {
                    id = g.First().id,
                    ma_san_pham = g.Key,
                    ten_san_pham = g.First().ten_san_pham,
                    mo_ta = g.First().mo_ta,
                    danh_muc_id = g.First().danh_muc_id,
                    is_active = g.First().is_active,
                    xuat_xu = g.First().xuat_xu,
                    gia = g.First().gia,
                    khuyen_mai = g.First().khuyen_mai,
                    so_luong = g.Sum(y => y.so_luong),
                    Created = g.First().Created,
                });
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DanhSachSanPham");

                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã sản phẩm";
                worksheet.Cells[1, 3].Value = "Tên sản phẩm";
                worksheet.Cells[1, 4].Value = "Mô tả";
                worksheet.Cells[1, 5].Value = "Giá sản phẩm";
                worksheet.Cells[1, 6].Value = "Giá khuyễn mãi";
                worksheet.Cells[1, 7].Value = "Số lượng";
                worksheet.Cells[1, 8].Value = "Trạng thái";
                worksheet.Cells[1, 9].Value = "Ngày tạo";

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[1, 1, 1, 9])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Ghi dữ liệu danh mục vào file Excel
                int row = 2;
                int stt = 1; // Initialize the sequence number
                foreach (var danhMuc in dataQueryDto)
                {
                    worksheet.Cells[row, 1].Value = stt; // Set the sequence number
                    worksheet.Cells[row, 2].Value = danhMuc.ma_san_pham;
                    worksheet.Cells[row, 3].Value = danhMuc.ten_san_pham;
                    worksheet.Cells[row, 4].Value = danhMuc.mo_ta;
                    worksheet.Cells[row, 5].Value = danhMuc.gia;
                    worksheet.Cells[row, 6].Value = danhMuc.khuyen_mai;
                    worksheet.Cells[row, 7].Value = danhMuc.so_luong;
                    worksheet.Cells[row, 8].Value = danhMuc.is_active == true ? "Hoạt động" : "Không hoạt động";
                    worksheet.Cells[row, 9].Value = danhMuc.Created!.ToString("dd/MM/yyyy HH:mm:ss");
                    row++;
                    stt++; // Increment the sequence number
                }

                // Auto-fit cột
                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        public async Task<PaginatedList<SanPhamDto>> GetAll(SanPhamDto request)
        {
            try
            {
                IQueryable<san_pham> dataQuery = _context.san_pham.AsNoTracking();

                if (!string.IsNullOrEmpty(request.ma_san_pham))
                {
                    dataQuery = dataQuery.Where(x => x.ma_san_pham.Contains(request.ma_san_pham));
                }

                if (!string.IsNullOrEmpty(request.ten_san_pham))
                {
                    dataQuery = dataQuery.Where(x => x.ten_san_pham.Contains(request.ten_san_pham));
                }

                if (request.fromDate != null && request.toDate != null)
                {
                    dataQuery = dataQuery.Where(x => x.Created > request.fromDate && x.Created < request.toDate);
                }

                var dataQueryDto = dataQuery
                .GroupBy(x => x.ma_san_pham)
                .Select(g => new SanPhamDto
                {
                    id = g.First().id,
                    ma_san_pham = g.Key,
                    ten_san_pham = g.First().ten_san_pham,
                    mo_ta = g.First().mo_ta,
                    danh_muc_id = g.First().danh_muc_id,
                    is_active = g.First().is_active,
                    xuat_xu = g.First().xuat_xu,
                    gia = g.First().gia,
                    khuyen_mai = g.First().khuyen_mai,
                    so_luong = g.Sum(y => y.so_luong),
                    Created = g.First().Created,
                });

                var result = await PaginatedList<SanPhamDto>.Create(dataQueryDto, request.pageNumber, request.pageSize);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginatedList<SanPhamDto>> GetAllSKU(SanPhamDto request)
        {
            try
            {
                IQueryable<san_pham> dataQuery = _context.san_pham.Where(x=> x.is_active == true).AsNoTracking();

                if (!string.IsNullOrEmpty(request.danh_muc_id.ToString()))
                {
                    dataQuery = dataQuery.Where(x => x.danh_muc_id.Equals(request.danh_muc_id));
                }

                var result = await PaginatedList<SanPhamDto>.Create(dataQuery.Select(x => new SanPhamDto
                {
                    id = x.id,
                    ten_danh_muc = _context.danh_muc.FirstOrDefault(y=> y.id == x.danh_muc_id)!.ten_danh_muc,
                    ma_san_pham = x.ma_san_pham,
                    ten_san_pham = x.ten_san_pham,
                    mo_ta = x.mo_ta,
                    danh_muc_id = x.danh_muc_id,
                    is_active = x.is_active,
                    xuat_xu = x.xuat_xu,
                    gia = x.gia,
                    sku = x.sku,
                    khuyen_mai = x.khuyen_mai,
                    so_luong = x.so_luong,
                    duong_dan_anh_bia = x.duong_dan_anh_bia,
                    Created = x.Created
                }), request.pageNumber, request.pageSize);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<List<SanPhamDto>> GetByMa([FromRoute]string ma)
        {
            var dataQuery = _context.san_pham.Where(x => x.ma_san_pham == ma);

            if(dataQuery.Count() > 0)
            {
                var ls_mau_sac = dataQuery.Where(x => x.mau_sac != null).GroupBy(x => x.mau_sac).Select(y => y.Key).ToList();
                var ls_size = dataQuery.Where(x=>x.size != null).GroupBy(x => x.size).Select(y => y.Key).ToList();
                var ls_phan_loai = new List<PhanLoai>();
                if(ls_mau_sac.Count() > 0)
                {
                    ls_phan_loai.Add(new PhanLoai
                    {
                        ten_phan_loai = "mau-sac",
                        phan_loai = ls_mau_sac!
                    });
                }
                
                if(ls_size.Count() > 0)
                {
                    ls_phan_loai.Add(new PhanLoai
                    {
                        ten_phan_loai = "size",
                        phan_loai = ls_size!
                    });
                }
                

                var dataResult = dataQuery.Select(x => new SanPhamDto
                {
                    id = x.id,
                    ma_san_pham = x.ma_san_pham,
                    ten_san_pham = x.ten_san_pham,
                    danh_muc_id = x.danh_muc_id,
                    duong_dan_anh_bia = x.duong_dan_anh_bia,
                    gia = x.gia,
                    khuyen_mai = x.khuyen_mai,
                    is_active = x.is_active,
                    mau_sac = x.mau_sac,
                    size = x.size,
                    sku = x.sku,
                    so_luong = x.so_luong,
                    mo_ta = x.mo_ta,
                    xuat_xu = x.xuat_xu,
                    ds_anh_san_pham = _context.anh_san_pham.Where(ls => ls.ma_san_pham == x.ma_san_pham).Select(y => y.duong_dan).ToList(),
                    ls_phan_loai = ls_phan_loai.ToList()
                }).ToList();
                return Task.FromResult(dataResult);
            }
            else
            {
                throw new Exception($"Không có dữ liệu của mã {ma}");
            }
            
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
