
namespace Agape.Storage.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddStorageService(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IStorageService>(new MinioService(connectionString));
        }
    }
}
