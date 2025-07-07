using E_TicaretNew.Application.Abstracts.Repositories;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Infrastructure.Services;
using E_TicaretNew.Persistence.Repositories;
using E_TicaretNew.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_TicaretNew.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        #endregion

        #region Servicies
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        #endregion
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    }
}