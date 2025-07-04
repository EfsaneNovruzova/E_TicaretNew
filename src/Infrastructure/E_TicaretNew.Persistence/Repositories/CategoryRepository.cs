using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Persistence.Contexts;

namespace E_TicaretNew.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(E_TicaretNewDbContext context) : base(context)
    {
    }
}
