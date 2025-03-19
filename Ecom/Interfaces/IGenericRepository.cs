using backend_v3.Dto.Common;
using backend_v3.Models;
using System.Security.Principal;

namespace Ecom.Interfaces
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<PaginatedList<TDto>> GetAllAsync<TDto>(PaginParams paginParams);
        Task<TDto> GetByIdAsync<TDto>(Guid id);
        Task<TDto> AddAsync<TDto>(TDto dto);
        Task<bool> UpdateAsync<TDto>(Guid id, TDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
