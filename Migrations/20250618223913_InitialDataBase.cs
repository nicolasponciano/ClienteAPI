using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClienteAPI_.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_CLIENTES",
                columns: table => new
                {
                    CLI_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLI_NOME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CLI_DATA_CADASTRO = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLIENTES", x => x.CLI_ID);
                });

            migrationBuilder.CreateTable(
                name: "T_CONTATO",
                columns: table => new
                {
                    CTT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CTT_TIPO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CTT_TEXTO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CTT_ID_CLIENTE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CONTATO", x => x.CTT_ID);
                    table.ForeignKey(
                        name: "FK_T_CONTATO_T_CLIENTES_CTT_ID_CLIENTE",
                        column: x => x.CTT_ID_CLIENTE,
                        principalTable: "T_CLIENTES",
                        principalColumn: "CLI_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_ENDERECO",
                columns: table => new
                {
                    END_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    END_CEP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    END_LOGRADOURO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    END_CIDADE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    END_NUMERO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    END_COMPLEMENTO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    END_ID_CLIENTE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ENDERECO", x => x.END_ID);
                    table.ForeignKey(
                        name: "FK_T_ENDERECO_T_CLIENTES_END_ID_CLIENTE",
                        column: x => x.END_ID_CLIENTE,
                        principalTable: "T_CLIENTES",
                        principalColumn: "CLI_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_CONTATO_CTT_ID_CLIENTE",
                table: "T_CONTATO",
                column: "CTT_ID_CLIENTE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_ENDERECO_END_ID_CLIENTE",
                table: "T_ENDERECO",
                column: "END_ID_CLIENTE",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_CONTATO");

            migrationBuilder.DropTable(
                name: "T_ENDERECO");

            migrationBuilder.DropTable(
                name: "T_CLIENTES");
        }
    }
}
