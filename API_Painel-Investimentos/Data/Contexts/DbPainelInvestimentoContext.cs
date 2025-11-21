using Microsoft.EntityFrameworkCore;
using API_Painel_Investimentos.Data.Entities;
using API_Painel_Investimentos.Enums;

namespace API_Painel_Investimentos.Data.Contexts
{
    public class DbPainelInvestimentoContext(DbContextOptions<DbPainelInvestimentoContext> options) : DbContext(options)
    {
        public DbSet<ClienteEntity> Clientes => Set<ClienteEntity>();
        public DbSet<InvestimentoEntity> Investimentos => Set<InvestimentoEntity>();
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

            modelBuilder.Entity<InvestimentoEntity>()
                .HasOne(i => i.Cliente)
                .WithMany(c => c.Investimentos)
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

            modelBuilder.Entity<PerfilRiscoEntity>().HasData(
                new PerfilRiscoEntity
                {
                    Id = 1,
                    NomePerfil = EnumPerfilRisco.Conservador.ToString(),
                    DescricaoPerfil = "Baixa exposição a risco, foco em preservação de capital."
                },
                new PerfilRiscoEntity
                {
                    Id = 2,
                    NomePerfil = EnumPerfilRisco.Moderado.ToString(),
                    DescricaoPerfil = "Equilíbrio entre risco e retorno."
                },
                new PerfilRiscoEntity
                {
                    Id = 3,
                    NomePerfil = EnumPerfilRisco.Agressivo.ToString(),
                    DescricaoPerfil = "Alta exposição a risco, busca maior rentabilidade."
                }
            );

            
            modelBuilder.Entity<ProdutoEntity>().HasData(
                new ProdutoEntity
                {
                    Id = 1,
                    PerfilRiscoId = 1,
                    PerfilRisco = null!,
                    Nome = "CDB Prefixado Caixa Liquidez diária",
                    Tipo = "CDB",
                    Rentabilidade = 0.085,
                    Risco = EnumRiscoInvestimento.Baixo.ToString(),
                    PrazoMinimoResgateMeses = 0
                },
                new ProdutoEntity
                {
                    Id = 2,
                    PerfilRiscoId = 1,
                    PerfilRisco = null!,
                    Nome = "Tesouro SELIC",
                    Tipo = "TesouroDireto",
                    Rentabilidade = 0.15,
                    Risco = EnumRiscoInvestimento.Baixo.ToString(),
                    PrazoMinimoResgateMeses = 0
                },
                new ProdutoEntity
                {
                    Id = 3,
                    PerfilRiscoId = 1,
                    PerfilRisco = null!,
                    Nome = "CDB Prefixado Caixa Resgate 2 Anos",
                    Tipo = "CDB",
                    Rentabilidade = 0.1,
                    Risco = EnumRiscoInvestimento.Baixo.ToString(),
                    PrazoMinimoResgateMeses = 24
                },
                new ProdutoEntity
                {
                    Id = 4,
                    PerfilRiscoId = 2,
                    PerfilRisco = null!,
                    Nome = "LCI Caixa",
                    Tipo = "LCI",
                    Rentabilidade = 0.128,
                    Risco = EnumRiscoInvestimento.Medio.ToString(),
                    PrazoMinimoResgateMeses = 3
                },
                new ProdutoEntity
                {
                    Id = 5,
                    PerfilRiscoId = 2,
                    PerfilRisco = null!,
                    Nome = "Fundo de Investimento Caixa Multimercado",
                    Tipo = "Fundo",
                    Rentabilidade = 0.12,
                    Risco = EnumRiscoInvestimento.Medio.ToString(),
                    PrazoMinimoResgateMeses = 6
                },
                new ProdutoEntity
                {
                    Id = 6,
                    PerfilRiscoId = 3,
                    PerfilRisco = null!,
                    Nome = "FII Caixa Agências",
                    Tipo = "FII",
                    Rentabilidade = 0.15,
                    Risco = EnumRiscoInvestimento.Alto.ToString(),
                    PrazoMinimoResgateMeses = 0
                },
                new ProdutoEntity
                {
                    Id = 7,
                    PerfilRiscoId = 3,
                    PerfilRisco = null!,
                    Nome = "Ações Caixa Seguridade",
                    Tipo = "Ações",
                    Rentabilidade = 0.18,
                    Risco = EnumRiscoInvestimento.Alto.ToString(),
                    PrazoMinimoResgateMeses = 0
                }
            );
            
            modelBuilder.Entity<ClienteEntity>().HasData(
                new ClienteEntity
                {
                    Id = 1,
                    Pontuacao = 50,
                    Investimentos = []
                }
            );

           
            modelBuilder.Entity<InvestimentoEntity>().HasData(
                new InvestimentoEntity
                {
                    Id = 1,
                    ClienteId = 1,
                    Cliente = null!,
                    Tipo = "CDB",
                    Valor = 10000.00,
                    Rentabilidade = 0.085,
                    Data = new DateOnly(2025, 1, 1)
                }
            );

            modelBuilder.Entity<SimulacaoEntity>().HasData(
                new SimulacaoEntity
                {
                    Id = 1,
                    ClienteId = 1,
                    Cliente = null!,
                    ProdutoId = 1,
                    Produto = null!,
                    ValorInvestido = 10000.00,
                    ValorFinal = 10850.00,
                    PrazoMeses = 12,
                    DataSimulacao = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}