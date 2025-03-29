using Ecom.Context;
using Ecom.Dto.QuanLySanPham;
using Ecom.Interfaces;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Ecom.Dto.VanHanh;
using Ecom.Entity;
using AutoMapper;
using System.Linq;

namespace Ecom.Services
{
    public class PhieuNhapKhoService : IPhieuNhapKhoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public PhieuNhapKhoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PhieuNhapKhoDto> Create(PhieuNhapKhoDto request)
        {
            try
            {
                var isDuplicate = _context.phieu_nhap_kho.FirstOrDefault(x => x.ma == request.ma);
                if (isDuplicate != null)
                {
                    throw new Exception("Mã phiếu nhập đã tồn tại");
                }
                var newPhieuNhap = new phieu_nhap_kho
                {
                    id = Guid.NewGuid(),
                    ma = request.ma ?? Guid.NewGuid().ToString(),
                    ngay_du_kien = request.ngay_du_kien ?? DateTime.Now.AddDays(1),
                    ngay_het_han = request.ngay_het_han ?? null,
                    nha_cung_cap = request.nha_cung_cap ?? null,
                    ghi_chu = request.ghi_chu ?? null,
                    trang_thai = 1,
                    Created = DateTime.Now,
                };
                _context.phieu_nhap_kho.Add(newPhieuNhap);

                List<chi_tiet_phieu_nhap_kho> newChiTiet = new List<chi_tiet_phieu_nhap_kho>();
                foreach (var item in request.ls_san_phan_nhap_kho ?? [])
                {
                    var newItem = new chi_tiet_phieu_nhap_kho
                    {
                        id = Guid.NewGuid(),
                        san_pham_id = item.san_pham_id ?? Guid.NewGuid(),
                        phieu_nhap_kho_id = newPhieuNhap.id,
                        ma_san_pham = item.ma_san_pham ?? "",
                        sku = item.sku ?? "",
                        so_luong = item.so_luong ?? 0,
                        don_gia = item.don_gia ?? 0,
                        Created = DateTime.Now,
                    };

                    newChiTiet.Add(newItem);
                }
                _context.chi_tiet_phieu_nhap_kho.AddRange(newChiTiet);
                await _context.SaveChangesAsync();

                return new PhieuNhapKhoDto
                {
                    id = newPhieuNhap.id,
                    ma = newPhieuNhap.ma,
                    ngay_het_han = newPhieuNhap.ngay_het_han,
                    ngay_du_kien = newPhieuNhap.ngay_du_kien,
                    nha_cung_cap = newPhieuNhap.nha_cung_cap,
                    ghi_chu = newPhieuNhap.ghi_chu,
                    ls_san_phan_nhap_kho = _mapper.Map<List<ChiTietPhieuNhapDto>>(newChiTiet.ToList())
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PhieuNhapKhoDto> Edit(string id, PhieuNhapKhoDto request)
        {
            try
            {
                var data = _context.phieu_nhap_kho.FirstOrDefault(x => x.id.ToString() == id);
                if (data == null)
                {
                    throw new Exception("không tìm thấy phiếu nhập");
                }

                data.ma = request.ma;
                data.ngay_du_kien = request.ngay_du_kien ?? DateTime.Now;
                data.ngay_het_han = request.ngay_het_han;
                data.nha_cung_cap = request.nha_cung_cap;
                data.ghi_chu = request.ghi_chu;

                _context.phieu_nhap_kho.Update(data);
                foreach (var item in request.ls_san_phan_nhap_kho ?? [])
                {
                    var existingRecord = _context.chi_tiet_phieu_nhap_kho.FirstOrDefault(x => x.san_pham_id == item.san_pham_id);
                    if (existingRecord != null)
                    {
                        // Update the existing record with the values from the item
                        existingRecord.ma_san_pham = item.ma_san_pham!;
                        existingRecord.sku = item.sku!;
                        existingRecord.so_luong = item.so_luong ?? 0;
                        existingRecord.don_gia = item.don_gia ?? 0;
                        _context.chi_tiet_phieu_nhap_kho.Update(existingRecord);
                    }
                    else
                    {
                        // Add a new record to the database
                        var newRecord = new chi_tiet_phieu_nhap_kho
                        {
                            id = Guid.NewGuid(),
                            san_pham_id = item.san_pham_id ?? Guid.NewGuid(),
                            phieu_nhap_kho_id = data.id,
                            ma_san_pham = item.ma_san_pham ?? "",
                            sku = item.sku ?? "",
                            so_luong = item.so_luong ?? 0,
                            don_gia = item.don_gia ?? 0,
                            Created = DateTime.Now,
                        };
                        _context.chi_tiet_phieu_nhap_kho.Add(newRecord);
                    }
                }

                // Remove records from the database that are not present in ls_san_phan_nhap_kho
                var recordsToRemove = _context.chi_tiet_phieu_nhap_kho.ToList().Where(x => !request.ls_san_phan_nhap_kho.AsEnumerable().Any(item => item.san_pham_id == x.san_pham_id));
                _context.chi_tiet_phieu_nhap_kho.RemoveRange(recordsToRemove);
                await _context.SaveChangesAsync();

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] ExportToExcel()
        {
            var datas = _context.phieu_nhap_kho.ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DanhSachPhieuNhapKho");

                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã phiếu";
                worksheet.Cells[1, 3].Value = "Ngày nhận dự kiến";
                worksheet.Cells[1, 4].Value = "Ngày hết hạn";
                worksheet.Cells[1, 5].Value = "Nhà cung cấp";
                worksheet.Cells[1, 6].Value = "Trạng thái";
                worksheet.Cells[1, 7].Value = "Ghi chú";
                worksheet.Cells[1, 8].Value = "Ngày tạo";

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[1, 1, 1, 8])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Ghi dữ liệu danh mục vào file Excel
                int row = 2;
                int stt = 1; // Initialize the sequence number
                foreach (var data in datas)
                {
                    worksheet.Cells[row, 1].Value = stt; // Set the sequence number
                    worksheet.Cells[row, 2].Value = data.ma;
                    worksheet.Cells[row, 3].Value = data.ngay_du_kien;
                    worksheet.Cells[row, 4].Value = data.ngay_het_han;
                    worksheet.Cells[row, 5].Value = data.nha_cung_cap;
                    worksheet.Cells[row, 6].Value = data.trang_thai == 1 ? "Phiếu mới" : (data.trang_thai == 2 ? "Hết hạn" : "Hoàn thành");
                    worksheet.Cells[row, 7].Value = data.ghi_chu;
                    worksheet.Cells[row, 8].Value = data.Created!.ToString("dd/MM/yyyy HH:mm:ss");
                    row++;
                    stt++; // Increment the sequence number
                }

                // Auto-fit cột
                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        public async Task<PhieuNhapKhoDto> GetById(string id)
        {
            try
            {
                var data = _context.phieu_nhap_kho.FirstOrDefault(x => x.id.ToString() == id);
                if (data == null)
                {
                    throw new Exception("không tìm thấy phiếu nhập");
                }

                return new PhieuNhapKhoDto
                {
                    id = data.id,
                    ma = data.ma,
                    ghi_chu = data.ghi_chu,
                    ngay_du_kien = data.ngay_du_kien,
                    ngay_het_han = data.ngay_het_han,
                    Created = data.Created,
                    nha_cung_cap = data.nha_cung_cap,
                    trang_thai = data.trang_thai,
                    thanh_tien = data.thanh_tien,
                    ls_san_phan_nhap_kho = _context.chi_tiet_phieu_nhap_kho.Where(y => y.phieu_nhap_kho_id == data.id).Select(x => new ChiTietPhieuNhapDto
                    {
                        id = x.id,
                        ma_san_pham = x.ma_san_pham,
                        don_gia = x.don_gia,
                        phieu_nhap_kho_id = x.phieu_nhap_kho_id,
                        san_pham_id = x.san_pham_id,
                        sku = x.sku,
                        so_luong = x.so_luong,
                        san_pham_dto =  _mapper.Map<SanPhamDto>(_context.san_pham.FirstOrDefault(z => z.id == x.san_pham_id)),
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void XuLyPhieuNhap(string id)
        {
            try
            {
                var data = _context.phieu_nhap_kho.FirstOrDefault(x => x.id.ToString() == id);
                if (data == null)
                {
                    throw new Exception("không tìm thấy phiếu nhập");
                }
                data.trang_thai = 3;
                _context.phieu_nhap_kho.Update(data);

                var dataChiTiet = _context.chi_tiet_phieu_nhap_kho
                .Where(x => x.phieu_nhap_kho_id == data.id)
                .ToDictionary(y => y.san_pham_id, y => y.so_luong);

                foreach (var item in dataChiTiet)
                {
                    var sanPham = _context.san_pham.FirstOrDefault(x => x.id == item.Key);
                    if (sanPham == null)
                    {
                        throw new Exception($"không tìm thấy sản phẩm có id {item.Key}");
                    }
                    sanPham.so_luong += item.Value;
                    _context.san_pham.Update(sanPham);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex) { 
                throw new Exception(ex.Message);
            }
        }
    }
}
