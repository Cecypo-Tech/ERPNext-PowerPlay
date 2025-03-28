using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPNext_PowerPlay.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PrinterSetting",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WarehouseFilter = table.Column<string>(type: "TEXT", nullable: true),
                    PrintEngine = table.Column<int>(type: "INTEGER", nullable: false),
                    DocType = table.Column<int>(type: "INTEGER", nullable: false),
                    CopyName = table.Column<string>(type: "TEXT", nullable: true),
                    FontSize = table.Column<int>(type: "INTEGER", nullable: true),
                    Orientation = table.Column<int>(type: "INTEGER", nullable: false),
                    Scaling = table.Column<int>(type: "INTEGER", nullable: false),
                    Printer = table.Column<string>(type: "TEXT", nullable: true),
                    Copies = table.Column<int>(type: "INTEGER", nullable: true),
                    PageRange = table.Column<string>(type: "TEXT", nullable: true),
                    FrappeTemplateName = table.Column<string>(type: "TEXT", nullable: true),
                    LetterHead = table.Column<string>(type: "TEXT", nullable: true),
                    Compact = table.Column<bool>(type: "INTEGER", nullable: false),
                    UOM = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "date('now')"),
                    DateModified = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "date('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterSetting", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Creds",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    URL = table.Column<string>(type: "TEXT", nullable: false),
                    User = table.Column<string>(type: "TEXT", nullable: false),
                    Pass = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Secret = table.Column<string>(type: "TEXT", nullable: false),
                    CredListID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creds", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Creds_Credentials_CredListID",
                        column: x => x.CredListID,
                        principalTable: "Credentials",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Creds_CredListID",
                table: "Creds",
                column: "CredListID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Creds");

            migrationBuilder.DropTable(
                name: "PrinterSetting");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Credentials");
        }
    }
}
