using E_TicaretNew.Application.Abstracts.Repositories;
using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Persistence.Contexts;

namespace E_TicaretNew.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(E_TicaretNewDbContext context) : base(context)
    {
    }
}
