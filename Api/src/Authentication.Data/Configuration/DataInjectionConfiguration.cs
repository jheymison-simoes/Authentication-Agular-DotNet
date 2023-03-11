using Microsoft.Extensions.DependencyInjection;
using Authentication.Business.Interfaces.Repositories;
using Authentication.Data.Repositories;
using Authentication.Domain.Models;

namespace Authentication.Data.Configuration;

public static class DataInjectionConfiguration
{
    public static void DependencyInjection(this IServiceCollection services)
    {
        InjectionDependencyRepository(services);
        InjectionDependencyUniOfWork(services);
    }

    private static void InjectionDependencyRepository(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
    
    private static void InjectionDependencyUniOfWork(IServiceCollection services)
    {

    }
}