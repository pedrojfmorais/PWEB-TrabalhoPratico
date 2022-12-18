using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoPratico.Data.Migrations
{
    public partial class dois : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empresa_empresaId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "empresaId",
                table: "AspNetUsers",
                newName: "EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_empresaId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_EmpresaId");

            migrationBuilder.CreateTable(
                name: "CategoriaVeiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaVeiculo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Localizacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quilometros = table.Column<int>(type: "int", nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: false),
                    PrecoDia = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    LocalizacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veiculo_CategoriaVeiculo_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriaVeiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Veiculo_Localizacao_LocalizacaoId",
                        column: x => x.LocalizacaoId,
                        principalTable: "Localizacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_CategoriaId",
                table: "Veiculo",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_LocalizacaoId",
                table: "Veiculo",
                column: "LocalizacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresa_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empresa_EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Veiculo");

            migrationBuilder.DropTable(
                name: "CategoriaVeiculo");

            migrationBuilder.DropTable(
                name: "Localizacao");

            migrationBuilder.RenameColumn(
                name: "EmpresaId",
                table: "AspNetUsers",
                newName: "empresaId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_EmpresaId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_empresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresa_empresaId",
                table: "AspNetUsers",
                column: "empresaId",
                principalTable: "Empresa",
                principalColumn: "Id");
        }
    }
}
