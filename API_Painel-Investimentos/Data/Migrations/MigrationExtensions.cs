using Microsoft.EntityFrameworkCore;

namespace API_Painel_Investimentos.Data.Migrations
{
    public static class MigrationExtensions
    {
        public static IHost MigrateAndSeed<TContext>(
            this IHost host,
            Action<TContext, IServiceProvider>? seedAction = null)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<TContext>();

                    dbContext.Database.Migrate();

                    seedAction?.Invoke(dbContext, services);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro ao migrar e/ou popular o banco de dados {typeof(TContext).Name}: {ex.Message}");
                }
            }
            return host;
        }
    }
}