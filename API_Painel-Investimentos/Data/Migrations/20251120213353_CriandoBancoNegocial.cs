using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API_Painel_Investimentos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriandoBancoNegocial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false),
                    Pontuacao = table.Column<ushort>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfisRisco",
                columns: table => new
                {
                    Id = table.Column<ushort>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomePerfil = table.Column<string>(type: "TEXT", nullable: false),
                    DescricaoPerfil = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfisRisco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Investimentos",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Valor = table.Column<double>(type: "REAL", nullable: false),
                    Rentabilidade = table.Column<double>(type: "REAL", nullable: false),
                    Data = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investimentos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PerfilRiscoId = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Rentabilidade = table.Column<double>(type: "REAL", nullable: false),
                    Risco = table.Column<string>(type: "TEXT", nullable: false),
                    PrazoMinimoResgateMeses = table.Column<ushort>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_PerfisRisco_PerfilRiscoId",
                        column: x => x.PerfilRiscoId,
                        principalTable: "PerfisRisco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Simulacoes",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<uint>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<uint>(type: "INTEGER", nullable: false),
                    ValorInvestido = table.Column<double>(type: "REAL", nullable: false),
                    ValorFinal = table.Column<double>(type: "REAL", nullable: false),
                    PrazoMeses = table.Column<ushort>(type: "INTEGER", nullable: false),
                    DataSimulacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Simulacoes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Simulacoes_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Investimentos_ClienteId",
                table: "Investimentos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_PerfilRiscoId",
                table: "Produtos",
                column: "PerfilRiscoId");

            migrationBuilder.CreateIndex(
                name: "IX_Simulacoes_ClienteId",
                table: "Simulacoes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Simulacoes_ProdutoId",
                table: "Simulacoes",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investimentos");

            migrationBuilder.DropTable(
                name: "Simulacoes");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "PerfisRisco");
        }
    }
}
