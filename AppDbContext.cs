using DevExpress.Data.Async.Helpers;
using ERPNext_PowerPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ERPNext_PowerPlay
{
    public class AppDbContext : DbContext
    {
        public DbSet<CredList> Credentials { get; set; }
        public DbSet<Cred> Creds { get; set; }
        public DbSet<PrinterSetting> PrinterSetting { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }

        public string DbPath { get; }
        public AppDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "ERPNext_PowerPlay.db"); //localappdata + productname, doesn't work here.
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrinterSetting>()
                .Property(e => e.DateCreated)
                .HasDefaultValueSql("date('now')");

            modelBuilder.Entity<PrinterSetting>()
                .Property(e => e.DateModified)
                .HasDefaultValueSql("date('now')");
        }
        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entity in modifiedEntities)
            {
                entity.Property("DateModified").CurrentValue = DateTime.Now;
            }

            return base.SaveChanges();
        }


    }


}
