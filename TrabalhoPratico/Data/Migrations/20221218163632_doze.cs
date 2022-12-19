using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoPratico.Data.Migrations
{
    public partial class doze : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoEntrega_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoLevantamento_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.DropColumn(
                name: "FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropColumn(
                name: "FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.AlterColumn<string>(
                name: "FuncionarioId",
                table: "ReservaEstadoVeiculoLevantamento",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "FuncionarioId",
                table: "ReservaEstadoVeiculoEntrega",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_FuncionarioId",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_FuncionarioId",
                table: "ReservaEstadoVeiculoEntrega",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaEstadoVeiculoEntrega_AspNetUsers_FuncionarioId",
                table: "ReservaEstadoVeiculoEntrega",
                column: "FuncionarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaEstadoVeiculoLevantamento_AspNetUsers_FuncionarioId",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "FuncionarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoEntrega_AspNetUsers_FuncionarioId",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoLevantamento_AspNetUsers_FuncionarioId",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_FuncionarioId",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.DropIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_FuncionarioId",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "ReservaEstadoVeiculoLevantamento",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "ReservaEstadoVeiculoEntrega",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoLevantamento_FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "FuncionarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaEstadoVeiculoEntrega_FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega",
                column: "FuncionarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaEstadoVeiculoEntrega_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega",
                column: "FuncionarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaEstadoVeiculoLevantamento_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "FuncionarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
