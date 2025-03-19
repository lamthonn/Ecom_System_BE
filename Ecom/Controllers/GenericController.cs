using backend_v3.Dto.Common;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using Ecom.Interfaces;

namespace Ecom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T> : ControllerBase where T : class, IEntity
    {
        private readonly IGenericRepository<T> _repository;
        public GenericController(IGenericRepository<T> repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<PaginatedList<T>>> GetAll([FromQuery] PaginParams paginParams)
        {
            return Ok(await _repository.GetAllAsync(paginParams));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetById(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<T>> Create(T entity)
        {
            var createdEntity = await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.id }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, T entity)
        {
            if (!await _repository.UpdateAsync(id, entity)) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _repository.DeleteAsync(id)) return NotFound();
            return NoContent();
        }

    }
}
