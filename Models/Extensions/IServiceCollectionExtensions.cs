using Microsoft.EntityFrameworkCore;

namespace Agape.Models.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddAppDbContext(this IServiceCollection services, string connectionString)
        {
            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        }
    }
}
