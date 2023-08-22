using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(Guid Id);
        Task UpdateAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> includeExpression);
        Task<T> GetAsync(Guid Id);
        Task<T> GetAsync(Guid Id, Expression<Func<T, object>> includeExpression);
    }
}