using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClienteAPI_.Migrations
{
    /// <inheritdoc />
    public partial class ViaCepDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CLI_DATA_CADASTRO",
                table: "T_CLIENTES",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "T_VIACEP",
                columns: table => new
                {
                    VIA_CEP = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    VIA_LOGRADOURO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIA_COMPLEMENTO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIA_BAIRRO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIA_LOCALIDADE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIA_UF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIA_IBGE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIA_GIA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIA_DDD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIA_SIAFI = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VIACEP", x => x.VIA_CEP);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_VIACEP");

            migrationBuilder.AlterColumn<string>(
                name: "CLI_DATA_CADASTRO",
                table: "T_CLIENTES",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
