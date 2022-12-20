using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoPratico.Migrations
{
    public partial class dois : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_ReservaEstadoVeiculoEntrega_ReservaEstadoVeiculoEntregaId",
                table: "Reserva");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_ReservaEstadoVeiculoLevantamento_ReservaEstadoVeiculoLevantamentoId",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_ReservaId",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_ReservaId",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_ReservaEstadoVeiculoEntregaId",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_ReservaEstadoVeiculoLevantamentoId",
                table: "Reserva");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_ReservaId",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "ReservaId",
                unique: true,
                filter: "[ReservaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_ReservaId",
                table: "ReservaEstadoVeiculoEntrega",
                column: "ReservaId",
                unique: true,
                filter: "[ReservaId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_ReservaId",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_ReservaId",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_ReservaId",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_ReservaId",
                table: "ReservaEstadoVeiculoEntrega",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_ReservaEstadoVeiculoEntregaId",
                table: "Reserva",
                column: "ReservaEstadoVeiculoEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_ReservaEstadoVeiculoLevantamentoId",
                table: "Reserva",
                column: "ReservaEstadoVeiculoLevantamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_ReservaEstadoVeiculoEntrega_ReservaEstadoVeiculoEntregaId",
                table: "Reserva",
                column: "ReservaEstadoVeiculoEntregaId",
                principalTable: "ReservaEstadoVeiculoEntrega",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_ReservaEstadoVeiculoLevantamento_ReservaEstadoVeiculoLevantamentoId",
                table: "Reserva",
                column: "ReservaEstadoVeiculoLevantamentoId",
                principalTable: "ReservaEstadoVeiculoLevantamento",
                principalColumn: "Id");
        }
    }
}
