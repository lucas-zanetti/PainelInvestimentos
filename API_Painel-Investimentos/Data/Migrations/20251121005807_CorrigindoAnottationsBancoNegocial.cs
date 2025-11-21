using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API_Painel_Investimentos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrigindoAnottationsBancoNegocial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Investimentos",
                keyColumn: "Id",
                keyValue: 1ul);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 2u);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 3u);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 4u);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 5u);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 6u);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 7u);

            migrationBuilder.DeleteData(
                table: "Simulacoes",
                keyColumn: "Id",
                keyValue: 1ul);

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: 1u);

            migrationBuilder.DeleteData(
                table: "PerfisRisco",
                keyColumn: "Id",
                keyValue: (ushort)2);

            migrationBuilder.DeleteData(
                table: "PerfisRisco",
                keyColumn: "Id",
                keyValue: (ushort)3);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 1u);

            migrationBuilder.DeleteData(
                table: "PerfisRisco",
                keyColumn: "Id",
                keyValue: (ushort)1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Pontuacao" },
                values: new object[] { 1u, (ushort)50 });

            migrationBuilder.InsertData(
                table: "PerfisRisco",
                columns: new[] { "Id", "DescricaoPerfil", "NomePerfil" },
                values: new object[,]
                {
                    { (ushort)1, "Baixa exposição a risco, foco em preservação de capital.", "Conservador" },
                    { (ushort)2, "Equilíbrio entre risco e retorno.", "Moderado" },
                    { (ushort)3, "Alta exposição a risco, busca maior rentabilidade.", "Agressivo" }
                });

            migrationBuilder.InsertData(
                table: "Investimentos",
                columns: new[] { "Id", "ClienteId", "Data", "Rentabilidade", "Tipo", "Valor" },
                values: new object[] { 1ul, 1u, new DateOnly(2025, 1, 1), 0.085000000000000006, "CDB", 10000.0 });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Nome", "PerfilRiscoId", "PrazoMinimoResgateMeses", "Rentabilidade", "Risco", "Tipo" },
                values: new object[,]
                {
                    { 1u, "CDB Prefixado Caixa Liquidez diária", (ushort)1, (ushort)0, 0.085000000000000006, "Baixo", "CDB" },
                    { 2u, "Tesouro SELIC", (ushort)1, (ushort)0, 0.14999999999999999, "Baixo", "TesouroDireto" },
                    { 3u, "CDB Prefixado Caixa Resgate 2 Anos", (ushort)1, (ushort)24, 0.10000000000000001, "Baixo", "CDB" },
                    { 4u, "LCI Caixa", (ushort)2, (ushort)3, 0.128, "Medio", "LCI" },
                    { 5u, "Fundo de Investimento Caixa Multimercado", (ushort)2, (ushort)6, 0.12, "Medio", "Fundo" },
                    { 6u, "FII Caixa Agências", (ushort)3, (ushort)0, 0.14999999999999999, "Alto", "FII" },
                    { 7u, "Ações Caixa Seguridade", (ushort)3, (ushort)0, 0.17999999999999999, "Alto", "Ações" }
                });

            migrationBuilder.InsertData(
                table: "Simulacoes",
                columns: new[] { "Id", "ClienteId", "DataSimulacao", "PrazoMeses", "ProdutoId", "ValorFinal", "ValorInvestido" },
                values: new object[] { 1ul, 1u, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), (ushort)12, 1u, 10850.0, 10000.0 });
        }
    }
}
