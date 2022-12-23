using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoPratico.Migrations
{
    public partial class tres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classificacao",
                table: "Empresa");

            migrationBuilder.CreateTable(
                name: "Classificacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    ReservaId = table.Column<int>(type: "int", nullable: true),
                    ClassificacaoReserva = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classificacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classificacao_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Classificacao_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classificacao_Reserva_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reserva",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classificacao_ApplicationUserId",
                table: "Classificacao",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Classificacao_EmpresaId",
                table: "Classificacao",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Classificacao_ReservaId",
                table: "Classificacao",
                column: "ReservaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Classificacao");

            migrationBuilder.AddColumn<int>(
                name: "Classificacao",
                table: "Empresa",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
