using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatusFieldToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Alterar a coluna Status de integer para varchar
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Books",
                type: "character varying(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverter a coluna Status de varchar para integer
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Books",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)");
        }
    }
}
