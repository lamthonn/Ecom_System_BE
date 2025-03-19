using AutoMapper;
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
        private readonly IMapper _mapper;

        public GenericRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _mapper = mapper;
        }

        // Phương thức hỗ trợ lọc dữ liệu bằng Expression
        // GET /api/items?pageNumber=1&pageSize=10&keySearch={"name":"Nam hip","email":"abc@gmail.com"}

        private IQueryable<T> ApplySearchFilter(IQueryable<T> query, string propertyName, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);
            Expression condition;

            if (property.Type == typeof(string))
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var valueExpression = Expression.Constant(value, typeof(string));
                condition = Expression.Call(property, method, valueExpression);
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
                var searchDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(paginParams.keySearch);
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
