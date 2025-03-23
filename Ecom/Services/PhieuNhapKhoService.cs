using Ecom.Context;
using Ecom.Dto.QuanLySanPham;
using Ecom.Interfaces;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace Ecom.Services
{
    public class PhieuNhapKhoService : IPhieuNhapKhoService
    {
        private readonly AppDbContext _context;
        public PhieuNhapKhoService(AppDbContext context)
        {
            _context = context;
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

    }
}
