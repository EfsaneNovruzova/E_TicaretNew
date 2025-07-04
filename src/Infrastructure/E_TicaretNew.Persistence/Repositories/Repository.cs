using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Application.Abstracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using E_TicaretNew.Persistence.Contexts;

namespace E_TicaretNew.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    private E_TicaretNewDbContext _context { get; }
    private DbSet<T> Table { get; }// istediyimiz tipde entity temsil edir meselen bio
    public Repository(E_TicaretNewDbContext context)
    {
        _context = context;
        Table = _context.Set<T>();
    }
    public async Task AddAsync(T entity)
    {
        await Table.AddAsync(entity);
    }
    public void Update(T entity)
    {
        Table.Update(entity);
    }

    public async void Delete(T entity)
    {
        Table.Remove(entity);
    }
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await Table.FindAsync(id);
    }
    public IQueryable<T> GetByFiltered(Expression<Func<T, bool>>? predicate = null,
     Expression<Func<T, object>>[]? include = null,
     bool inTracking = false)
    {
        IQueryable<T> query = Table;
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        if (include is not null)
        {
            foreach (var includeExpression in include)
            {
                query = query.Include(includeExpression);
            }
        }
        if (!inTracking)
        {
            query = query.AsNoTracking();
        }
        return query;
    }

    public IQueryable<T> GetAll(bool isTracking = false)
    {
        if (!isTracking)
        {
            return Table.AsNoTracking();
        }
        return Table;
    }

    public IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isOrderByAsc = true,
        bool inTracking = false)
    {
        IQueryable<T> query = Table;
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        if (include is not null)
        {
            foreach (var includeExpression in include)
            {
                query = query.Include(includeExpression);
            }
        }
        if (orderBy is not null)
        {
            if (isOrderByAsc)
            {
                query = query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(orderBy);
            }
        }
        if (!inTracking)
        {
            query = query.AsNoTracking();
        }
        return query;
    }





    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }


}
