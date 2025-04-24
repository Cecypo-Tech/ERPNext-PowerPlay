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
                name: "Creds",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    URL = table.Column<string>(type: "TEXT", nullable: false),
                    User = table.Column<string>(type: "TEXT", nullable: true),
                    Pass = table.Column<string>(type: "TEXT", nullable: true),
                    APIKey = table.Column<string>(type: "TEXT", nullable: true),
                    Secret = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creds", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JobHistory",
                columns: table => new
                {
                    Job = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DocType = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Owner = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Grand_Total = table.Column<double>(type: "REAL", nullable: false),
                    custom_print_count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobHistory", x => x.Job);
                });

            migrationBuilder.CreateTable(
                name: "PrinterSetting",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DocType = table.Column<int>(type: "INTEGER", nullable: false),
                    WarehouseFilter = table.Column<string>(type: "TEXT", nullable: true),
                    UserFilter = table.Column<string>(type: "TEXT", nullable: true),
                    PrintEngine = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldList = table.Column<string>(type: "TEXT", nullable: true),
                    FilterList = table.Column<string>(type: "TEXT", nullable: true),
                    Printer = table.Column<string>(type: "TEXT", nullable: true),
                    CopyName = table.Column<string>(type: "TEXT", nullable: true),
                    Copies = table.Column<int>(type: "INTEGER", nullable: false),
                    PageRange = table.Column<string>(type: "TEXT", nullable: true),
                    FontSize = table.Column<int>(type: "INTEGER", nullable: true),
                    Orientation = table.Column<int>(type: "INTEGER", nullable: false),
                    Scaling = table.Column<int>(type: "INTEGER", nullable: false),
                    REPX_Template = table.Column<string>(type: "TEXT", nullable: true),
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
                name: "ReportList",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReportName = table.Column<string>(type: "TEXT", nullable: false),
                    DocType = table.Column<int>(type: "INTEGER", nullable: false),
                    EndPoint = table.Column<string>(type: "TEXT", nullable: false),
                    FieldList = table.Column<string>(type: "TEXT", nullable: false),
                    FilterList = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportList", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    selected = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    selected = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobHistory_DocType_Name_Date",
                table: "JobHistory",
                columns: new[] { "DocType", "Name", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Creds");

            migrationBuilder.DropTable(
                name: "JobHistory");

            migrationBuilder.DropTable(
                name: "PrinterSetting");

            migrationBuilder.DropTable(
                name: "ReportList");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Warehouse");
        }
    }
}
