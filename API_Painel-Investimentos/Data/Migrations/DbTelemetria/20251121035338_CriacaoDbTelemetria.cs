using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Painel_Investimentos.Data.Migrations.DbTelemetria
{
    /// <inheritdoc />
    public partial class CriacaoDbTelemetria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DadosTelemetria",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataRequisicao = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CodEndpoint = table.Column<ushort>(type: "INTEGER", nullable: false),
                    TempoResposta = table.Column<ushort>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosTelemetria", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DadosTelemetria");
        }
    }
}
