using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoPratico.Data.Migrations
{
    public partial class onze : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoEntrega_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoLevantamento_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.AlterColumn<string>(
                name: "FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoEntrega_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservaEstadoVeiculoLevantamento_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento");

            migrationBuilder.AlterColumn<string>(
                name: "FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaEstadoVeiculoEntrega_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoEntrega",
                column: "FuncionarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaEstadoVeiculoLevantamento_AspNetUsers_FuncionarioId1",
                table: "ReservaEstadoVeiculoLevantamento",
                column: "FuncionarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
