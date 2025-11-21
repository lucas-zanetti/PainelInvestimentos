using API_Painel_Investimentos.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_Painel_Investimentos.Data.Contexts
{
    public class DbTelemetriaContext : DbContext
    {
        public DbTelemetriaContext() { }

        public DbTelemetriaContext(DbContextOptions<DbTelemetriaContext> options) : base(options) { }

        public virtual DbSet<TelemetriaEntity> DadosTelemetria { get; set; }
    }
}
