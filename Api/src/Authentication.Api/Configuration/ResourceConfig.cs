using System.Globalization;
using System.Resources;
using Authentication.Api.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Api.Configuration;

public static class ResourceConfig
{
    public static IServiceCollection AddResourceConfiguration(this IServiceCollection services)
    {
        services.AddSingleton(new ResourceManager(typeof(ApiResource)));
        services.AddScoped(provider =>
        {
            var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;
            if (httpContext is null) return CultureInfo.InvariantCulture;
            return httpContext.Request.Headers.TryGetValue("language", out var language)
                ? new CultureInfo(language)
                : CultureInfo.InvariantCulture;
        });
        return services;
    }
}