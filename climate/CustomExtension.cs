using System;
using Climate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Climate
{
    public static class ClimateExtensionMethods
    {
        public static IServiceCollection AddClimateContext(this IServiceCollection services)
        {
            var dbStringConnection = Environment.GetEnvironmentVariable("SQLSERVER") ?? throw new NullReferenceException("missing enviroment variable SQLSERVER");

            services.AddDbContext<AppDbContext>(opt =>
                                  opt.UseLazyLoadingProxies()
                                  .UseSqlServer(dbStringConnection));

            return services;
        }
    }

}