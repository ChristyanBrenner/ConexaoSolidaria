using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CAMPANHA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TITULO = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DATA_INICIO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DATA_FIM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    META_FINANCEIRA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VALOR_TOTAL_ARRECADADO = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAMPANHA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME_COMPLETO = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    SENHA_HASH = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ROLE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DOACAO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USUARIO_ID = table.Column<int>(type: "int", nullable: false),
                    CAMPANHA_ID = table.Column<int>(type: "int", nullable: false),
                    VALOR_DOACAO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DATA_PROCESSAMENTO = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOACAO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DOACAO_CAMPANHA_CAMPANHA_ID",
                        column: x => x.CAMPANHA_ID,
                        principalTable: "CAMPANHA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DOACAO_USUARIO_USUARIO_ID",
                        column: x => x.USUARIO_ID,
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DOACAO_CAMPANHA_ID",
                table: "DOACAO",
                column: "CAMPANHA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DOACAO_USUARIO_ID",
                table: "DOACAO",
                column: "USUARIO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_CPF",
                table: "USUARIO",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_EMAIL",
                table: "USUARIO",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DOACAO");

            migrationBuilder.DropTable(
                name: "CAMPANHA");

            migrationBuilder.DropTable(
                name: "USUARIO");
        }
    }
}
