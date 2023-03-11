using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Authentication.Api.Configuration;
using Authentication.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Api;

public class Startup
{
    private IConfiguration Configuration { get; }
        
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiConfiguration(Configuration);
        services.AddResourceConfiguration();
        services.AddAuthenticatedJwt(Configuration);
        services.AddSwaggerConfiguration();
        services.DependencyInjection(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,  IApiVersionDescriptionProvider provider, SqlContext context)
    {
        context.Database.Migrate();
        app.UseApiConfiguration(env);
        app.UseSwaggerConfiguration(provider);
    }
}
