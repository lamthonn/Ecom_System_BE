using backend_v3.Dto.Common;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using Ecom.Interfaces;
using AutoMapper;

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
            if (id != entity.id) return BadRequest("ID mismatch");

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
    }


}
