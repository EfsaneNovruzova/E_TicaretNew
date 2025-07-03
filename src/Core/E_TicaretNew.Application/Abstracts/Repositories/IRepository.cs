using E_Ticaret.Domain.Entities;
using System.Linq.Expressions;

namespace E_TicaretNew.Application.Abstracts.Repositories;

public interface IRepository<T> where T : BaseEntity, new()
{
    Task<T?> GetByIdAsync(Guid id);
    IQueryable<T> GetByFiltered(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        bool inTracking = false);
    IQueryable<T> GetAll(bool isTracking = false);
    IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isOrderByAsc = true,
        bool inTracking = false);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangeAsync();

}
