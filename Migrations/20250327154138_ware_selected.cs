using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPNext_PowerPlay.Migrations
{
    /// <inheritdoc />
    public partial class ware_selected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "selected",
                table: "Warehouse",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "selected",
                table: "Warehouse");
        }
    }
}
