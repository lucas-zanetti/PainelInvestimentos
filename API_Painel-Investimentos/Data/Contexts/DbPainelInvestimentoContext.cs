using Microsoft.EntityFrameworkCore;
using API_Painel_Investimentos.Data.Entities;

namespace API_Painel_Investimentos.Data.Contexts
{
    public class DbPainelInvestimentoContext(DbContextOptions<DbPainelInvestimentoContext> options) : DbContext(options)
    {
        public DbSet<ClienteEntity> Clientes => Set<ClienteEntity>();
        public DbSet<ProdutoEntity> Produtos => Set<ProdutoEntity>();
        public DbSet<PerfilRiscoEntity> PerfisRisco => Set<PerfilRiscoEntity>();
        public DbSet<SimulacaoEntity> Simulacoes => Set<SimulacaoEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClienteEntity>().Property(c => c.Id).ValueGeneratedNever();

            modelBuilder.Entity<ClienteEntity>()
                .HasMany(c => c.Investimentos)
                .WithOne(i => i.Cliente)
                .HasForeignKey(i => i.ClienteId)
                .IsRequired();

            modelBuilder.Entity<ProdutoEntity>()
                .HasOne(p => p.PerfilRisco)
                .WithMany()
                .HasForeignKey(p => p.PerfilRiscoId)
                .IsRequired();

            modelBuilder.Entity<SimulacaoEntity>()
                .HasOne(s => s.Cliente)
                .WithMany()
                .HasForeignKey(s => s.ClienteId)
                .IsRequired();

            modelBuilder.Entity<SimulacaoEntity>()
                .HasOne(s => s.Produto)
                .WithMany()
                .HasForeignKey(s => s.ProdutoId)
                .IsRequired();
        }
    }
}