using Microsoft.EntityFrameworkCore;

namespace API_Painel_Investimentos.Data.Migrations
{
    public static class MigrationExtensions
    {
        public static IHost MigrateDatabases<TContext>(this IHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<TContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocorreu um erro ao migrar o banco de dados: " + ex.Message);
                }
            }
            return host;
        }
    }
}
