using backend_v3.Dto.Common;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Ecom.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        // GET /api/items?pageNumber=1&pageSize=10&keySearch={"name":"Nam hip","email":"abc@gmail.com"}
        private IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string propertyName, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);

            Expression condition;

            // Kiểm tra nếu thuộc tính là kiểu string thì dùng Contains
            if (property.Type == typeof(string))
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var valueExpression = Expression.Constant(value, typeof(string));
                condition = Expression.Call(property, method, valueExpression);
            }
            else
            {
                // Nếu không phải string thì so sánh ==
                var convertedValue = Convert.ChangeType(value, property.Type);
                var valueExpression = Expression.Constant(convertedValue, property.Type);
                condition = Expression.Equal(property, valueExpression);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            return query.Where(lambda);
        }

        public async Task<PaginatedList<T>> GetAllAsync(PaginParams paginParams)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(paginParams.keySearch))
            {
                var searchDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(paginParams.keySearch);
                if (searchDict != null)
                {
                    foreach (var filter in searchDict)
                    {
                        query = ApplySearchFilter(query, filter.Key, filter.Value);
                    }
                }
            }
            return await Task.Run(() => PaginatedList<T>.Create(query, paginParams.pageNumber, paginParams.pageSize));
        }
        public async Task<T> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> UpdateAsync(Guid id, T entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null) return false;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
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
    }
}
