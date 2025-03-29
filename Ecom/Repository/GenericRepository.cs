using AutoMapper;
using backend_v3.Dto.Common;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Linq.Expressions;
using Ecom.Dto.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ecom.Repository
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IMapper _mapper;

        public GenericRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _mapper = mapper;
        }

        // Phương thức hỗ trợ lọc dữ liệu bằng Expression
        // GET /api/items?pageNumber=1&pageSize=10&keySearch={"name":"Nam hip","email":"abc@gmail.com"}

        private IQueryable<T> ApplySearchFilter(IQueryable<T> query, string propertyName, object value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);
            Expression condition;

            if (property.Type == typeof(string) && value is string stringValue)
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var valueExpression = Expression.Constant(stringValue, typeof(string));
                condition = Expression.Call(property, method, valueExpression);
            }
            else if (property.Type == typeof(bool) && value is bool boolValue) // Xử lý bool
            {
                var valueExpression = Expression.Constant(boolValue, typeof(bool));
                condition = Expression.Equal(property, valueExpression);
            }
            else if (property.Type == typeof(bool?)) // Xử lý bool?
            {
                if (value is bool nullableBoolValue)
                {
                    var hasValueProperty = Expression.Property(property, "HasValue");
                    var valueProperty = Expression.Property(property, "Value");
                    var valueExpression = Expression.Constant(nullableBoolValue, typeof(bool));

                    // Điều kiện: x.trang_thai.HasValue && x.trang_thai.Value == boolValue
                    var notNullCheck = Expression.Equal(hasValueProperty, Expression.Constant(true));
                    var valueCheck = Expression.Equal(valueProperty, valueExpression);
                    condition = Expression.AndAlso(notNullCheck, valueCheck);
                }
                else
                {
                    throw new ArgumentException($"Cannot convert {value} to boolean.");
                }
            }
            else
            {
                var convertedValue = Convert.ChangeType(value, property.Type);
                var valueExpression = Expression.Constant(convertedValue, property.Type);
                condition = Expression.Equal(property, valueExpression);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            return query.Where(lambda);
        }



        public async Task<PaginatedList<TDto>> GetAllAsync<TDto>(PaginParams paginParams)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(paginParams.keySearch))
            {
                var searchDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(paginParams.keySearch);
                if (searchDict != null)
                {
                    foreach (var filter in searchDict)
                    {
                        query = ApplySearchFilter(query, filter.Key, filter.Value);
                    }
                }
            }

            var paginatedResult = await PaginatedList<T>.Create(query, paginParams.pageNumber, paginParams.pageSize);
            return _mapper.Map<PaginatedList<TDto>>(paginatedResult);
        }


        public async Task<TDto> GetByIdAsync<TDto>(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> AddAsync<TDto>(TDto dto)
        {
            var entity = _mapper.Map<T>(dto);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TDto>(entity);
        }

        public async Task<bool> UpdateAsync<TDto>(Guid id, TDto dto)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null) return false;

            _mapper.Map(dto, existingEntity);
            _context.Entry(existingEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMultipleAsync(List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return false; // Không có ID nào để xóa
            }

            try
            {
                // Tìm tất cả các bản ghi có ID nằm trong danh sách ids
                var entities = await _dbSet
                    .Where(e => ids.Contains(e.id))
                    .ToListAsync();

                if (!entities.Any())
                {
                    return false; // Không tìm thấy bản ghi nào để xóa
                }

                // Xóa các bản ghi
                _dbSet.RemoveRange(entities);

                // Lưu thay đổi vào cơ sở dữ liệu
                int changes = await _context.SaveChangesAsync();
                return changes > 0; // Trả về true nếu có ít nhất 1 bản ghi được xóa
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa nhiều bản ghi: {ex.Message}");
                return false;
            }
        }
        public byte[] ExportToExcelDynamic<TData>(IEnumerable<TData> data, List<ExcelColumnDto> columns, string sheetName = "Sheet1")
        {
            if (data == null || !data.Any())
            {
                throw new ArgumentException("Dữ liệu đầu vào không được rỗng.");
            }

            if (columns == null || !columns.Any())
            {
                throw new ArgumentException("Danh sách cột không được rỗng.");
            }


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);

                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "STT";
                for (int i = 0; i < columns.Count; i++)
                {
                    worksheet.Cells[1, i + 2].Value = columns[i].DisplayName;
                }

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[1, 1, 1, columns.Count + 1])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Ghi dữ liệu vào file Excel
                int row = 2;
                int stt = 1;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = stt; // Số thứ tự

                    // Ghi giá trị cho từng cột
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var column = columns[i];
                        var propertyInfo = item.GetType().GetProperty(column.PropertyName);
                        if (propertyInfo != null)
                        {
                            var value = propertyInfo.GetValue(item);
                            if (value != null)
                            {
                                // 📌 Nếu giá trị là kiểu bool, áp dụng BoolTrueValue / BoolFalseValue
                                if (value is bool boolValue)
                                {
                                    worksheet.Cells[row, i + 2].Value = boolValue
                                        ? column.BoolTrueValue ?? "True"
                                        : column.BoolFalseValue ?? "False";
                                }
                                // 📌 Nếu có định dạng ngày tháng
                                else if (!string.IsNullOrEmpty(column.Format) && value is DateTime dateValue)
                                {
                                    worksheet.Cells[row, i + 2].Value = dateValue.ToString(column.Format);
                                }
                                else
                                {
                                    worksheet.Cells[row, i + 2].Value = value;
                                }
                            }
                            else
                            {
                                worksheet.Cells[row, i + 2].Value = string.Empty;
                            }
                        }
                        else
                        {
                            worksheet.Cells[row, i + 2].Value = string.Empty;
                            Console.WriteLine($"Thuộc tính {column.PropertyName} không tồn tại trong dữ liệu.");
                        }
                    }

                    row++;
                    stt++;
                }

                // Auto-fit cột
                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
    }

}
