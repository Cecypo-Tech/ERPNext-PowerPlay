using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPNext_PowerPlay.Migrations
{
    /// <inheritdoc />
    public partial class rpx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "REPX_Template",
                table: "PrinterSetting",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "REPX_Template",
                table: "PrinterSetting");
        }
    }
}
