using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoPratico.Data.Migrations
{
    public partial class cinco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nome",
                table: "Empresa",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "estadoSubscricao",
                table: "Empresa",
                newName: "EstadoSubscricao");

            migrationBuilder.RenameColumn(
                name: "classificacao",
                table: "Empresa",
                newName: "Classificacao");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Veiculo",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Classificacao",
                table: "Empresa",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_EmpresaId",
                table: "Veiculo",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculo_Empresa_EmpresaId",
                table: "Veiculo",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculo_Empresa_EmpresaId",
                table: "Veiculo");

            migrationBuilder.DropIndex(
                name: "IX_Veiculo_EmpresaId",
                table: "Veiculo");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Veiculo");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Empresa",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "EstadoSubscricao",
                table: "Empresa",
                newName: "estadoSubscricao");

            migrationBuilder.RenameColumn(
                name: "Classificacao",
                table: "Empresa",
                newName: "classificacao");

            migrationBuilder.AlterColumn<double>(
                name: "classificacao",
                table: "Empresa",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
