using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoPratico.Data.Migrations
{
    public partial class dezanove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservaEstadoVeiculoEntregaId",
                table: "Reserva",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservaEstadoVeiculoLevantamentoId",
                table: "Reserva",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReservaEstadoVeiculoEntrega",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quilometros = table.Column<int>(type: "int", nullable: false),
                    DanosVeiculo = table.Column<bool>(type: "bit", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuncionarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReservaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaEstadoVeiculoEntrega", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservaEstadoVeiculoEntrega_AspNetUsers_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaEstadoVeiculoEntrega_Reserva_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reserva",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReservaEstadoVeiculoLevantamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quilometros = table.Column<int>(type: "int", nullable: false),
                    DanosVeiculo = table.Column<bool>(type: "bit", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuncionarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReservaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaEstadoVeiculoLevantamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservaEstadoVeiculoLevantamento_AspNetUsers_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaEstadoVeiculoLevantamento_Reserva_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reserva",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_FuncionarioId",
                table: "ReservaEstadoVeiculoEntrega",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_ReservaId",
                table: "ReservaEstadoVeiculoEntrega",
                column: "ReservaId",
                unique: true,
                filter: "[ReservaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_FuncionarioId",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_ReservaId",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "ReservaId",
                unique: true,
                filter: "[ReservaId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.DropTable(
                name: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropColumn(
                name: "ReservaEstadoVeiculoEntregaId",
                table: "Reserva");

            migrationBuilder.DropColumn(
                name: "ReservaEstadoVeiculoLevantamentoId",
                table: "Reserva");
        }
    }
}
