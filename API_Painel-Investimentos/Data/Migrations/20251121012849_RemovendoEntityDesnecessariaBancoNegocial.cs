using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Painel_Investimentos.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovendoEntityDesnecessariaBancoNegocial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investimentos");

            migrationBuilder.AddColumn<uint>(
                name: "ClienteEntityId",
                table: "Simulacoes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rentabilidade",
                table: "Simulacoes",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Simulacoes_ClienteEntityId",
                table: "Simulacoes",
                column: "ClienteEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Simulacoes_Clientes_ClienteEntityId",
                table: "Simulacoes",
                column: "ClienteEntityId",
                principalTable: "Clientes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Simulacoes_Clientes_ClienteEntityId",
                table: "Simulacoes");

            migrationBuilder.DropIndex(
                name: "IX_Simulacoes_ClienteEntityId",
                table: "Simulacoes");

            migrationBuilder.DropColumn(
                name: "ClienteEntityId",
                table: "Simulacoes");

            migrationBuilder.DropColumn(
                name: "Rentabilidade",
                table: "Simulacoes");

            migrationBuilder.CreateTable(
                name: "Investimentos",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Data = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Rentabilidade = table.Column<double>(type: "REAL", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Valor = table.Column<double>(type: "REAL", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Investimentos_ClienteId",
                table: "Investimentos",
                column: "ClienteId");
        }
    }
}
