using API_Painel_Investimentos.Data.Contexts;
using API_Painel_Investimentos.Data.Entities;
using API_Painel_Investimentos.Enums;
using Microsoft.AspNetCore.Identity;

namespace API_Painel_Investimentos.Data.Migrations;

public static class DbInitializer
{
    public static void SeedPainelInvestimentoContext(DbPainelInvestimentoContext context, IServiceProvider services)
    {
        if (context.PerfisRisco.Any())
        {
            Console.WriteLine("Tabela PerfisRisco já contém dados. Seeding ignorado.");
            return;
        }

        Console.WriteLine("Iniciando Seeding condicional para DbPainelInvestimentoContext...");
        
        #region Perfis de Risco
        var perfisRisco = new PerfilRiscoEntity[]
        {
            new()
            {
                Id = 1,
                NomePerfil = EnumPerfilRisco.Conservador.ToString(),
                DescricaoPerfil = "Baixa exposição a risco, foco em preservação de capital."
            },
            new()
            {
                Id = 2,
                NomePerfil = EnumPerfilRisco.Moderado.ToString(),
                DescricaoPerfil = "Equilíbrio entre risco e retorno."
            },
            new()
            {
                Id = 3,
                NomePerfil = EnumPerfilRisco.Agressivo.ToString(),
                DescricaoPerfil = "Alta exposição a risco, busca maior rentabilidade."
            }
        };
        #endregion

        #region Produtos
        var produtos = new ProdutoEntity[]
        {
            new() { Id = 1, PerfilRiscoId = 1, Nome = "CDB Prefixado Caixa Liquidez diária", Tipo = "CDB", Rentabilidade = 0.085, Risco = EnumRiscoInvestimento.Baixo.ToString(), PrazoMinimoResgateMeses = 0, PerfilRisco = null! },
            new() { Id = 2, PerfilRiscoId = 1, Nome = "Tesouro SELIC", Tipo = "TesouroDireto", Rentabilidade = 0.15, Risco = EnumRiscoInvestimento.Baixo.ToString(), PrazoMinimoResgateMeses = 0, PerfilRisco = null! },
            new() { Id = 3, PerfilRiscoId = 1, Nome = "CDB Prefixado Caixa Resgate 2 Anos", Tipo = "CDB", Rentabilidade = 0.1, Risco = EnumRiscoInvestimento.Baixo.ToString(), PrazoMinimoResgateMeses = 24, PerfilRisco = null! },
            new() { Id = 4, PerfilRiscoId = 2, Nome = "LCI Caixa", Tipo = "LCI", Rentabilidade = 0.128, Risco = EnumRiscoInvestimento.Medio.ToString(), PrazoMinimoResgateMeses = 3, PerfilRisco = null! },
            new() { Id = 5, PerfilRiscoId = 2, Nome = "Fundo de Investimento Caixa Multimercado", Tipo = "Fundo", Rentabilidade = 0.12, Risco = EnumRiscoInvestimento.Medio.ToString(), PrazoMinimoResgateMeses = 6, PerfilRisco = null! },
            new() { Id = 6, PerfilRiscoId = 3, Nome = "FII Caixa Agências", Tipo = "FII", Rentabilidade = 0.15, Risco = EnumRiscoInvestimento.Alto.ToString(), PrazoMinimoResgateMeses = 0, PerfilRisco = null! },
            new() { Id = 7, PerfilRiscoId = 3, Nome = "Ações Caixa Seguridade", Tipo = "Ações", Rentabilidade = 0.18, Risco = EnumRiscoInvestimento.Alto.ToString(), PrazoMinimoResgateMeses = 0, PerfilRisco = null! }
        };
        #endregion

        #region Clientes
        var clientes = new ClienteEntity[]
        {
            new() { Id = 1, Pontuacao = 50, Investimentos = [] }
        };
        #endregion

        #region Simulações
        var simulacoes = new SimulacaoEntity[]
        {
            new() { Id = 1, ClienteId = 1, ProdutoId = 1, ValorInvestido = 10000.00, ValorFinal = 10850.00, PrazoMeses = 12, DataSimulacao = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc), Cliente = null!, Produto = null! }
        };
        #endregion

        context.PerfisRisco.AddRange(perfisRisco);
        context.Produtos.AddRange(produtos);
        context.Clientes.AddRange(clientes);
        context.Simulacoes.AddRange(simulacoes);

        context.SaveChanges();
        Console.WriteLine("Seeding de dados concluído com sucesso.");
    }

    public static void SeedUsuarioContext(DbUsuarioContext context, IServiceProvider services)
    {
        if (context.Users.Any())
        {
            Console.WriteLine("Tabela de Usuários já contém dados. Seeding de Admin ignorado.");
            return;
        }

        Console.WriteLine("Iniciando Seeding condicional para DbUsuarioContext (usuário Admin)...");

        var hasher = new PasswordHasher<UsuarioEntity>();

        var admin = new UsuarioEntity
        {
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            Role = (int)UsuarioRoleEnum.Admin
        };

        admin.PasswordHash = hasher.HashPassword(admin, "admin");

        context.Users.Add(admin);

        context.SaveChanges();
        Console.WriteLine("Usuário 'Admin' inserido com sucesso.");
    }
}