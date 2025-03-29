using backend_v3.Dto.Common;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using Ecom.Interfaces;
using AutoMapper;
using Ecom.Dto.Common;

namespace Ecom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T, TDto> : ControllerBase
     where T : class, IEntity
     where TDto : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IMapper _mapper; // Sử dụng AutoMapper để chuyển đổi Entity -> DTO

        public GenericController(IGenericRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TDto>>> GetAll([FromQuery] PaginParams paginParams)
        {
            var result = await _repository.GetAllAsync<TDto>(paginParams);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TDto>> GetById(Guid id)
        {
            var entity = await _repository.GetByIdAsync<TDto>(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<TDto>> Create([FromBody] TDto dto)
        {
            var entity = _mapper.Map<T>(dto); // Chuyển đổi từ DTO sang Entity
            var createdEntity = await _repository.AddAsync(entity);
            var createdDto = _mapper.Map<TDto>(createdEntity); // Chuyển đổi lại từ Entity sang DTO

            return CreatedAtAction(nameof(GetById), new { id = createdEntity.id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TDto dto)
        {
            var entity = _mapper.Map<T>(dto);

            var updated = await _repository.UpdateAsync(id, entity);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
        [HttpDelete("delete-multiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<Guid> ids)
        {
            try
            {
                var deleted = await _repository.DeleteMultipleAsync(ids);
                if (!deleted)
                {
                    return NotFound("Không tìm thấy bản ghi nào để xóa.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi xóa nhiều bản ghi: {ex.Message}");
            }
        }

        [HttpPost("export-excel")]
        public async Task<IActionResult> ExportToExcel([FromBody] ExportExcelRequest request)
        {
            try
            {
                // Lấy dữ liệu từ repository (có thể áp dụng phân trang hoặc lọc nếu cần)
                var paginParams = new PaginParams
                {
                    pageNumber = 1,
                    pageSize = int.MaxValue, // Lấy tất cả dữ liệu
                    keySearch = request.KeySearch // Nếu có lọc dữ liệu
                };

                var paginatedData = await _repository.GetAllAsync<TDto>(paginParams);
                var data = paginatedData.Items; // Lấy danh sách DTO

                if (data == null || !data.Any())
                {
                    return BadRequest("Không có dữ liệu để xuất.");
                }

                // Gọi phương thức ExportToExcelDynamic từ repository
                var fileContent = _repository.ExportToExcelDynamic(data, request.Columns, request.SheetName ?? "Sheet1");

                // Trả về file Excel
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"{request.SheetName ?? "ExportData"}.xlsx";
                return File(fileContent, contentType, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi xuất file Excel: {ex.Message}");
            }
        }

    }
    public class ExportExcelRequest
    {
        public List<ExcelColumnDto> Columns { get; set; }
        public string SheetName { get; set; }
        public string KeySearch { get; set; } // Để lọc dữ liệu nếu cần
    }


}
