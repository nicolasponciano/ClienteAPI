using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClienteAPI_.Migrations
{
    /// <inheritdoc />
    public partial class DataCadastroString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CLI_DATA_CADASTRO",
                table: "T_CLIENTES",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CLI_DATA_CADASTRO",
                table: "T_CLIENTES",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
