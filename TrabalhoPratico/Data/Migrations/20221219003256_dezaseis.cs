using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoPratico.Data.Migrations
{
    public partial class dezaseis : Migration
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
                name: "IX_Reserva_ReservaEstadoVeiculoEntregaId",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_ReservaEstadoVeiculoLevantamentoId",
                table: "Reserva");

            migrationBuilder.AddColumn<int>(
                name: "ReservaId",
                table: "ReservaEstadoVeiculoLevantamento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservaId",
                table: "ReservaEstadoVeiculoEntrega",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaEstadoVeiculoEntrega_Reserva_ReservaId",
                table: "ReservaEstadoVeiculoEntrega",
                column: "ReservaId",
                principalTable: "Reserva",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaEstadoVeiculoLevantamento_Reserva_ReservaId",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "ReservaId",
                principalTable: "Reserva",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoEntrega_Reserva_ReservaId",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoLevantamento_Reserva_ReservaId",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_ReservaId",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_ReservaId",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.DropColumn(
                name: "ReservaId",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropColumn(
                name: "ReservaId",
                table: "ReservaEstadoVeiculoEntrega");

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
