using System;
using System.Diagnostics.CodeAnalysis;
using Climate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Climate
{
    public static class ClimateExtensionMethods
    {
        public static IServiceCollection AddClimateContext(this IServiceCollection services, [NotNull]Action<AppDbContext> configure)
        {
            var dbStringConnection = Environment.GetEnvironmentVariable("SQLSERVER") ?? throw new NullReferenceException("missing enviroment variable SQLSERVER");

            services.AddDbContext<AppDbContext>(opt =>
                                  opt.UseLazyLoadingProxies()
                                  .UseSqlServer(dbStringConnection));

            services.Configure(configure);

            return services;
        }
    }

}