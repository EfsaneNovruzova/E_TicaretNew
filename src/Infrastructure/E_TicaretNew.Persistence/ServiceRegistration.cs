using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_TicaretNew.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories

        #endregion

        #region Servicies
        services.AddScoped<IUserService, UserService>();
        #endregion
    }
}