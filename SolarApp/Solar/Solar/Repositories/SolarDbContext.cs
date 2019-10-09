using Microsoft.EntityFrameworkCore;
using Solar.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
namespace Solar.Repositories
{
    public class SolarDbContext:DbContext
    {
        public string DbPath { get; set; }

        #region Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Panel> Panels { get; set; }
        public DbSet<DataPanel> Datas { get; set; }
        public DbSet<Reading> Readings { get; set; }
        #endregion

        public SolarDbContext():base()
        {
            DbPath = Path.Combine(FileSystem.AppDataDirectory, "SolarDb.sqlite");
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Panel>().HasKey(p => p.Id);
            modelBuilder.Entity<DataPanel>().HasKey(p => p.Id);
            modelBuilder.Entity<Reading>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().HasKey(p => p.Id);
        }
    }
}
