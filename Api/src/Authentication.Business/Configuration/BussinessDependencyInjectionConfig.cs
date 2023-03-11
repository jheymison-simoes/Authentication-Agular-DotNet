using Authentication.Business.Interfaces.Services;
using Authentication.Business.Services;
using Authentication.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Authentication.Business.Models.User.Request;

namespace Authentication.Business.Configuration
{
    public static class BussinessDependencyInjectionConfig
    {
        public static void DependencyInjection(this IServiceCollection services)
        {
            services.DependencyInjectionServices();
            services.DependencyInjectionValidators();
        }

        private static void DependencyInjectionValidators(this IServiceCollection services)
        {
            #region Bussiness
            services.AddScoped<LoginRequestValidator>();
            services.AddScoped<CreateUserRequestValidator>();
            #endregion

            #region Domain
            services.AddScoped<UserValidator>();
            #endregion
        }

        private static void DependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthenticatedService, AuthenticatedService>();
            services.AddScoped<IEncryptService, EncryptService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
