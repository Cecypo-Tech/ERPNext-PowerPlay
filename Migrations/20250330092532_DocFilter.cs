using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPNext_PowerPlay.Migrations
{
    /// <inheritdoc />
    public partial class DocFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocFilter",
                table: "PrinterSetting",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocFilter",
                table: "PrinterSetting");
        }
    }
}
