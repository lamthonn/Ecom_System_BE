using backend_v3.Dto.Common;
using backend_v3.Models;
using System.Security.Principal;

namespace Ecom.Interfaces
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<PaginatedList<T>> GetAllAsync(PaginParams paginParams);
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(Guid id, T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
