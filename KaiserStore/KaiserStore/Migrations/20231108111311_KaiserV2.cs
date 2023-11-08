using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaiserStore.Migrations
{
    /// <inheritdoc />
    public partial class KaiserV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sold",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sold",
                table: "products");
        }
    }
}
