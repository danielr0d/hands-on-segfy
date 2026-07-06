using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Segfy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ramos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ramos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Apolices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    RamoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apolices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apolices_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Apolices_Ramos_RamoId",
                        column: x => x.RamoId,
                        principalTable: "Ramos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sinistros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApoliceId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ValorEstimado = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ValorAprovado = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MotivoNegativa = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sinistros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sinistros_Apolices_ApoliceId",
                        column: x => x.ApoliceId,
                        principalTable: "Apolices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoSinistros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SinistroId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusAnterior = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StatusNovo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoSinistros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoSinistros_Sinistros_SinistroId",
                        column: x => x.SinistroId,
                        principalTable: "Sinistros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apolices_ClienteId",
                table: "Apolices",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Apolices_Numero",
                table: "Apolices",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Apolices_RamoId",
                table: "Apolices",
                column: "RamoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSinistros_SinistroId",
                table: "HistoricoSinistros",
                column: "SinistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Sinistros_ApoliceId",
                table: "Sinistros",
                column: "ApoliceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoSinistros");

            migrationBuilder.DropTable(
                name: "Sinistros");

            migrationBuilder.DropTable(
                name: "Apolices");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Ramos");
        }
    }
}
