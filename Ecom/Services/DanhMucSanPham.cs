using Azure.Core;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Services
{
    public class DanhMucSanPham : IDanhMucSanPhamService
    {
        private readonly AppDbContext _context;
        public DanhMucSanPham(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<DanhMucDto>> GetAll(DanhMucDto request)
        {
            try
            {
                var dataQuery = _context.danh_muc.AsNoTracking();

                if (!string.IsNullOrEmpty(request.ma_danh_muc))
                {
                    dataQuery = dataQuery.Where(x => x.ma_danh_muc.Contains(request.ma_danh_muc));
                }

                if (!string.IsNullOrEmpty(request.ten_danh_muc))
                {
                    dataQuery = dataQuery.Where(x => x.ten_danh_muc.Contains(request.ten_danh_muc));
                }

                if (request.Created != null)
                {
                    dataQuery = dataQuery.Where(x => x.Created == request.Created);
                }

                var dataQueryDto = dataQuery.Select(x => new DanhMucDto
                {
                    id = x.id,
                    ma_danh_muc = x.ma_danh_muc,
                    ten_danh_muc = x.ten_danh_muc,
                    mo_ta = x.mo_ta,
                    Created = x.Created,
                    CreatedBy = x.CreatedBy,
                });

                var result = PaginatedList<DanhMucDto>.Create(dataQueryDto, request.pageNumber, request.pageSize);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<DanhMucDto> GetById(string id)
        {
            try
            {
                var dataQuery = _context.danh_muc.FirstOrDefault(x => x.id.Equals(id));
                if (dataQuery != null)
                {
                    var result = new DanhMucDto{
                        id = dataQuery.id,
                        ma_danh_muc = dataQuery.ma_danh_muc,
                        ten_danh_muc = dataQuery.ten_danh_muc,
                        Created = dataQuery.Created,
                        mo_ta = dataQuery.mo_ta,
                        CreatedBy = dataQuery.CreatedBy,
                        LastModified = dataQuery.LastModified,
                        LastModifiedBy = dataQuery.LastModifiedBy
                    };
                    return Task.FromResult(result);
                }
                else
                {
                    throw new Exception($"Không tìm thấy danh mục với id {id}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<DanhMucDto> create(DanhMucDto request)
        {
            try
            {
                var dataQuery = _context.danh_muc.FirstOrDefault(x => x.ma_danh_muc.Equals(request.ma_danh_muc));
                if (dataQuery == null)
                {
                    var result = new danh_muc
                    {
                        id = Guid.NewGuid(),
                        ma_danh_muc = request.ma_danh_muc!,
                        ten_danh_muc = request.ten_danh_muc!,
                        Created = DateTime.Now,
                        mo_ta = request.mo_ta!,
                        CreatedBy = request.CreatedBy,
                        LastModified = DateTime.Now,
                        LastModifiedBy = request.LastModifiedBy
                    };
                    _context.danh_muc.Add(result);
                    _context.SaveChanges();
                    return Task.FromResult(request);
                }
                else
                {
                    throw new Exception($"mã danh mục đã tồn tại");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Edit(string id, DanhMucDto request)
        {
            try
            {
                var dataQuery = _context.danh_muc.FirstOrDefault(x => x.id.ToString() == id );
                if (dataQuery != null)
                {
                    dataQuery.ma_danh_muc = request.ma_danh_muc;
                    dataQuery.ten_danh_muc = request.ten_danh_muc;
                    dataQuery.mo_ta = request.mo_ta;
                    dataQuery.LastModified = DateTime.Now;
                    dataQuery.LastModifiedBy = request.LastModifiedBy;

                    _context.danh_muc.Update(dataQuery);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception($"Không tìm thấy danh mục với id {id}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(string id)
        {
            try
            {
                var dataQuery = _context.danh_muc.FirstOrDefault(x => x.id.ToString().Equals(id));
                if (dataQuery != null)
                {
                    _context.danh_muc.Remove(dataQuery);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception($"Không tìm thấy danh mục với id {id}");
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            
        }

        public void DeleteAny(List<string> ids)
        {
            try
            {
                var dataQuery = _context.danh_muc.Where(x => ids.Contains(x.id.ToString())).ToList();
                _context.danh_muc.RemoveRange(dataQuery);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] ExportToExcel()
        {
            var danhMucs = _context.danh_muc.ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DanhMucSanPham");

                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã danh mục";
                worksheet.Cells[1, 3].Value = "Tên danh mục";
                worksheet.Cells[1, 4].Value = "Mô tả";
                worksheet.Cells[1, 5].Value = "Ngày tạo";

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Ghi dữ liệu danh mục vào file Excel
                int row = 2;
                int stt = 1; // Initialize the sequence number
                foreach (var danhMuc in danhMucs)
                {
                    worksheet.Cells[row, 1].Value = stt; // Set the sequence number
                    worksheet.Cells[row, 2].Value = danhMuc.ma_danh_muc;
                    worksheet.Cells[row, 3].Value = danhMuc.ten_danh_muc;
                    worksheet.Cells[row, 4].Value = danhMuc.mo_ta;
                    worksheet.Cells[row, 5].Value = danhMuc.Created!.ToString("dd/MM/yyyy HH:mm:ss");
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
