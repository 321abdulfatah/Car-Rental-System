using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(Guid Id);
        Task UpdateAsync(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(IEnumerable<Expression<Func<T, object>>> includeExpressions);
        Task<T> GetAsync(Guid Id);
        Task<T> GetAsync(Guid Id, IEnumerable<Expression<Func<T, object>>> includeExpressions);
    }
}