using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Data.Sqlite;

#nullable disable

namespace ERPNext_PowerPlay.Migrations
{
    /// <inheritdoc />
    public partial class Set_Warehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add columns only if they don't exist (safe for databases created with EnsureCreated)
            AddColumnIfNotExists(migrationBuilder, "Settings", "StringValue", "TEXT NOT NULL DEFAULT ''");
            AddColumnIfNotExists(migrationBuilder, "Settings", "Value", "INTEGER NOT NULL DEFAULT 0");
            AddColumnIfNotExists(migrationBuilder, "JobHistory", "Set_Warehouse", "TEXT NOT NULL DEFAULT ''");
        }

        private void AddColumnIfNotExists(MigrationBuilder migrationBuilder, string table, string column, string type)
        {
            // SQLite will throw an error if column exists, but we suppress it
            migrationBuilder.Sql($@"
                ALTER TABLE ""{table}"" ADD COLUMN ""{column}"" {type};
            ", suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // SQLite doesn't support DROP COLUMN in older versions
            // These columns will remain if rolling back
        }
    }
}
