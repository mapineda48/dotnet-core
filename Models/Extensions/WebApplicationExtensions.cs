namespace Agape.Models.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void ResetDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.EnsureDeleted();

                // Asegura la creación de la base de datos si no existe
                dbContext.Database.EnsureCreated();
            }
        }

        public static void EnsureCreatedDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Asegura la creación de la base de datos si no existe
                dbContext.Database.EnsureCreated();
            }
        }

        public static void InitializeDatabase(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.EnsureCreatedDatabase();
                return;
            }

            app.ResetDatabase();
        }
    }
}
