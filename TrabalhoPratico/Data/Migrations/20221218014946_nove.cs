using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoPratico.Data.Migrations
{
    public partial class nove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_AspNetUsers_ClienteId1",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_ClienteId1",
                table: "Reserva");

            migrationBuilder.DropColumn(
                name: "ClienteId1",
                table: "Reserva");

            migrationBuilder.AlterColumn<string>(
                name: "ClienteId",
                table: "Reserva",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_ClienteId",
                table: "Reserva",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_AspNetUsers_ClienteId",
                table: "Reserva",
                column: "ClienteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_AspNetUsers_ClienteId",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_ClienteId",
                table: "Reserva");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Reserva",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ClienteId1",
                table: "Reserva",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_ClienteId1",
                table: "Reserva",
                column: "ClienteId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_AspNetUsers_ClienteId1",
                table: "Reserva",
                column: "ClienteId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
