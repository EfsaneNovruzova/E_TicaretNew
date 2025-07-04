using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Domain.Entities;
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
        #endregion

        #region Servicies
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUserService, UserService>();
        #endregion
    }
}