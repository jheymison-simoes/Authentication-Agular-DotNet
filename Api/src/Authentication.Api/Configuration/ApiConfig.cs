﻿using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authentication.Business.Models;
using Authentication.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authentication.Api.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        var appSettingsSection = configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);
        var appSettings = appSettingsSection.Get<AppSettings>();
        
        //services.AddAuthenticatedJwt(appSettings);
        
        services.AddHealthChecks().AddDbContextCheck<SqlContext>();
        
        services.AddDbContext<SqlContext>(options =>
        {
            // options
            //     .LogTo(Console.WriteLine, LogLevel.Information)
            //     .EnableSensitiveDataLogging(
            //         AspnetcoreEnvironment.IsCurrentAspnetcoreEnvironmentValue(AspnetcoreEnvironmentEnum.Docker)
            //         || AspnetcoreEnvironment.IsCurrentAspnetcoreEnvironmentValue(AspnetcoreEnvironmentEnum
            //             .Development));
            
            options
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
        });
            
        services.AddMemoryCache();

        services.AddAutoMapper(typeof(Startup));

        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });
        
        services.AddApiVersioning(p =>
        {
            p.DefaultApiVersion = new ApiVersion(1, 0);
            p.ReportApiVersions = true;
            p.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.AddVersionedApiExplorer(p =>
        {
            p.GroupNameFormat = "'v'VVV";
            p.SubstituteApiVersionInUrl = true;
        });
    }
    
    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
                        
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("Total");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health/startup");
            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = _ => false });
            endpoints.MapControllers();
        });

        return app;
    }
}